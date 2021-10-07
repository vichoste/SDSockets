using System.Text;

using EzSockets;

var server = new EzSocketListener(new EzEventsListener() {
	OnNewConnectionHandler = (EzSocket socket) => {
		Console.WriteLine($"I have a new connection");
		socket.StartReadingMessages();
	},
	OnConnectionClosedHandler = (EzSocket socket) => {
		Console.WriteLine($"My connection has been closed");
		socket.StopReadingMessages();
	},
	OnMessageSendHandler = (EzSocket socket, byte[] data) => {
		Console.WriteLine($"I've sent data: {Encoding.Default.GetString(data)}");
	},
	OnExceptionHandler = (EzSocket socket, Exception ex) => {
		Console.WriteLine("My connection has been forcefully closed");
		return ExceptionHandlerResponse.CloseSocket;
	},
	OnMessageReadHandler = (EzSocket socket, byte[] data) => {
		var receivedData = Encoding.Default.GetString(data).Split(' ');
		var operation = receivedData[0] is "0" ? "Addition" : receivedData[0] is "1" ? "Subtraction" : receivedData[0] is "2" ? "Multiplication" : receivedData[0] is "3" ? "Division" : "None";
		var a = receivedData[1];
		var b = receivedData[2];
		Console.WriteLine($"I got new data:");
		Console.WriteLine($"Operation: {operation}");
		Console.WriteLine($"a: {a}");
		Console.WriteLine($"b: {b}");
		if (operation is "Division" && b is "0") {
			socket.SendMessage($"Division by zero");
		} else {
			switch (operation) {
				case "Addition":
					socket.SendMessage($"{(int.Parse(a) + int.Parse(b))}");
					break;
				case "Subtraction":
					socket.SendMessage($"{(int.Parse(a) - int.Parse(b))}");
					break;
				case "Multiplication":
					socket.SendMessage($"{(int.Parse(a) * int.Parse(b))}");
					break;
				case "Division":
					socket.SendMessage($"{(int.Parse(a) / int.Parse(b))}");
					break;
			}
		}
	},
});

server.Listen(5000);
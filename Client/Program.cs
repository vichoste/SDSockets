using System.Text;

using EzSockets;

var socket = new EzSocket(null, 5000, new EzEventsListener() {
	OnNewConnectionHandler = (EzSocket socket) => {
		Console.WriteLine($"I'm connected to the math server");
	},
	OnConnectionClosedHandler = (EzSocket socket) => {
		Console.WriteLine($"My connection to the math server has been closed");
	},
	OnMessageReadHandler = (EzSocket sock, byte[] buff) => {
		Console.WriteLine($"Math server's response is: {Encoding.Default.GetString(buff)}");
	},
});

var loop = true;

socket.StartReadingMessages();

do {
	Console.WriteLine($"0) Addition - 1) Substraction - 2) Multiplication - 3) Division - 4) Exit");
	Console.WriteLine($"Enter an option below:");
	var input = Console.ReadLine();
	var inputParse = int.TryParse(input, out var inputAsInt);
	if (!inputParse) {
		Console.WriteLine($"(!) {input} is not an integer (!)");
		continue;
	}
	if (inputAsInt is >= 0 and <= 3) {
		Console.Write($"a: ");
		var a = Console.ReadLine();
		var aParse = int.TryParse(a, out var aAsInt);
		if (!aParse) {
			Console.WriteLine($"(!) This is not an integer (!)");
			continue;
		}
		Console.Write($"b: ");
		var b = Console.ReadLine();
		var bParse = int.TryParse(b, out var bAsInt);
		if (!bParse) {
			Console.WriteLine($"(!) This is not an integer (!)");
			continue;
		}
		socket.SendMessage($"{inputAsInt} {a} {b}");
	} else if (inputAsInt is 4) {
		loop = false;
	} else {
		Console.WriteLine($"(!) {input} is not a valid option (!)");
	}
} while (loop);

socket.StopReadingMessages();

socket.Close();
using MyMonkeyApp.Services;
using MyMonkeyApp.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var service = new MonkeyService();
var monkeys = service.GetAll();

if (monkeys.Count == 0)
{
	Console.WriteLine("No monkeys available.");
	return;
}

PrintBanner();
PrintMonkeyTable(monkeys);

static void PrintBanner()
{
	const string banner = @"  __  __            _              
 |  \/  | ___  _ __| | _____ _   _ 
 | |\/| |/ _ \| '__| |/ / _ \ | | |
 | |  | | (_) | |  |   <  __/ |_| |
 |_|  |_|\___/|_|  |_|\_\___|\__, |
							 |___/ 
   Monkey Directory
";
	Console.ForegroundColor = ConsoleColor.Yellow;
	Console.WriteLine(banner);
	Console.ResetColor();
}

static void PrintMonkeyTable(IReadOnlyList<Monkey> monkeys)
{
	// Determine column widths
	int nameW = Math.Max("Name".Length, monkeys.Max(m => m.Name.Length));
	int sciW = Math.Max("Scientific".Length, monkeys.Max(m => m.ScientificName.Length));
	int regionW = Math.Max("Region".Length, monkeys.Max(m => m.Region.Length));
	int descW = 70; // limit description width

	string Line(string sepLeft, string sepMid, string sepRight, char fill) =>
		sepLeft + new string(fill, nameW + 2) + sepMid + new string(fill, sciW + 2) + sepMid + new string(fill, regionW + 2) + sepMid + new string(fill, descW + 2) + sepRight;

	string Trunc(string text, int w) => text.Length <= w ? text : text[..(w - 1)] + "…";

	Console.ForegroundColor = ConsoleColor.DarkGray;
	Console.WriteLine(Line("╔", "╦", "╗", '═'));
	Console.ResetColor();

	Console.ForegroundColor = ConsoleColor.Cyan;
	Console.WriteLine($"║ {"Name".PadRight(nameW)} ║ {"Scientific".PadRight(sciW)} ║ {"Region".PadRight(regionW)} ║ {"Description".PadRight(descW)} ║");
	Console.ResetColor();

	Console.ForegroundColor = ConsoleColor.DarkGray;
	Console.WriteLine(Line("╠", "╬", "╣", '═'));
	Console.ResetColor();

	foreach (var m in monkeys)
	{
		Console.WriteLine($"║ {m.Name.PadRight(nameW)} ║ {m.ScientificName.PadRight(sciW)} ║ {m.Region.PadRight(regionW)} ║ {Trunc(m.Description, descW).PadRight(descW)} ║");
	}

	Console.ForegroundColor = ConsoleColor.DarkGray;
	Console.WriteLine(Line("╚", "╩", "╝", '═'));
	Console.ResetColor();
}

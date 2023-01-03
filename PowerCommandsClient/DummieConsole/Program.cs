// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var input = "";
while (!input.StartsWith("x"))
{
    input = Console.ReadLine();
    Console.WriteLine(input);
}
Console.WriteLine("Proxy program ended, you can now stop your application gracefully!");
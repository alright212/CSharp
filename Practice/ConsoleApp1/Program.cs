// See https://aka.ms/new-console-template for more information

using CalculatorBrain;

Console.WriteLine("Hello, World!");
var calculator = new SimpleCalculator();
while (true)  // Start an infinite loop
{
    Console.WriteLine("Give me a number or type 'exit' to quit: ");
    var nrStr = Console.ReadLine();

    if (nrStr.ToLower() == "exit") 
    {
        break;
    }

    var nr = decimal.Parse(nrStr);
    calculator.SetState(nr);

    Console.WriteLine("Give me an operation: ");
    var operation = Console.ReadLine();

    Console.WriteLine("Give me a number: ");
    nrStr = Console.ReadLine();
    nr = decimal.Parse(nrStr);

    if (operation == "+") calculator.Add(nr);
    else if (operation == "-") calculator.Minus(nr);
    else if (operation == "*") calculator.Multiply(nr);
    else if (operation == "/") calculator.Divide(nr);

    Console.WriteLine("Result is: " + calculator.CurrentState);
}
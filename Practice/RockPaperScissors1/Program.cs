using System;

namespace RockPaperScissors1
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to Rock, Paper, Scissors, Lizard, Spock!");
            DisplayRules();

            string playResponse;
            do
            {
                Console.WriteLine("Do you want to play? (Yes/No)");
                playResponse = Console.ReadLine()?.ToUpper() ?? "";

                if (playResponse != "YES" && playResponse != "Y" && playResponse != "NO" && playResponse != "N")
                {
                    Console.WriteLine("Invalid response. Please answer with Yes or No.");
                }
            } while (playResponse != "YES" && playResponse != "Y" && playResponse != "NO" && playResponse != "N");

            if (playResponse == "YES" || playResponse == "Y")
            {
                PlayGame();
            }
            else
            {
                Console.WriteLine("Thanks for checking out the game! Live long and prosper.");
            }
        }
        static void DisplayRules()
        {
            Console.WriteLine("\nRules:");
            Console.WriteLine("- Rock crushes Scissors");
            Console.WriteLine("- Scissors cuts Paper");
            Console.WriteLine("- Paper covers Rock");
            Console.WriteLine("- Rock crushes Lizard");
            Console.WriteLine("- Lizard poisons Spock");
            Console.WriteLine("- Spock smashes Scissors");
            Console.WriteLine("- Scissors decapitates Lizard");
            Console.WriteLine("- Lizard eats Paper");
            Console.WriteLine("- Paper disproves Spock");
            Console.WriteLine("- Spock vaporizes Rock\n");
        }

        static void PlayGame()
        {
            string userInput;
            do
            {
                Console.WriteLine("Choose: Rock (R), Paper (P), Scissors (S), Lizard (L), or Spock (K)");
                userInput = Console.ReadLine()!.ToUpper();

                if (!"RPSLK".Contains(userInput))
                {
                    Console.WriteLine("Invalid input. Please enter a valid choice.");
                }
            } while (!"RPSLK".Contains(userInput));

            Random rnd = new Random();
            int computerChoice = rnd.Next(1, 6);

            string computerInput = "";
            switch (computerChoice)
            {
                case 1:
                    computerInput = "R";
                    break;
                case 2:
                    computerInput = "P";
                    break;
                case 3:
                    computerInput = "S";
                    break;
                case 4:
                    computerInput = "L";
                    break;
                case 5:
                    computerInput = "K";
                    break;
            }

            Console.WriteLine($"Computer chose: {computerInput}");

            if (userInput == computerInput)
            {
                Console.WriteLine("It's a draw!");
            }
            else if ((userInput == "R" && (computerInput == "S" || computerInput == "L")) ||
                     (userInput == "P" && (computerInput == "R" || computerInput == "K")) ||
                     (userInput == "S" && (computerInput == "P" || computerInput == "L")) ||
                     (userInput == "L" && (computerInput == "K" || computerInput == "P")) ||
                     (userInput == "K" && (computerInput == "S" || computerInput == "R")))
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("Computer wins!");
            }
        }
    }
}

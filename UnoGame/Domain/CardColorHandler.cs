namespace Domain;

public class CardColorHandler
{
    public static bool DisplayColors { get; set; } = true; // Default to true

    public static string CardColorToString(EUnoCard.UnoCardsColors color)
    {
        if (DisplayColors)
        {
            switch (color)
            {
                case EUnoCard.UnoCardsColors.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case EUnoCard.UnoCardsColors.Blue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case EUnoCard.UnoCardsColors.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case EUnoCard.UnoCardsColors.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
        }
        return color.ToString();

    }

}
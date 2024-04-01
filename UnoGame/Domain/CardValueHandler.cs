namespace Domain;

public class CardValueHandler
{
    public static string CardValueToString(EUnoCard.UnoCardsValues value)
    {
        return value switch
        {
            EUnoCard.UnoCardsValues.Value0 => "0",
            EUnoCard.UnoCardsValues.Value1 => "1",
            EUnoCard.UnoCardsValues.Value2 => "2",
            EUnoCard.UnoCardsValues.Value3 => "3",
            EUnoCard.UnoCardsValues.Value4 => "4",
            EUnoCard.UnoCardsValues.Value5 => "5",
            EUnoCard.UnoCardsValues.Value6 => "6",
            EUnoCard.UnoCardsValues.Value7 => "7",
            EUnoCard.UnoCardsValues.Value8 => "8",
            EUnoCard.UnoCardsValues.Value9 => "9",
            EUnoCard.UnoCardsValues.ValueBlock => "Skip",
            EUnoCard.UnoCardsValues.ValuePlus2 => "+2",
            EUnoCard.UnoCardsValues.ValueReverse => "Reverse",
            EUnoCard.UnoCardsValues.ValuePlus4 => "+4",
            EUnoCard.UnoCardsValues.ValuePickColor => "Color Card",
            _ => throw new ApplicationException("No such card value " + value)
        };
    }
}
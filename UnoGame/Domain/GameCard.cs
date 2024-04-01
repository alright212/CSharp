namespace Domain;

public class GameCard
{
    public EUnoCard.UnoCardsValues Value { get; init; }
    public EUnoCard.UnoCardsColors Color { get; set; }

    public override string ToString()
    {
        var color = CardColorHandler.CardColorToString(Color);
        var value = CardValueHandler.CardValueToString(Value);

        return Value is EUnoCard.UnoCardsValues.ValuePlus4 or EUnoCard.UnoCardsValues.ValuePickColor
            ? value
            : $"{color} {value}";
    }
}
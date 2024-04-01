using DAL;
using Domain;

namespace GameEngine;

public class GameEngine
{
    private const int HandSizeAtTheStart = 7;
    private const int SpecialCardCount = 4;
    private const int CardValuePlus2 = (int)EUnoCard.UnoCardsValues.ValuePlus2;
    private const int CardColorBlue = (int)EUnoCard.UnoCardsColors.Blue;
    private const int CardValuePickColor = (int)EUnoCard.UnoCardsValues.ValuePickColor;
    private const int CardValuePlus4 = (int)EUnoCard.UnoCardsValues.ValuePlus4;

    private readonly Random _random = new();

    public GameState GameState { get; private set; } = new();
    private IGameRepository? GameRepo { get; }

    public GameEngine(IGameRepository? gameRepo)
    {
        InitializeDefaultPlayers();
        ShuffleAndDistributeCards();
        GameRepo = gameRepo;
        GameState.Id = Guid.NewGuid();
    }

    public Player GetActivePlayer()
    {
        return GameState.Players[GameState.CurrentPlayerIndex];
    }

    private void ShuffleAndDistributeCards()
    {
        InitializeFullShuffledDeck();
        GiveCardsToPlayers();
    }

    private void InitializeDefaultPlayers()
    {
        GameState.Players = new List<Player>
        {
            new() { Nickname = "PERSON", Type = EPlayerType.Human },
            new() { Nickname = "BOT", Type = EPlayerType.Ai }
        };
    }

    private List<GameCard> ShuffleTheDeck(List<GameCard> deck)
    {
        var shuffledDeck = new List<GameCard>();

        while (deck.Any())
        {
            var randomPosition = _random.Next(deck.Count);
            shuffledDeck.Add(deck[randomPosition]);
            deck.RemoveAt(randomPosition);
        }

        return shuffledDeck;
    }

    private void InitializeFullShuffledDeck()
    {
        GameState.DrawDeck.Clear();
        BluePlusTwo();
        OneUpToPlusTwo();
        FourOfEachSpecialCard();

        GameState.DrawDeck = ShuffleTheDeck(GameState.DrawDeck);

        SetColorOfFirstCardToBePlayed();
    }
    private void AddCardsToDeck(int startValue, int endValue)
    {
        for (int cardValue = startValue; cardValue <= endValue; cardValue++)
        {
            for (int cardColor = 0; cardColor <= CardColorBlue; cardColor++)
            {
                GameState.DrawDeck.Add(new GameCard
                {
                    Value = (EUnoCard.UnoCardsValues)cardValue,
                    Color = (EUnoCard.UnoCardsColors)cardColor
                });
            }
        }
    }
    private void SetColorOfFirstCardToBePlayed()
    {
        var firstCard = GameState.DrawDeck.Last();
        if (firstCard.Value is EUnoCard.UnoCardsValues.ValuePlus4 or EUnoCard.UnoCardsValues.ValuePickColor)
        {
            firstCard.Color = (EUnoCard.UnoCardsColors)_random.Next((int)EUnoCard.UnoCardsColors.Blue);
        }

        GameState.PlayingDeck.Add(firstCard);
        GameState.DrawDeck.RemoveAt(GameState.DrawDeck.Count - 1);
    }

    private void BluePlusTwo()
    {
        AddCardsToDeck(0, CardValuePlus2);
    }

    private void FourOfEachSpecialCard()
    {
        for (var amount = 0; amount < SpecialCardCount; amount++)
        {
            for (var cardValue = CardValuePickColor; cardValue <= CardValuePlus4; cardValue++)
            {
                GameState.DrawDeck.Add(new GameCard
                {
                    Value = (EUnoCard.UnoCardsValues)cardValue
                });
            }
        }
    }

    private void OneUpToPlusTwo()
    {
        AddCardsToDeck(1, CardValuePlus2);
    }

    private void GiveCardsToPlayers()
    {
        for (var i = 0; i < GameState.Players.Count; i++)
        {
            var player = GameState.Players[i];
            for (var deckIndex = 0; deckIndex < HandSizeAtTheStart; deckIndex++)
            {
                player.CardsHand.Add(GameState.DrawDeck.Last());
                GameState.DrawDeck.RemoveAt(GameState.DrawDeck.Count - 1);
            }
        }
    }


    public void SetCustomPlayerTypes(int humanCount, int aiCount)
    {
        // Clear existing players
        GameState.Players.Clear();

        // Add human players
        for (int i = 0; i < humanCount; i++)
        {
            GameState.Players.Add(new Player
            {
                Nickname = "PERSON " + (i + 1),
                Type = EPlayerType.Human
            });
        }

        // Add AI players
        for (int i = 0; i < aiCount; i++)
        {
            GameState.Players.Add(new Player
            {
                Nickname = "BOT " + (i + 1),
                Type = EPlayerType.Ai
            });
        }

        // Reinitialize deck and distribute cards
        InitializeFullShuffledDeck();
        GiveCardsToPlayers();
    }

    public void SetPlayerCount(int count)
    {
        GameState.Players.Clear();
        for (var i = 0; i < count; i++)
        {
            GameState.Players.Add(new Player()
            {
                Nickname = "PERSON " + (i + 1),
                Type = EPlayerType.Human
            });
        }

        InitializeFullShuffledDeck();
        GiveCardsToPlayers();
    }

    public string GetPlayerTypes()
    {
        return string.Join(", ", GameState.Players.Select(player => player.Type).Distinct());
    }

    public string GetPlayerCount()
    {
        return GameState.Players.Count.ToString();
    }

    public string GetPlayerNicknames()
    {
        return string.Join(", ", GameState.Players.Select(player => player.Nickname));
    }

    public void SaveGame(Guid? saveName)
    {
        GameRepo?.SaveGame(saveName, GameState);
    }

    public void LoadGame(Guid saveName)
    {
        GameState = GameRepo?.LoadGameState(saveName) ??
                    throw new InvalidOperationException("Loading save: " + saveName + " returned null");
    }

    private void RefillPlayDeckWhenEmpty()
    {
        if (GameState.DrawDeck.Any()) return;
        var lastCard = GameState.PlayingDeck.Last();
        GameState.PlayingDeck.RemoveAt(GameState.PlayingDeck.Count - 1);
        GameState.DrawDeck = ShuffleTheDeck(GameState.PlayingDeck);
        GameState.PlayingDeck.Clear();
        GameState.PlayingDeck.Add(lastCard);
    }

    private bool CardCanBePlayed(GameCard card)
    {
        if (card.Color == GameState.PlayingDeck.Last().Color)
        {
            return true;
        }

        if ((card.Value == EUnoCard.UnoCardsValues.ValuePlus4 &&
             !GameState.Players[GameState.CurrentPlayerIndex].CardsHand.Select(unoCard => unoCard.Color).Distinct()
                 .Contains(GameState.PlayingDeck.Last().Color)) ||
            card.Value == EUnoCard.UnoCardsValues.ValuePickColor)
        {
            return true;
        }

        return card.Value == GameState.PlayingDeck.Last().Value;
    }

    private void DrawCard(int playerIndex)
    {
        RefillPlayDeckWhenEmpty();
        var card = GameState.DrawDeck.Last();
        GameState.Players[playerIndex].CardsHand.Add(card);
        GameState.Players[playerIndex].Drew = true;
        GameState.DrawDeck.RemoveAt(GameState.DrawDeck.Count - 1);
        Console.WriteLine($"{GameState.Players[playerIndex].Nickname} drew a card");
    }

    private bool ApplyCardRules(GameCard card)
    {
        if (card.Value == EUnoCard.UnoCardsValues.ValueBlock)
        {
            IncreasePlayerIndex();
            return false;
        }

        if (card.Value == EUnoCard.UnoCardsValues.ValuePlus2)
        {
            for (int i = 0; i < 2; i++)
            {
                DrawCard(GetNextPlayerIndex());
            }

            IncreasePlayerIndex();
            return false;
        }

        if (card.Value == EUnoCard.UnoCardsValues.ValueReverse)
        {
            GameState.WhatWay = -GameState.WhatWay;
            return false;
        }

        if (card.Value == EUnoCard.UnoCardsValues.ValuePlus4)
        {
            for (int i = 0; i < 4; i++)
            {
                DrawCard(GetNextPlayerIndex());
            }

            IncreasePlayerIndex();
            return true;
        }

        if (card.Value == EUnoCard.UnoCardsValues.ValuePickColor)
        {
            return true;
        }

        return false;
    }

    private string? PlayCard(int playerIndex, int cardIndex)
    {
        var card = GameState.Players[playerIndex].CardsHand[cardIndex];
        if (CardCanBePlayed(card))
        {
            Console.WriteLine(GameState.Players[playerIndex].Nickname + " PLAYED: " + card);
            Console.ForegroundColor = ConsoleColor.White;
            var askColor = ApplyCardRules(card);
            GameState.PlayingDeck.Add(card);
            GameState.Players[playerIndex].CardsHand.RemoveAt(cardIndex);
            IncreasePlayerIndex();
            if (GameState.Players[playerIndex].CardsHand.Count == 0)
            {
                Console.WriteLine(GameState.Players[playerIndex].Nickname + " Won");
                return "won";
            }

            if (askColor)
            {
                return "askColor";
            }
        }
        else
        {
            Console.WriteLine(card + " Can't be played onto " + GameState.PlayingDeck.Last());
        }

        return null;
    }

    public bool DrawUntilPlayableCard()
    {
        var drew = false;
        var playableCards = GameState.Players[GameState.CurrentPlayerIndex].CardsHand.FindAll(CardCanBePlayed);
        while (playableCards.Count == 0)
        {
            DrawCard(GameState.CurrentPlayerIndex);
            playableCards = GameState.Players[GameState.CurrentPlayerIndex].CardsHand.FindAll(CardCanBePlayed);
            drew = true;
        }

        return drew;
    }

    public string? AiPlayRandomCard()
    {
        DrawUntilPlayableCard();
        var playableCards = GameState.Players[GameState.CurrentPlayerIndex].CardsHand.FindAll(CardCanBePlayed);
        var randomCard = playableCards[_random.Next(playableCards.Count)];
        var result = PlayCard(GameState.CurrentPlayerIndex,
            GameState.Players[GameState.CurrentPlayerIndex].CardsHand.IndexOf(randomCard));
        if (result != "askColor") return result == "won" ? null : result;
        GameState.PlayingDeck.Last().Color = (EUnoCard.UnoCardsColors)_random.Next((int)EUnoCard.UnoCardsColors.Blue);
        return result;
    }

    public string? HumanPlayCard(int playerIndex, int cardIndex)
    {
        return PlayCard(playerIndex, cardIndex);
    }

    private int GetNextPlayerIndex()
    {
        var tempIndex = GameState.CurrentPlayerIndex;
        tempIndex += GameState.WhatWay;
        if (tempIndex > GameState.Players.Count - 1)
        {
            return tempIndex - GameState.Players.Count;
        }

        if (tempIndex < 0)
        {
            return tempIndex + GameState.Players.Count;
        }

        return tempIndex;
    }

    public int GetLastPlayerIndex(int moveTimes)
    {
        var temporaryIndex = GameState.CurrentPlayerIndex;
        for (var i = 0; i < moveTimes; i++)
        {
            temporaryIndex -= GameState.WhatWay;
            if (temporaryIndex > GameState.Players.Count - 1)
            {
                return temporaryIndex - GameState.Players.Count;
            }

            if (temporaryIndex < 0)
            {
                return temporaryIndex + GameState.Players.Count;
            }
        }

        return temporaryIndex;
    }

    public void IncreasePlayerIndex()
    {
        GetActivePlayer().Drew = false;
        GameState.CurrentPlayerIndex = GetNextPlayerIndex();
    }

    public void SetTopCardColor(string? color)
    {
        EUnoCard.UnoCardsColors eColor;
        if (color == "y")
        {
            eColor = EUnoCard.UnoCardsColors.Yellow;
        }
        else if (color == "r")
        {
            eColor = EUnoCard.UnoCardsColors.Red;
        }
        else if (color == "l")
        {
            eColor = EUnoCard.UnoCardsColors.Blue;
        }
        else if (color == "g")
        {
            eColor = EUnoCard.UnoCardsColors.Green;
        }
        else
        {
            Console.WriteLine("Undefined option");
            return;
        }

        GameState.PlayingDeck.Last().Color = eColor;
    }
}
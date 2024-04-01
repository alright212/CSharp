namespace Domain;

public class GameState
{
    public Guid Id { get; set; }
    public List<GameCard> DrawDeck { get; set; } = new();
    public List<GameCard> PlayingDeck { get; set; } = new();
    public List<Player> Players { get; set; } = new();
    public int CurrentPlayerIndex { get; set; }

    public int WhatWay = 1;
}
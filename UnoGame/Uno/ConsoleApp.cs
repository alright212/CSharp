using DAL;
using Domain;
using MenuSystem;

Menu PlayerTypesMenu(List<string> list, GameEngine.GameEngine? gameEngine)
{
    var menu = new Menu("Set player types", new List<MenuItem>
    {
        new()
        {
            PromptText = "c", MenuLabel = () => "Custom player types", MethodToRun = SetCustomPlayerTypes
        }
    }, list);
    return menu;

    string? SetCustomPlayerTypes()
    {
        Console.Clear();
        Console.Write("Enter number of human players: ");
        var humanCountStr = Console.ReadLine()?.Trim();
        Console.Write("Enter number of AI players: ");
        var aiCountStr = Console.ReadLine()?.Trim();

        try
        {
            var humanCount = int.Parse(humanCountStr ?? "0");
            var aiCount = int.Parse(aiCountStr ?? "0");

            // Validation for player count
            if (humanCount + aiCount < 2)
            {
                Console.WriteLine("There must be at least two players (either human or AI). Please try again.");
                return "b";
            }

            if (humanCount < 0 || aiCount < 0)
            {
                Console.WriteLine("Number of players cannot be negative. Please try again.");
                return "b";
            }

            // Additional constraints can be added here (like maximum number of players)

            gameEngine!.SetCustomPlayerTypes(humanCount, aiCount);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter integer values.");
            return "b";
        }

        return "b";
    }
}

IGameRepository gameRepository = new GameRepositoryEF();
//var gameRepository = new GameRepositoryFileSystem();
var game = new GameEngine.GameEngine(gameRepository);

var menuLevels = new List<string> { "mainMenu" };

var colorOptions = new Dictionary<string, string>
{
    { "l", "Blue" },
    { "r", "Red" },
    { "g", "Green" },
    { "y", "Yellow" }
};

var colorMenuItems = colorOptions.Select(option => new MenuItem
{
    PromptText = option.Key, MenuLabel = () => option.Value, MethodToRun = () => option.Value
}).ToList();

var colorMenu = new Menu("Pick a color for the top card: ", colorMenuItems, menuLevels);

colorMenu.RemoveBackAndExitKeys();


string? SetPlayerCount()
{
    Console.Clear();
    Console.Write("Player count: ");
    var countStr = Console.ReadLine()?.Trim();
    try
    {
        if (countStr != null)
        {
            var count = int.Parse(countStr);
            game.SetPlayerCount(count);
        }
    }
    catch (FormatException)
    {
        Console.WriteLine($"Input ({countStr}) was not an integer number");
        return "b";
    }

    return "b";
}

string? SetNickNames()
{
    Console.Clear();
    var menuItems = new List<MenuItem>();
    int i = 1;
    foreach (var player in game.GameState.Players)
    {
        menuItems.Add(new MenuItem()
        {
            PromptText = i.ToString(), MenuLabel = () => $"Change nickname of {player.Nickname} ({player.Type})",

            MethodToRun = () =>
            {
                Console.Write($"Set nickname of {player.Nickname} ({player.Type}) to: ");
                var newNickname = Console.ReadLine()?.Trim();
                player.Nickname = newNickname;
                return null;
            }
        });
        i++;
    }

    var nicknamesMenu = new Menu("Nicknames: ", menuItems, menuLevels);
    return nicknamesMenu.RunOnce();
}

var playerTypesMenu = PlayerTypesMenu(menuLevels, game);


var startNewGameMenu = new Menu("New game", new List<MenuItem>
{
    new()
    {
        PromptText = "c", MenuLabel = () => "Player count: " + game.GetPlayerCount(), MethodToRun = SetPlayerCount
    },
    new()
    {
        PromptText = "t", MenuLabel = () => "Player types: " + game.GetPlayerTypes(),
        MethodToRun = playerTypesMenu.RunOnce
    },
    new()
    {
        PromptText = "n", MenuLabel = () => "Nicknames: " + game.GetPlayerNicknames(), MethodToRun = SetNickNames
    },
    new()
    {
        PromptText = "s", MenuLabel = () => "Start the game", MethodToRun = GameLoop
    }
}, menuLevels);

var mainMenu = new Menu("Main menu", new List<MenuItem>
{
    new()
    {
        PromptText = "n", MenuLabel = () => "Start a new game",

        MethodToRun = () =>
        {
            game = new GameEngine.GameEngine(gameRepository);
            return startNewGameMenu.Run();
        }
    },
    new()
    {
        PromptText = "l", MenuLabel = () => "Load a game",

        MethodToRun = () =>
        {
            var menuItems = new List<MenuItem>();
            int i = 1;
            List<(Guid ID, DateTime LastEditedAt)> saves;
            try
            {
                saves = gameRepository.GetAllSaves();
            }
            catch (Exception)
            {
                Console.WriteLine("No saves");
                return "";
            }

            foreach (var save in saves)
            {
                menuItems.Add(new MenuItem()
                {
                    PromptText = i.ToString(), MenuLabel = () => save.ID + ", " + save.LastEditedAt,

                    MethodToRun = () =>
                    {
                        game.LoadGame(save.ID);
                        return GameLoop();
                    }
                });
                i++;
            }

            var loadGameMenu = new Menu("Load menu", menuItems, menuLevels);
            return loadGameMenu.RunOnce();
        }
    },
    new MenuItem
    {
        PromptText = "ö", 
        MenuLabel = () => "Toggle Card Colors",
        MethodToRun = () =>
        {
            CardColorHandler.DisplayColors = !CardColorHandler.DisplayColors;
            return null; 
        }
    }
    
}, menuLevels, menuLevels[0]);
mainMenu.Run();
return;

string? GameLoop()
{
    var userChoice = "";
    do
    {
        // Also checks whether or not the player has to draw
        var drew = game.DrawUntilPlayableCard();
        var player = game.GameState.Players[game.GameState.CurrentPlayerIndex];
        if (player.Type == EPlayerType.Human)
        {
            Console.WriteLine("Top card: " + game.GameState.PlayingDeck.Last());
            Console.ForegroundColor = ConsoleColor.White;
            var cardsMenuItems = new List<MenuItem>();
            for (var cardIndex = 0; cardIndex < player.CardsHand.Count; cardIndex++)
            {
                var thisPlayerIndex = game.GameState.CurrentPlayerIndex;
                var thisCardIndex = cardIndex;
                cardsMenuItems.Add(new MenuItem()
                {
                    PromptText = (cardIndex + 1).ToString(), MenuLabel = player.CardsHand[cardIndex].ToString,

                    MethodToRun = () => game.HumanPlayCard(thisPlayerIndex, thisCardIndex)
                });
            }

            if (drew)
            {
                cardsMenuItems.Add(new MenuItem()
                {
                    PromptText = "k", MenuLabel = () => "Skip turn",

                    MethodToRun = () =>
                    {
                        Console.WriteLine(game.GameState.Players[game.GameState.CurrentPlayerIndex].Nickname + " skipped their turn");
                        game.IncreasePlayerIndex();
                        return null;
                    }
                });
            }

            var gameLoopMenu = new Menu(player.Nickname + ", play a card from your deck", cardsMenuItems, menuLevels);
            gameLoopMenu.RemoveBackKey();
            gameLoopMenu.AddMenuItem(new MenuItem()
            {
                PromptText = "b", MenuLabel = () => "Back to main menu", MethodToRun = () => menuLevels[0]
            });
            gameLoopMenu.AddMenuItem(new MenuItem()
            {
                PromptText = "s", MenuLabel = () => "Save game and exit to menu",
                
                MethodToRun = () =>
                {
                    gameRepository.SaveGame(game.GameState.Id, game.GameState);
                    return menuLevels[0];
                }
            });

            userChoice = gameLoopMenu.RunOnce();

            if (userChoice == "askColor")
            {
                Console.Clear();
                string? color;
                do
                {
                    color = colorMenu.RunOnce();
                } while (!new List<string> { "l", "r", "g", "y" }.Contains(color!));

                game.SetTopCardColor(color);
                Console.Clear();
                Console.WriteLine(game.GameState.Players[game.GetLastPlayerIndex(1)].Nickname + " PLAYED: " +
                                  game.GameState.PlayingDeck.Last());
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (userChoice is null or "won")
            {
                return menuLevels[0];
            }
        }
        else if (player.Type == EPlayerType.Ai)
        {
            userChoice = game.AiPlayRandomCard();
            if (userChoice == "askColor")
            {
                Console.WriteLine(game.GameState.Players[game.GetLastPlayerIndex(2)].Nickname + " PLAYED: " +
                                  game.GameState.PlayingDeck.Last());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    } while (userChoice != "x" && !menuLevels.Contains(userChoice!));

    Console.Clear();
    return userChoice;
}
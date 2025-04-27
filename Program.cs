using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal class Program
    {
        /// <summary>
        /// Main method that executes when the program starts.
        /// </summary>
        static void Main(string[] args)
        {
            // Testing
            Test test = new Test();
            test.TestAll();
            Console.Write("\n\n\nPress Enter to continue.");
            Console.ReadKey();

            // Game title
            Console.WriteLine("========Welcome to DUNGEON EXPLORER!========\n");
            Console.WriteLine("Options:\n1) New Game\n2) Load Game\n3) Exit");
            while (true)
            {
                Console.Write(">");
                string userInput = Console.ReadLine();
                // New game
                if (userInput == "1")
                {
                    // Initialising player
                    Console.WriteLine("\nYou wake up alone in a dark dungeon. You don't remember who you are or how you got here.");
                    string playerName = "";
                    while (playerName == "")
                    {
                        Console.Write("What will you call yourself?\n>");
                        playerName = Console.ReadLine().Trim();
                    }
                    Player player = new Player(playerName, 30, 1, 1);
                    // Initialising starting room
                    Room startingRoom = new Room(null, new List<Potion>() { new Potion("Potion", 0, 10, 0) }, new Weapon("Sword", 10, 1), false);
                    // Initialising and starting game
                    Game game = new Game(player, startingRoom);
                    game.Start();
                    break;
                }
                // Loads game from savegame.dat
                else if (userInput == "2")
                {
                    // Path generated using BaseDirectory to ensure consistency across different machines.
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.dat");
                    Game game = Save.LoadGame(path);
                    Console.WriteLine("Loading saved game...");
                    // If the game was not found, the user is returned to the menu.
                    if (game == null)
                    {
                        Console.WriteLine("No saved game found.");
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadKey();
                        continue;
                    }
                    Console.WriteLine("Game successfully loaded.");
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadKey();
                    game.Start();
                    return;
                }
                // Exit game
                else if (userInput == "3")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Please enter a valid input.");
                }
            }
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
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
            Console.WriteLine("========Welcome to DUNGEON EXPLORER!========\n"); // Game title
            // Initializing player
            Console.WriteLine("You wake up alone in a dark dungeon. You don't remember who you are or how you got here.");
            string playerName = "";
            while (playerName == "")
            {
                Console.Write("What will you call yourself?\n>");
                playerName = Console.ReadLine().Trim();
            }
            Player player = new Player(playerName, 30, 0, 1);
            // Initializing starting room
            Room startingRoom = new Room(null, new List<Potion>(){ new Potion("Health(10) Potion", 0, 10, 0) }, new Weapon("Sword", 10));
            // Initializing and starting game
            Game game = new Game(player, startingRoom);
            /*
            Test test = new Test();
            test.TestHealth();
            test.TestInventory();
            test.TestRoom();
            test.TestItems();
            */
            game.Start();
            Console.ReadKey();
        }
    }
}

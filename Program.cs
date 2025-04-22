using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    // REFLECTIVE ANALYISSIS:
    // LINQ Could be used in the Game class to filter rooms based on their properties.
    // LINQ Could be used in the Player class to automatically equip the best weapon from the inventory.
    // Examples of interfaces could be IDescribable for Item. For Creature, an interface could be ICanAttack to define the attack method.
    // A statistics class could be created to handle Creature stats.
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
            Room startingRoom = new Room(null, new List<Potion>(){ new Potion("Health(10) Potion", 0, 10, 0) }, new Weapon("Sword", 10, 1));
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

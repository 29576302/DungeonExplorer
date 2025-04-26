using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// This class is used to test various instantiations and interactions in the game.
    /// </summary> 
    class Test
    {
        private TextWriter consoleOut;
        public Test()
        {
            // Used to store the original Console output.
            consoleOut = Console.Out;
        }
        /// <summary>
        /// Method used to run all tests.
        /// </summary>
        public void TestAll()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testresults.txt");
            using (StreamWriter writer = new StreamWriter(path))
            {
                // Copies Debug.Assert output to .txt file.
                TextWriterTraceListener listener = new TextWriterTraceListener(writer);
                Debug.Listeners.Add(listener);

                Console.WriteLine("Testing:");
                TestStats();
                TestPlayer();
                TestInventory();
                TestRoom();
                TestMap();

                // Removes listener
                Debug.Flush();
                Debug.Listeners.Remove(listener);
                Console.WriteLine($"Tests completed. Asserts can be found at {path}");
            }
        }
        /// <summary>
        /// Method used to test the CreatureStats class.
        /// </summary>
        public void TestStats()
        {
            CreatureStats stats = new CreatureStats(30, 5, 1, 1, true);
            Console.WriteLine("CreatureStats initialisation.");
            Debug.Assert(stats.MaxHealth == 30, "MaxHealth initialisation failed.");
            Debug.Assert(stats.CurrentHealth == 30, "CurrentHealth initialisation failed.");
            Debug.Assert(stats.Attack == 5, "Attack initialisation failed.");
            Debug.Assert(stats.Speed == 1, "Speed initialisation failed.");
            Debug.Assert(stats.Level == 1, "Level initialisation failed.");
            Debug.Assert(stats.XP == 0, "XP initialisation failed.");

            Console.WriteLine("CreatureStats modifying values.");
            // MaxHealth
            stats.ModifyMaxHealth(5);
            Debug.Assert(stats.MaxHealth == 35, "MaxHealth modified incorrectly.");
            stats.ModifyMaxHealth(-1000);
            Debug.Assert(stats.MaxHealth >= 0, "MaxHealth is less than 0.");
            // CurrentHealth
            stats.ModifyMaxHealth(35);
            stats.ModifyCurrentHealth(5);
            Debug.Assert(stats.CurrentHealth == 35, "CurrentHealth modified incorrectly.");
            stats.ModifyCurrentHealth(-1000);
            Debug.Assert(stats.CurrentHealth >= 0, "CurrentHealth is less than 0.");
            // Attack
            stats.ModifyAttack(5);
            Debug.Assert(stats.Attack == 10, "Attack modified incorrectly.");
            stats.ModifyAttack(-1000);
            Debug.Assert(stats.MaxHealth >= 0, "Attack is less than 0.");
            // Speed
            stats.ModifySpeed(1);
            Debug.Assert(stats.Speed == 2, "Speed modified incorrectly.");
            stats.ModifySpeed(-1000);
            Debug.Assert(stats.Speed >= 0, "Speed is less than 0.");
            // Level
            stats.ModifyLevel(5);
            Debug.Assert(stats.Level == 6, "Level modified incorrectly.");
            stats.ModifyLevel(-1000);
            Debug.Assert(stats.Level >= 1, "Level is less than 1.");
            // XP
            Console.SetOut(TextWriter.Null); // Suppress Console output
            stats.ModifyXP(5);
            Console.SetOut(consoleOut); // Restore Console output
            Debug.Assert(stats.Level == 3 && stats.XP == 2, "XP modified incorrectly.");
            stats.ModifyXP(-1000);
            Debug.Assert(stats.XP >= 0, "XP is less than 0.");
        }
        /// <summary>
        /// Method used to test the Player class.
        /// </summary>
        public void TestPlayer()
        {
            Player player = new Player("Player", 30, 5, 1);
            Console.WriteLine("Player initialisation.");
            Debug.Assert(player.Name == "Player", "Name initialisation failed.");
            Debug.Assert(player.MaxHealth == 30, "MaxHealth initialisation failed.");
            Debug.Assert(player.CurrentHealth == 30, "CurrentHealth initialisation failed.");
            Debug.Assert(player.Attack == 5, "Attack initialisation failed.");
            Debug.Assert(player.Speed == 0, "Speed initialisation failed.");
            Debug.Assert(player.Level == 1, "Level initialisation failed.");
            Debug.Assert(player.IsAlive == true, "IsAlive initialisation failed.");
            Debug.Assert(player.EquippedWeapon == null, "EquippedWeapon initialisation failed.");

            Console.WriteLine("Player methods.");
            // TakeDamage
            player.TakeDamage(5);
            Debug.Assert(player.CurrentHealth == 25, "TakeDamage method failed.");
            // UsePotion
            player.UsePotion(new Potion("Potion", 10, 10, 10));
            Debug.Assert(player.CurrentHealth == 35 && player.MaxHealth == 40 && player.Attack == 15, "UsePotion method failed.");
            // EquipWeapon
            player.EquipWeapon(new Weapon("Sword", 10, 1));
            Debug.Assert(player.EquippedWeapon.BaseName == "Sword" && player.Attack == 25 && player.Speed == 1, "EquipWeapon method failed.");
            // UnequipWeapon
            player.UnequipWeapon();
            Debug.Assert(player.EquippedWeapon == null && player.Attack == 15 && player.Speed == 0, "UnequipWeapon method failed.");
            // GainXP
            Console.SetOut(TextWriter.Null); // Suppress Console output
            player.GainXP(1);
            Console.SetOut(consoleOut); // Restore Console output
            Debug.Assert(player.Level == 2, "GainXP method failed.");
        }
        /// <summary>
        /// Method used to test the Inventory class.
        /// </summary>
        public void TestInventory()
        {
            Inventory inventory = new Inventory();
            Console.WriteLine("Inventory limits & adding/removing items.");
            // Weapons
            for (int i = 0; i < 6; i++)
            {
                inventory.AddWeapon(new Weapon($"Sword{i}", 10, 1));
            }
            Debug.Assert(inventory.GetWeapon(4).BaseName == "Sword4", "GetWeapon method failed.");
            Debug.Assert(inventory.WeaponCount() == 5, "Weapon limit surpassed.");
            // Potions
            for (int i = 0; i < 11; i++)
            {
                inventory.AddPotion(new Potion($"Potion{i}", 10, 1, 1));
            }
            Debug.Assert(inventory.GetPotion(9).BaseName == "Potion9", "GetPotion method failed.");
            Debug.Assert(inventory.PotionCount() == 10, "Potion limit surpassed.");

            Console.WriteLine("Inventory removing items.");
            // Weapons
            inventory.RemoveWeapon(inventory.GetWeapon(0));
            Debug.Assert(inventory.WeaponCount() == 4, "RemoveWeapon method failed.");
            Debug.Assert(inventory.GetWeapon(0).BaseName == "Sword1", "GetWeapon method failed.");
            // Potions
            inventory.RemovePotion(inventory.GetPotion(0));
            Debug.Assert(inventory.PotionCount() == 9, "RemovePotion method failed.");
            Debug.Assert(inventory.GetPotion(0).BaseName == "Potion1", "GetPotion method failed.");
        }
        /// <summary>
        /// Method used to test the Room class.
        /// </summary>
        public void TestRoom()
        {
            Room room = new Room(new Dragon(), new List<Potion>() { new Potion("Potion1", 10, 0, 0), new Potion("Potion2", 0, 10, 0), new Potion("Potion3", 0, 0, 10) }, new Weapon("Sword", 10, 1), true);
            Console.WriteLine("Room initialisation.");
            Debug.Assert(room.Monster is Dragon, "Monster initialisation failed.");
            Debug.Assert(room.Potions.Count == 3, "Potions initialisation failed.");
            Debug.Assert(room.Weapon.BaseName == "Sword", "Weapon initialisation failed.");
            Debug.Assert(room.IsBossRoom == true, "IsBossRoom initialisation failed.");

            Console.WriteLine("Room methods.");
            // RemovePotion
            for (int i = 2; i >= 0; i--)
            {
                room.RemovePotion(i);
            }
            Debug.Assert(room.Potions == null, "RemovePotion method failed.");
            // RemoveWeapon
            room.RemoveWeapon();
            Debug.Assert(room.Weapon == null, "RemoveWeapon method failed.");
            // RemoveMonster
            room.RemoveMonster();
            Debug.Assert(room.Monster == null, "RemoveMonster method failed.");

        }
        /// <summary>
        /// Method used to test the Map class.
        /// </summary>
        public void TestMap()
        {
            Map map = new Map();
            Console.WriteLine("Map initialisation.");
            Debug.Assert(map.RoomCount == 0, "Map initialisation failed.");

            Console.WriteLine("Map methods.");
            Room room1 = new Room(new Goblin(), null, null, false);
            Room room2 = new Room(new Orc(), null, null, false);
            Room room3 = new Room(new Troll(), null, null, true);
            // AddRoom
            map.AddRoom(room1);
            map.AddRoom(room2);
            map.AddRoom(room3);
            Debug.Assert(map.RoomCount == 3, "AddRoom method failed.");
            // LastRoom
            Debug.Assert(map.LastRoom(room2) == room1, "LastRoom method failed.");
            Debug.Assert(map.LastRoom(room1) == null, "LastRoom method failed.");
            // NextRoom
            Debug.Assert(map.NextRoom(room1) == room2, "NextRoom method failed.");
            Debug.Assert(map.NextRoom(room3) == null, "NextRoom method failed.");
            // NewestRoom
            Debug.Assert(map.NewestRoom() == room3, "NewestRoom method failed.");
            // GetMap
            Debug.Assert(map.GetMap(room1) == "[|][][]", "GetMap method failed.");
        }
    }
}

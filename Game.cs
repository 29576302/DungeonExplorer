using System;
using System.Media;
using System.Linq;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// This class contains the Main method and is used to run the game. It also controls certain player actions.
    /// </summary>
    internal class Game
    {
        private Player Player;
        private Room CurrentRoom;
        private bool Playing;
        /// <summary>
        /// This is the constructor for the Game class.
        /// </summary>
        /// <param name="player">This is the object used to represent the user's character.</param>
        /// <param name="startingRoom">The room that the player starts in.</param>
        public Game(Player player, Room startingRoom)
        {
            Player = player;
            CurrentRoom = startingRoom;
            Playing = true;

        }
        /// <summary>
        /// This method is used to start the game and contains the main game loop.
        /// </summary>
        public void Start()
        {
            // Basic game loop.
            while (Playing)
            {
                TakeAction();
            }
            if (!Playing)
            {
                Console.WriteLine("Game Over!");
            }
        }
        /// <summary>
        /// Method that enables the user to interract with the game environment.
        /// </summary>
        private void TakeAction()
        {
            // Displays the room description.
            Console.WriteLine($"\n{CurrentRoom.GetDescription()}");
            // Calculates and displays the player's available actions.
            string actions = "\nActions: \nM) Menu";
            if (CurrentRoom.Monster == null)
            {
                if (CurrentRoom.Potions != null)
                {
                    actions += "\nP) Take potions.";
                }
                if (CurrentRoom.Weapon != null)
                {
                    actions += $"\nW) Take {CurrentRoom.Weapon.Name}";
                }
                actions += "\nR) Explore a new room";
            }
            else
            {
                actions += $"\nA) Attack {CurrentRoom.Monster.Name}";
            }
            Console.WriteLine(actions);
            // Asks for and validates user input. While true loop until a valid input is given.
            while (true)
            {
                Console.Write(">");
                // Converts userChoice input to upper to avoid case sensitivity.
                string userChoice = Console.ReadLine().ToUpper().Trim();
                // The user is only allowed to take a potion if there is one or more in the room (and if the monster is dead).
                if (userChoice == "P" && CurrentRoom.Potions != null && CurrentRoom.Monster == null)
                {
                    Console.WriteLine("Which potion would you like to take?");
                    // Generates a list of available potions in the room.
                    string availablePotions = "";
                    for (int i = 0; i < CurrentRoom.Potions.Count; i++)
                    {
                        availablePotions += $"{i + 1}) {CurrentRoom.Potions[i].Name}\n";
                    }
                    Console.Write(availablePotions);
                    // Try catch block to validate user input.
                    while (true)
                    {
                        try
                        {
                            Console.Write(">");
                            int potionChoice = Convert.ToInt32(Console.ReadLine());
                            Player.PlayerInventory.AddPotion(CurrentRoom.Potions[potionChoice - 1]);
                            Console.WriteLine($"You take the {CurrentRoom.Potions[potionChoice - 1].Name}.");
                            CurrentRoom.RemovePotion(potionChoice - 1);
                            break;
                        }
                        catch
                        {
                            Console.WriteLine("Please enter a valid input.");
                        }
                    }
                    break;
                }
                // Similarly to the potion statement,
                // the user is only allowed to take a weapon if one is present in the room (and if the monster is dead).
                else if (userChoice == "W" && CurrentRoom.Weapon != null && CurrentRoom.Monster == null)
                {
                    Player.PlayerInventory.AddWeapon(CurrentRoom.Weapon);
                    Console.WriteLine($"You take the {CurrentRoom.Weapon.Name}.");
                    CurrentRoom.RemoveWeapon();
                    break;
                }
                // The user is only allowed to attack the Monster if it is present in the room.
                else if (userChoice == "A" && CurrentRoom.Monster != null)
                {
                    FightMonster(CurrentRoom.Monster);
                    break;
                }
                // Opens the player menu, where inventory and stats can be viewed.
                else if (userChoice == "M")
                {
                    Player.Menu();
                    break;
                }
                // Generates a new room and assigns it to CurrentRoom.
                else if (userChoice == "R")
                {
                    CurrentRoom = NewRoom();
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid input.");
                }
            }
            // Asks for user input as to not overwhelm the player with text on screen.
            Console.Write("Press Enter to continue.");
            Console.ReadLine();
        }
        /// <summary>
        /// Method to generate a new room. Randomly generates a monster, weapon, and potion,
        /// with the possibility of null values.
        /// </summary>
        /// <returns></returns>
        private Room NewRoom()
        {
            Random random = new Random();
            // A list of monsters and weapons to be randomly selected from.
            Monster[] monsters = new Monster[]
            {
                new Monster("Goblin", 10, 5, 1),
                new Monster("Hobgoblin", 15, 8, 2),
                new Monster("Orc", 20, 10, 3),
                new Monster("Troll", 30, 15, 5),
                null
            };
            Weapon[] weapons = new Weapon[]
            {
                new Weapon("Dagger", 5),
                new Weapon("Sword", 10),
                new Weapon("Great Sword", 15),
                null
            };
            Monster monster = monsters[random.Next(0, monsters.Length)];
            Weapon weapon = weapons[random.Next(0, weapons.Length)];
            List<Potion> potions = new List<Potion>();
            // Randomly generates between 0 and 2 potions.
            for (int i = 0; i < random.Next(0, 3); i++)
            { 
                // Generates potion stats and name. Stats may be 0.

                Potion potion = null;
                string potionName = "";
                int potionHealthRestore = 0;
                int potionHealthBonus = 0;
                int potionDamageBonus = 0;
                if (random.Next(0, 2) == 0)
                {
                    potionHealthRestore = random.Next(5, 16);
                    potionName = $"Health({potionHealthRestore})";
                }
                if (random.Next(0, 6) == 0)
                {
                    potionHealthBonus = random.Next(1, 6);
                    if (potionHealthRestore != 0)
                    {
                        potionName += ", ";
                    }
                    potionName += $"Vitality({potionHealthBonus})";
                }
                if (random.Next(0, 11) == 0)
                {
                    potionDamageBonus = random.Next(1, 6);
                    if (potionHealthRestore != 0 || potionHealthBonus != 0)
                    {
                        potionName += ", ";
                    }
                    potionName += $"Strength({potionDamageBonus})";
                }
                // If all stats are 0, the potion remains null.
                if (!(potionHealthRestore == 0 && potionHealthBonus == 0 && potionDamageBonus == 0))
                {
                    potionName += " Potion";
                    potion = new Potion(potionName, potionDamageBonus, potionHealthRestore, potionHealthBonus);
                }
                potions.Add(potion);
            }
            return new Room(monster, potions, weapon);
        }
        // Method to make player and monster fight.
        private void FightMonster(Monster monster)
        {
            while (Player.IsAlive)
            {
                // If no weapon is equipped, the players's weapon is named bare hands.
                string weapon = "your ";
                if (Player.EquippedWeapon == null)
                {
                    weapon += "bare hands";
                }
                else
                {
                    weapon += Player.EquippedWeapon.Name;
                }
                Console.WriteLine($"\nYou attack the {monster.Name} with {weapon}!");
                Player.AttackTarget(CurrentRoom.Monster);
                Console.ReadKey(); //ReadKey used to segment fight sequence.
                // If the monster is alive, it attacks the player.
                if (CurrentRoom.Monster.IsAlive)
                {
                    Console.WriteLine($"\nThe {monster.Name} attacks you!");
                    CurrentRoom.Monster.AttackTarget(Player);
                    Console.ReadKey();
                }
                // If the monster is dead, the monster is removed from the room.
                else
                {
                    Console.WriteLine($"\nYou defeat the {monster.Name}!");
                    CurrentRoom.RemoveMonster();
                    break;
                }
            }
            // Checks to see if the player is still alive after the fight sequence.
            if (!Player.IsAlive)
            {
                Console.WriteLine("\nYou have died.");
                Playing = false; // Main game loop ends if player dies.
            }
            else
            {
                Console.WriteLine($"You have {Player.CurrentHealth} health remaining.");
            }
        }
    }
}
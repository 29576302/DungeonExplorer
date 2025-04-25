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
        private Player player;
        private Room currentRoom;
        private Map exploredRooms;
        private bool playing;
        private static Random random = new Random();
        /// <summary>
        /// This is the constructor for the Game class.
        /// </summary>
        /// <param name="startingPlayer">This is the object used to represent the user's character.</param>
        /// <param name="startingRoom">The room that the player starts in.</param>
        public Game(Player startingPlayer, Room startingRoom)
        {
            player = startingPlayer;
            currentRoom = startingRoom;
            exploredRooms = new Map();
            exploredRooms.AddRoom(currentRoom);
            playing = true;

        }
        /// <summary>
        /// This method is used to start the game and contains the main game loop.
        /// </summary>
        public void Start()
        {
            // Basic game loop.
            while (playing)
            {
                TakeAction();
            }
            if (!playing)
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
            Console.WriteLine($"\n{currentRoom.GetDescription()}");
            // Calculates and displays the player's available actions.
            string actions = "\nActions: \nM) Menu";
            if (currentRoom.Monster == null)
            {
                if (currentRoom.Potions != null)
                {
                    actions += "\nP) Take potion(s)";
                }
                if (currentRoom.Weapon != null)
                {
                    actions += $"\nW) Take {currentRoom.Weapon.Name}";
                }
                if (exploredRooms.LastRoom(currentRoom) != null)
                {
                    actions += "\nL) Return to last room";
                }
                if (exploredRooms.NewestRoom() == currentRoom)
                {
                    actions += "\nR) Explore a new room";
                }
                else
                {
                    actions += "\nR) Advance to next room";
                }

            }
            else
            {
                actions += $"\nA) Attack {currentRoom.Monster.Name}";
                if (player.Speed >= 1.33f && exploredRooms.LastRoom(currentRoom) != null)
                {
                    actions += "\nF) Attempt to flee.";
                }
            }
            Console.WriteLine(actions);
            // Asks for and validates user input. While true loop until a valid input is given.
            while (true)
            {
                Console.Write(">");
                // Converts userChoice input to upper to avoid case sensitivity.
                string userChoice = Console.ReadLine().ToUpper().Trim();
                // The user is only allowed to take a potion if there is one or more in the room (and if the monster is dead).
                if (userChoice == "P" && currentRoom.Potions != null && currentRoom.Monster == null)
                {
                    if (!player.PlayerInventory.PotionIsFull)
                    {
                        Console.WriteLine("Which potion would you like to take?");
                        // Generates a list of available potions in the room.
                        string availablePotions = "";
                        for (int i = 0; i < currentRoom.Potions.Count; i++)
                        {
                            availablePotions += $"{i + 1}) {currentRoom.Potions[i].Name}\n";
                        }
                        Console.Write(availablePotions);
                        // Try catch block to validate user input.
                        while (true)
                        {
                            try
                            {
                                Console.Write(">");
                                int potionChoice = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine($"You take the {currentRoom.Potions[potionChoice - 1].Name}.");
                                player.PlayerInventory.AddPotion(currentRoom.Potions[potionChoice - 1]);
                                currentRoom.RemovePotion(potionChoice - 1);
                                break;
                            }
                            catch (Exception ex)
                            {
                                if (ex is FormatException || ex is ArgumentOutOfRangeException)
                                {
                                    Console.WriteLine("Please enter a valid input.");
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("You are carrying too many potions to take any more.");
                    }
                    break;
                }
                // Similarly to the potion statement,
                // the user is only allowed to take a weapon if one is present in the room (and if the monster is dead).
                else if (userChoice == "W" && currentRoom.Weapon != null && currentRoom.Monster == null)
                {
                    if (!player.PlayerInventory.WeaponIsFull)
                    {
                        Console.WriteLine($"You take the {currentRoom.Weapon.Name}.");
                        player.PlayerInventory.AddWeapon(currentRoom.Weapon);
                        currentRoom.RemoveWeapon();
                    }
                    else
                    {
                        Console.WriteLine($"You are carrying too many weapons to take the {currentRoom.Weapon.Name}.");
                    }
                    break;
                }
                // The user is only allowed to attack the Monster if it is present in the room.
                else if (userChoice == "A" && currentRoom.Monster != null)
                {
                    FightMonster(currentRoom.Monster);
                    break;
                }
                // Opens the player menu, where inventory and stats can be viewed.
                else if (userChoice == "M")
                {
                    player.Menu(exploredRooms.GetMap(currentRoom));
                    break;
                }
                // Generates a new room and assigns it to CurrentRoom (if there is no monster).
                else if (userChoice == "R" && currentRoom.Monster == null)
                {
                    if (exploredRooms.NewestRoom() == currentRoom)
                    {
                        currentRoom = NewRoom();
                        exploredRooms.AddRoom(currentRoom);
                        break;
                    }
                    else
                    {
                        currentRoom = exploredRooms.NextRoom(currentRoom);
                        break;
                    }
                }
                else if (userChoice == "L" && exploredRooms.LastRoom(currentRoom) != null)
                {
                    currentRoom = exploredRooms.LastRoom(currentRoom);
                    Console.WriteLine("You return to the last room.");
                    break;
                }
                else if (userChoice == "F" && currentRoom.Monster != null && player.Speed >= 1.33f && exploredRooms.LastRoom(currentRoom) != null)
                {
                    Console.WriteLine($"You attempt to flee from the {currentRoom.Monster.Name}.");
                    if (random.Next(0,3) == 0)
                    {
                        Console.WriteLine($"You successfully flee from the {currentRoom.Monster.Name}.");
                        Console.ReadKey();
                        currentRoom = exploredRooms.LastRoom(currentRoom);
                    }
                    else
                    {
                        Console.WriteLine($"You fail to flee from the {currentRoom.Monster.Name}.");
                        Console.ReadKey();
                        FightMonster(currentRoom.Monster);
                    }
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
            // A list of monsters and weapons to be randomly selected from.
            Monster[] monsters = new Monster[]
            {
                new Goblin(),
                new Orc(),
                new Troll(),
                null
            };
            Weapon[] weapons = new Weapon[]
            {
                new Weapon("Dagger", 5, 2),
                new Weapon("Sword", 10, 1),
                new Weapon("Great Sword", 15, 0.5f),
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
                int potionHealthRestore = 0;
                int potionHealthBonus = 0;
                int potionDamageBonus = 0;
                if (random.Next(0, 2) == 0)
                {
                    potionHealthRestore = random.Next(5, 16);
                }
                if (random.Next(0, 6) == 0)
                {
                    potionHealthBonus = random.Next(1, 6);
                }
                if (random.Next(0, 11) == 0)
                {
                    potionDamageBonus = random.Next(1, 6);
                }
                // If all stats are 0, the potion is not added to the list.
                if (!(potionHealthRestore == 0 && potionHealthBonus == 0 && potionDamageBonus == 0))
                {
                    
                    potion = new Potion("Potion", potionDamageBonus, potionHealthRestore, potionHealthBonus);
                    potions.Add(potion);
                }
            }
            // If no potions are generated, the list is set to null.
            if (!potions.Any())
            {
                potions = null;
            }
            return new Room(monster, potions, weapon);
        }
        // Method to make player and monster fight.
        private void FightMonster(Monster monster)
        {
            bool playerCanAttack = true;
            bool monsterCanAttack = true;
            while (player.IsAlive)
            {
                // If no weapon is equipped, the players's weapon is named bare hands.
                string weapon = "your ";
                if (player.EquippedWeapon == null)
                {
                    weapon += "bare hands";
                }
                else
                {
                    weapon += player.EquippedWeapon.Name;
                }
                if (player.Speed >= 0.66f)
                {
                    Console.WriteLine($"\nYou attack the {monster.Name} with {weapon}!");
                    player.AttackTarget(monster);
                    Console.ReadKey(); //ReadKey used to segment fight sequence.
                    // If the player has a fast speed, they can attack twice per turn.
                    if (player.Speed >= 1.33f)
                    {
                        Console.WriteLine($"\nYou attack the {monster.Name} again!");
                        player.AttackTarget(monster);
                        Console.ReadKey();
                    }
                }
                else if (playerCanAttack)
                {
                    Console.WriteLine($"\nYou attack the {monster.Name} with {weapon}!");
                    player.AttackTarget(monster);
                    Console.ReadKey();
                }
                // If the player has a slow speed, they can only attack once per 2 turns.
                if (player.Speed < 0.66f)
                {
                    if (!playerCanAttack)
                    {
                        Console.WriteLine($"\nYou are too slow and the {monster.Name} dodges your attack.");
                        Console.ReadKey();
                    }
                    playerCanAttack = !playerCanAttack;
                }
                if (monster.CurrentHealth < monster.MaxHealth / 3)
                {
                    monster.TryFlee();
                }
                // If the monster is alive, it attacks the player.
                if (monster.IsAlive)
                {
                    if (monster.Speed >= 0.66f)
                    {
                        Console.WriteLine($"\nThe {monster.Name} attacks you!");
                        monster.AttackTarget(player);
                        Console.ReadKey();
                        // If the monster has a fast speed, the monster can attack twice per turn.
                        if (monster.Speed >= 1.33f)
                        {
                            Console.WriteLine($"\nThe {monster.Name} attacks you again!");
                            monster.AttackTarget(player);
                            Console.ReadKey();
                        }
                    }
                    else if (monsterCanAttack)
                    {
                        Console.WriteLine($"\nThe {monster.Name} attacks you!");
                        monster.AttackTarget(player);
                        Console.ReadKey(); ;
                    }
                    // If the monster has a slow speed, it can only attack once per 2 turns.
                    if (monster.Speed < 0.66f)
                    {
                        if (!monsterCanAttack)
                        {
                            Console.WriteLine($"\nThe {monster.Name} is too slow and you dodge its attack!");
                            Console.ReadKey();
                        }
                        monsterCanAttack = !monsterCanAttack;
                    }
                }
                // If the monster is dead, the monster is removed from the room.
                else
                {
                    // If the monster fled, the player does not get this victory message.
                    if (!monster.Fled)
                    {
                        Console.WriteLine($"\nYou defeat the {monster.Name}!");
                        Console.WriteLine($"You gain {monster.Level} XP!");
                        player.GainXP(monster.Level);
                    }
                    currentRoom.RemoveMonster();
                    break;
                }
            }
            // Checks to see if the player is still alive after the fight sequence.
            if (!player.IsAlive)
            {
                Console.WriteLine("\nYou have died.");
                playing = false; // Main game loop ends if player dies.
            }
            else
            {
                Console.WriteLine($"You have {player.CurrentHealth} health remaining.");
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace DungeonExplorer
{
    /// <summary>
    /// This class forms the base of the Player and Monster classes.
    /// </summary>
    [Serializable]
    public abstract class Creature : ICanAttack
    {
        // Creature's stats are encapsulated in the CreatureStats class. Values here point to the stats.
        public string Name { get; protected set; }
        protected CreatureStats Stats;
        public int MaxHealth => Stats.MaxHealth;
        public int CurrentHealth => Stats.CurrentHealth;
        public int Attack => Stats.Attack;
        public float Speed => Stats.Speed;
        public int Level => Stats.Level;
        public bool IsAlive => Stats.CurrentHealth > 0;
        protected static Random dice = new Random();
        /// <summary>
        /// Constructor for the Creature class. A CreatureStats object is created to store the stats of the creature.
        /// </summary>
        /// <param name="name">The string that is displayed when the game refers to the creature.</param>
        /// <param name="health">The HP/health points of the creature.</param>
        /// <param name="attack">The starting attack/damage attribute of the creature.</param>
        /// <param name="level">The level of the creature.</param>
        public Creature(string name, int health, int attack, int level) 
        {
            Name = name;
            Stats = new CreatureStats(health, attack, 1, level, false);

        }
        /// <summary>
        /// A protected constructor is used to create a Creature object without any parameters. Used for Monster subclasses.
        /// </summary>
        protected Creature()
        {
            Stats = new CreatureStats(0, 0, 0, 0, false);
        }
        /// <summary>
        /// AttackTarget method uses a d20 roll to determine damage dealt.
        /// </summary>
        /// <param name="target">The creature that this attack targets.</param>
        public virtual void AttackTarget(Creature target)
        {
            int damage = (Attack * dice.Next(1, 21)) / 20;
            Console.WriteLine($"The attack deals {damage} damage.");
            target.TakeDamage(damage);
        }
        /// <summary>
        /// Used by Traps to deal damage to the target.
        /// </summary>
        /// <param name="damage">The amount of damage dealt.</param>
        public void TakeDamage(int damage)
        {
            Stats.ModifyCurrentHealth(-damage);
        }
        /// <summary>
        /// Abstract method for potion use, as monsters do not have an inventory.
        /// </summary>
        /// <param name="potion">The potion that will be used by the creature.</param>
        public abstract void UsePotion(Potion potion);
    }
    /// <summary>
    /// Player class inherits from Creature. This class is used to represent the player character.
    /// </summary>
    [Serializable]
    public class Player : Creature
    {
        public Weapon EquippedWeapon { get; private set; }
        public Inventory PlayerInventory = new Inventory();
        private bool equipStrongestWeapon = false;
        /// <summary>
        /// Constructor for the Player class.
        /// </summary>
        public Player(string name, int health, int attack, int level) : base(name, health, attack, level)
        {
            Stats = new CreatureStats(health, attack, 0, level, true);
            PlayerInventory.WeaponAdded += WeaponAdded;
        }
        /// <summary>
        /// UsePotion method allows the player to use a potion from their inventory.
        /// </summary>
        /// <param name="potion">The potion that is used from the player's inventory.</param>
        public override void UsePotion(Potion potion)
        {
            // Potion is removed from the player's inventory.
            PlayerInventory.RemovePotion(potion);
            // Potion effects are applied to the player's stats.
            Stats.ModifyMaxHealth(potion.HealthBonus);
            // If the player's (CurrentHealth + potion.HealthRestore) > MaxHealth,
            // the player's health is set to MaxHealth.
            // This is done to avoid the player's health exceeding their max health.
            if (CurrentHealth + potion.HealthRestore > MaxHealth)
            {
                Stats.ModifyCurrentHealth(MaxHealth - CurrentHealth);
            }
            else
            {
                Stats.ModifyCurrentHealth(potion.HealthRestore);
            }
            Stats.ModifyAttack(potion.Damage);
        }
        /// <summary>
        /// Menu method allows the player to check stats and interact with inventory.
        /// </summary>
        /// <param name="map"> A visualisation of explored rooms to be displayed for the player.</param>
        public void Menu(string map)
        {
            while (true)
            {
                Console.WriteLine("\n========Menu========");
                Console.WriteLine("Name: " + Name);
                Console.WriteLine($"Map:\n{map}");
                // Displays stats.
                Console.WriteLine("\nStats:");
                Console.WriteLine($"Health: {CurrentHealth}/{MaxHealth}");
                Console.WriteLine($"Attack: {Attack}");
                Console.WriteLine($"XP: {Stats.XP}/{Level}");
                Console.WriteLine($"Level: {Level}");
                // Displays the player's equipped weapon.
                if (EquippedWeapon == null)
                {
                    Console.WriteLine("Equipped Weapon: None");
                }
                else
                {
                    Console.WriteLine($"Equipped Weapon: {EquippedWeapon.Name}");
                }
                // Displays inventory with PlayerInventory.Contents().
                Console.Write("\nInventory:");
                Console.WriteLine(PlayerInventory.Contents());
                // Displays and determines what actions the user can take.
                string actions = "Actions:";
                if (PlayerInventory.WeaponCount() > 0 && !equipStrongestWeapon)
                {
                    actions += "\nW) Equip Weapon";
                }
                if (EquippedWeapon != null && !equipStrongestWeapon)
                {
                    actions += "\nU) Unequip Weapon";
                }
                if (PlayerInventory.PotionCount() > 0)
                {
                    actions += "\nP) Drink Potion";
                }
                if (PlayerInventory.PotionCount() > 0 || PlayerInventory.WeaponCount() > 0)
                {
                    actions += "\nD) Drop Items";
                }
                if (EquippedWeapon != null || PlayerInventory.WeaponCount() > 0)
                {
                    actions += $"\nA) Auto-Equip Strongest Weapon ({(equipStrongestWeapon ? "ON" : "OFF")})";
                }
                actions += "\nQ) Quit Menu";
                Console.WriteLine(actions);
                Console.Write(">");
                // Takes and validates user input, breaks from while true loop when a valid input is given.
                string userChoice = Console.ReadLine().ToUpper().Trim();
                // Equip weapon option.
                // The player is only able to equip a weapon if they have one in their inventory.
                if (userChoice == "W" && PlayerInventory.WeaponCount() > 0 && !equipStrongestWeapon)
                {
                    // While loop until user enters a valid input.
                    while (true)
                    { 
                        Console.Write("Select the weapon you want to equip, or enter Q to exit: ");
                        userChoice = Console.ReadLine().ToUpper().Trim();
                        if (userChoice == "Q")
                        {
                            break;
                        }
                        else
                        {
                            // Try catch to handle invalid inputs, as a type conversion from string to int is used.
                            try
                            {
                                // Only accepts inputs that are within the range of the player's inventory.
                                int weaponChoice = Convert.ToInt32(userChoice) - 1;
                                if (0 <= weaponChoice && weaponChoice <= PlayerInventory.WeaponCount())
                                {
                                    EquipWeapon(PlayerInventory.GetWeapon(weaponChoice));
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Your input was out of range.");
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Please enter a valid input.");
                            }
                        }
                    }
                }
                // Unequip weapon option.
                // The player is only able to unequip a weapon if they have one equipped.
                else if (userChoice == "U" && EquippedWeapon != null && !equipStrongestWeapon)
                {
                    if (!PlayerInventory.WeaponIsFull)
                    {
                        UnequipWeapon();
                    }
                    else
                    {
                        Console.WriteLine("\nYour have too many weapons in your inventory.");
                        Console.Write("Press Enter to continue.");
                        Console.ReadKey();
                    }
                }
                // Same concept as "W", except for potions instead of weapons.
                else if (userChoice == "P" && PlayerInventory.PotionCount() > 0)
                {
                    while (true)
                    {
                        Console.Write("Select the potion you want to drink, or enter Q to exit: ");
                        userChoice = Console.ReadLine().ToUpper().Trim();
                        if (userChoice == "Q")
                        {
                            break;
                        }
                        else
                        {
                            try
                            {
                                int potionChoice = Convert.ToInt32(userChoice) - 1;
                                if (0 <= potionChoice && potionChoice <= PlayerInventory.PotionCount())
                                {
                                    UsePotion(PlayerInventory.GetPotion(potionChoice));
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Your input was out of range.");
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Please enter a valid input.");
                            }
                        }
                    }
                }
                // Drop items option.
                else if (userChoice == "D" && (PlayerInventory.PotionCount() > 0 || PlayerInventory.WeaponCount() > 0))
                {
                    // The option to drop an item type is only available if the player has at least one item of the type.
                    actions = "";
                    if (PlayerInventory.PotionCount() > 0)
                    {
                        actions += "\nP) Drop Potions";
                    }
                    if (PlayerInventory.WeaponCount() > 0)
                    {
                        actions += "\nW) Drop Weapons";
                    }
                    actions += "\nQ) Quit";
                    Console.WriteLine(actions);
                    // While loop until user enters a valid input.
                    while (true)
                    {
                        Console.Write(">");
                        userChoice = Console.ReadLine().ToUpper().Trim();
                        if (userChoice == "Q")
                        {
                            break;
                        }
                        // The player is only able to drop a potion if they have at least one in their inventory.
                        else if (userChoice == "P" && PlayerInventory.PotionCount() > 0)
                        {
                            Console.WriteLine("Please select the potion you'd like to drop.");
                            // While loop and try catch to handle invalid inputs.
                            while (true)
                            {
                                Console.Write(">");
                                userChoice = Console.ReadLine().Trim();
                                // Try catch to handle invalid inputs when casting to potionChoice.
                                try
                                {
                                    int potionChoice = Convert.ToInt32(userChoice) - 1;
                                    if (0 <= potionChoice && potionChoice < PlayerInventory.PotionCount())
                                    {
                                        Console.WriteLine($"{PlayerInventory.GetPotion(potionChoice).Name} dropped.");
                                        PlayerInventory.RemovePotion(PlayerInventory.GetPotion(potionChoice));
                                        Console.Write("Press Enter to continue.");
                                        Console.ReadKey();
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Your input was out of range.");
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Please enter a valid input.");
                                }
                            }
                            break;
                        }
                        // Same as above, but for weapons.
                        else if (userChoice == "W" && PlayerInventory.WeaponCount() > 0)
                        {
                            Console.WriteLine("Please select the weapon you'd like to drop.");
                            while (true)
                            {
                                Console.Write(">");
                                userChoice = Console.ReadLine().Trim();
                                try
                                {
                                    int weaponChoice = Convert.ToInt32(userChoice) - 1;
                                    if (0 <= weaponChoice && weaponChoice < PlayerInventory.WeaponCount())
                                    {
                                        Console.WriteLine($"{PlayerInventory.GetWeapon(weaponChoice).Name} dropped.");
                                        PlayerInventory.RemoveWeapon(PlayerInventory.GetWeapon(weaponChoice));
                                        Console.Write("Press Enter to continue.");
                                        Console.ReadKey();
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Your input was out of range.");
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Please enter a valid input.");
                                }
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid input.");
                        }

                    }
                }
                // Auto-equip strongest weapon option.
                else if (userChoice == "A" && (EquippedWeapon != null || PlayerInventory.WeaponCount() > 0))
                {
                    equipStrongestWeapon = !equipStrongestWeapon;
                    // If equipStrongestWeapon has been turned on, the strongest weapon is equipped.
                    if (equipStrongestWeapon && PlayerInventory.WeaponCount() > 0)
                    {
                        EquipWeapon(PlayerInventory.StrongestWeapon);
                        Console.WriteLine($"\nAuto-equipped {EquippedWeapon.Name}.");
                        Console.Write("Press Enter to continue.");
                        Console.ReadKey();
                    }
                }
                // Quit menu option.
                else if (userChoice == "Q")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a valid input.");
                }
            }
        }
        /// <summary>
        /// This method is used to equip a weapon from the player's inventory,
        /// while also moving the currently eqipped weapon into the inventory.
        /// </summary>
        /// <param name="weapon">The weapon that is equipped from the player inventory.</param>
        public void EquipWeapon(Weapon weapon)
        {
            // If the player has a weapon equipped,
            // this code makes sure that it is not overriden when a new weapon is equipped.
            if (EquippedWeapon == null)
            {
                EquippedWeapon = weapon;
                PlayerInventory.RemoveWeapon(weapon);
                Stats.ModifyAttack(weapon.Damage);
                Stats.ModifySpeed(weapon.Speed);
            }
            else
            {
                PlayerInventory.AddWeapon(EquippedWeapon);
                Stats.ModifyAttack(-EquippedWeapon.Damage);
                Stats.ModifySpeed(-EquippedWeapon.Speed);
                EquippedWeapon = weapon;
                Stats.ModifyAttack(weapon.Damage);
                Stats.ModifySpeed(weapon.Speed);
                PlayerInventory.RemoveWeapon(weapon);
            }
        }
        /// <summary>
        /// This method is used to uneqip the player's weapon.
        /// </summary>
        public void UnequipWeapon()
        {
            // The player's equipped weapon is checked again to avoid exceptions.
            if (EquippedWeapon != null)
            {
                PlayerInventory.AddWeapon(EquippedWeapon);
                Stats.ModifyAttack(-EquippedWeapon.Damage);
                Stats.ModifySpeed(-EquippedWeapon.Speed);
                EquippedWeapon = null;
            }
        }
        /// <summary>
        /// Method used to gain XP and level up the player.
        /// </summary>
        /// <param name="xp">XP added to the player's level.</param>
        public void GainXP(int xp)
        {
            Stats.ModifyXP(xp);
        }
        /// <summary>
        /// When a weapon is added to the player's inventory,
        /// this method checks if the strongest weapon needs to be equipped.
        /// </summary>
        /// <param name="weapon">The weapon that is added to the player's inventory.</param>
        private void WeaponAdded(Weapon weapon)
        {
            if ((equipStrongestWeapon && weapon == PlayerInventory.StrongestWeapon && weapon.Damage > EquippedWeapon.Damage) || EquippedWeapon == null)
            {
                EquipWeapon(weapon);
                Console.WriteLine($"\nAuto-equipped {weapon.Name}.");
            }
        }
    }
    /// <summary>
    /// Monster subclass
    /// </summary>
    [Serializable]
    public abstract class Monster : Creature
    {
        public bool Fled {get; protected set; }
        protected abstract bool CanFlee { get; }
        /// <summary>
        /// Protected constructor for Monster subclasses to use.
        /// </summary>
        protected Monster() : base()
        {
            Fled = false;
        } 
        /// <summary>
        /// Same as Player's UsePotion method, except the potion is not removed from inventory.
        /// This is because Monsters do not have an inventory.
        /// </summary>
        /// <param name="potion">The potion used by the monster.</param>
        public override void UsePotion(Potion potion)
        {
            Stats.ModifyMaxHealth(potion.HealthBonus);
            if (CurrentHealth + potion.HealthRestore > MaxHealth)
            {
                Stats.ModifyCurrentHealth(MaxHealth - CurrentHealth);
            }
            else
            {
                Stats.ModifyCurrentHealth(potion.HealthRestore);
            }
            Stats.ModifyAttack(potion.Damage);
        }
        /// <summary>
        /// The monster is able to flee from battle. This is done by setting the monster's CurrentHealth to 0 to end the battle.
        /// </summary>
        public void TryFlee()
        {
            if (!CanFlee)
            {
                return;
            }
            // The monster has a 1/3 chance to flee.
            if (dice.Next(0, 3) == 0)
            {
                Console.WriteLine($"\nThe {Name} flees before you're able to finish it off.");
                Fled = true;
                Stats.ModifyCurrentHealth(-CurrentHealth);
            }
        }
    }
    /// <summary>
    /// The weakest but fastest common monster in the game.
    /// Can flee from fights.
    /// </summary>
    [Serializable]
    public class Goblin : Monster
    {
        protected override bool CanFlee => true;
        public Goblin()
        {
            Name = "Goblin";
            Stats.ModifyMaxHealth(10);
            Stats.ModifyCurrentHealth(10);
            Stats.ModifyAttack(5);
            Stats.ModifySpeed(1.5f);
            Stats.ModifyLevel(1);
        }
    }
    /// <summary>
    /// An all-rounder with decent speed and attack.
    /// Can flee from fights.
    /// </summary>
    [Serializable]
    public class Orc : Monster
    {
        protected override bool CanFlee => true;
        public Orc()
        {
            Name = "Orc";
            Stats.ModifyMaxHealth(20);
            Stats.ModifyCurrentHealth(20);
            Stats.ModifyAttack(10);
            Stats.ModifySpeed(1.0f);
            Stats.ModifyLevel(3);
        }
    }
    /// <summary>
    /// The strongest but slowest common monster in the game.
    /// Cannot flee from fights.
    /// </summary>
    [Serializable]
    public class Troll : Monster
    {
        protected override bool CanFlee => false;
        public Troll()
        {
            Name = "Troll";
            Stats.ModifyMaxHealth(30);
            Stats.ModifyCurrentHealth(MaxHealth);
            Stats.ModifyAttack(15);
            Stats.ModifySpeed(0.5f);
            Stats.ModifyLevel(5);
        }
    }
    /// <summary>
    /// The strongest monster in the game.
    /// Cannot flee from fights. Used for the 'boss fight'.
    /// </summary>
    [Serializable]
    public class Dragon : Monster
    {
        protected override bool CanFlee => false;
        public Dragon()
        {
            Name = "Dragon";
            Stats.ModifyMaxHealth(50);
            Stats.ModifyCurrentHealth(MaxHealth);
            Stats.ModifyAttack(15);
            Stats.ModifySpeed(1);
            Stats.ModifyLevel(10);
        }
    }
    /// <summary>
    /// A special type of monster that is used to represent traps.
    /// This monster is not fought and should be removed after it damages the player.
    /// </summary>
    [Serializable]
    public class Trap : Monster
    {
        protected override bool CanFlee => false;
        public Trap()
        {
            Name = "Trap";
            Stats.ModifyMaxHealth(0);
            Stats.ModifyCurrentHealth(0);
            Stats.ModifyAttack(dice.Next(1,6));
            Stats.ModifySpeed(0);
            Stats.ModifyLevel(0);
        }
        // Traps do not use potions.
        public override void UsePotion(Potion potion)
        {
            return;
        }
        // Traps activates and damages target for a set amount.
        public override void AttackTarget(Creature target)
        {
            target.TakeDamage(Attack);
            return;
        }
    }
}
using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// This class forms the base of the Player and Monster classes.
    /// </summary>
    public abstract class Creature
    {
        public string Name { get; private set; }
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int Attack { get; private set; }
        public int Level { get; private set; }
        public bool IsAlive { get; private set; }
        private static Random dice = new Random();
        /// <summary>
        /// Constructor for the Creature class.
        /// </summary>
        /// <param name="name">The string that is displayed when the game refers to the creature.</param>
        /// <param name="health">The HP/health points of the creature.</param>
        /// <param name="attack">The starting attack/damage attribute of the creature.</param>
        /// <param name="level">The level of the creature.</param>
        public Creature(string name, int health, int attack, int level) 
        {
            Name = name;
            MaxHealth = health;
            CurrentHealth = health;
            Attack = attack;
            Level = level;
            IsAlive = true;

        }
        // Protected methods allow subclasses to modify stats for potion use.
        protected void SetAttack(int attack)
        {
            Attack = attack;
        }
        protected void SetMaxHealth(int health)
        {
            MaxHealth = health;
        }
        protected void SetCurrentHealth(int health)
        {
            CurrentHealth = health;
        }
        /// <summary>
        /// AttackTarget method uses a d20 roll to determine damage dealt.
        /// </summary>
        /// <param name="target">The creature that this attack targets.</param>
        public void AttackTarget(Creature target)
        {
            int damage = (Attack * dice.Next(1,21)) / 20; //Attack value acts as a 'modifier' for the d20 roll.
            Console.WriteLine($"The attack deals {damage} damage.");
            if (target.CurrentHealth - damage > 0)
            {
                target.CurrentHealth -= damage;
            }
            // If the target's (current health - damage) <= 0, the target's health is set to 0
            // and the IsAlive bool is set to false (effectively 'killing' the target.
            else
            {
                target.CurrentHealth = 0; 
                target.IsAlive = false;
            }

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
    public class Player : Creature
    {
        public Weapon EquippedWeapon { get; private set; }
        public Inventory PlayerInventory = new Inventory();
        /// <summary>
        /// Constructor for the Player class.
        /// </summary>
        /// <param name="name">Inherited from Creature. Same use as described above.</param>
        /// <param name="health">Inherited from Creature. Same use as described above.</param>
        /// <param name="attack">Inherited from Creature. Same use as described above.</param>
        /// <param name="level">Inherited from Creature. Same use as described above.</param>
        public Player(string name, int health, int attack, int level) : base(name, health, attack, level)
        {
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
            SetMaxHealth(MaxHealth + potion.HealthBonus);
            // If the player's (CurrentHealth + potion.HealthRestore) > MaxHealth,
            // the player's health is set to MaxHealth.
            // This is done to avoid the player's health exceeding their max health.
            if (CurrentHealth + potion.HealthRestore > MaxHealth)
            {
                SetCurrentHealth(MaxHealth);
            }
            else
            {
                SetCurrentHealth(CurrentHealth + potion.HealthRestore);
            }
            SetAttack(Attack + potion.Damage);
        }
        /// <summary>
        /// Menu method allows the player to check stats and use inventory.
        /// </summary>
        public void Menu()
        {
            while (true)
            {
                Console.WriteLine("\n========Menu========");
                Console.WriteLine("Name: " + Name);
                // Displays stats.
                Console.WriteLine("\nStats:");
                Console.WriteLine($"Health: {CurrentHealth}");
                Console.WriteLine($"Attack: {Attack}");
                Console.WriteLine($"Level: {Level}");
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
                if (PlayerInventory.WeaponCount() > 0)
                {
                    actions += "\nW) Equip Weapon";
                }
                if (EquippedWeapon != null)
                {
                    actions += "\nU) Unequip Weapon";
                }
                if (PlayerInventory.PotionCount() > 0)
                {
                    actions += "\nP) Drink Potion";
                }
                actions += "\nQ) Quit Menu";
                Console.WriteLine(actions);
                Console.Write(">");
                // Takes and validates user input, breaks from while true loop when a valid input is given.
                string userChoice = Console.ReadLine().ToUpper().Trim();
                if (userChoice == "W" && PlayerInventory.WeaponCount() > 0)
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
                // The player is only able to unequip a weapon if they have one equipped.
                else if (userChoice == "U" && EquippedWeapon != null)
                {
                    UnequipWeapon();
                }
                // Same concept as above, except for potions instead of weapons.
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
                SetAttack(Attack + weapon.Damage);
            }
            else
            {
                PlayerInventory.AddWeapon(EquippedWeapon);
                SetAttack(Attack - EquippedWeapon.Damage);
                EquippedWeapon = weapon;
                SetAttack(Attack + weapon.Damage);
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
                SetAttack(Attack - EquippedWeapon.Damage);
                EquippedWeapon = null;
            }
        }
    }
    /// <summary>
    /// Monster subclass
    /// </summary>
    public class Monster : Creature
    {
        public Monster(string name, int health, int attack, int level) : base(name, health, attack, level)
        {
        }
        /// <summary>
        /// Same as Player's UsePotion method, except the potion is not removed from inventory.
        /// This is because Monsters do not have an inventory.
        /// </summary>
        /// <param name="potion">The potion used by the monster.</param>
        public override void UsePotion(Potion potion)
        {
            SetMaxHealth(MaxHealth + potion.HealthBonus);
            if (CurrentHealth + potion.HealthRestore > MaxHealth)
            {
                SetCurrentHealth(MaxHealth);
            }
            else
            {
                SetCurrentHealth(CurrentHealth + potion.HealthRestore);
            }
            SetAttack(Attack + potion.HealthBonus);
        }
    }

}
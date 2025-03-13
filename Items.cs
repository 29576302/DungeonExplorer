using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// This class forms the base of the Potion and Weapon classes.
    /// </summary>
    public abstract class Item
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        protected string description;
        /// <summary>
        /// Constructor for the abstract class Item. This class is not meant to be instantiated.
        /// </summary>
        /// <param name="name">The string that is displayed when the game refers to the item.</param>
        /// <param name="damage">The amount of damage the item adds to the player's attack.</param>
        public Item(string name, int damage)
        {
            Name = name;
            Damage = damage;
            description = "";
        }
        /// <summary>
        /// Abstract method, as item values are unique to each subclass.
        /// </summary>
        public abstract void CreateDescription();
        public string GetDescription()
        {
            CreateDescription();
            return description;
        }
    }
    /// <summary>
    /// This class is used to create potions that grant various bonuses to Creatures.
    /// </summary>
    public class Potion : Item
    {
        public int HealthRestore { get; private set; }
        public int HealthBonus { get; private set; }
        /// <summary>
        /// This constructor is used to create a potion with a name, damage, health restore, and health bonus.
        /// </summary>
        /// <param name="name">Inherited from the Item class. It is the string that is displayed when the game refers to the potion.</param>
        /// <param name="damage">Inherited from the Item class. It is the amount of damage the potion adds to the player's attack.</param>
        /// <param name="healthRestore">The amount of health restored by the potion (cannot exceed max health).</param>
        /// <param name="healthBonus">The amount of health added to the player's max health.</param>
        public Potion(string name, int damage, int healthRestore, int healthBonus) : base(name, damage)
        {
            HealthRestore = healthRestore;
            HealthBonus = healthBonus;
        }
        /// <summary>
        /// Potion specific override of CreateDescription.
        /// </summary>
        public override void CreateDescription()
        {
            Console.WriteLine($"{HealthRestore}, {HealthBonus}, {Damage}");
            description = $"Name: {Name}";
            // Only adds attributes if their value > 0.
            if (HealthRestore > 0)
            {
                description += $"\nHealth Restore: {HealthRestore}";
            }
            if (HealthBonus > 0)
            {
                description += $"\nHealth Bonus: {HealthBonus}";
            }
            if (Damage > 0)
            {
                description += $"\nAttack Bonus: {Damage}";
            }
        }
    }
    public class Weapon : Item
    {
        public Weapon(string name, int damage) : base(name, damage)
        {
        }
        /// <summary>
        /// Weapon specific override of CreateDescription.
        /// </summary>
        public override void CreateDescription()
        {
            description = $"Name: {Name}\nDamage: {Damage}";
        }
    }
}

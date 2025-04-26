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
    [Serializable]
    public abstract class Item : IDescribable
    {
        private string baseName;
        public string Name => baseName + $" ({Description})";
        public int Damage { get; private set; }
        public string Description { get; protected set; }
        /// <summary>
        /// Constructor for the abstract class Item. This class is not meant to be instantiated.
        /// </summary>
        /// <param name="name">The string that is displayed when the game refers to the item.</param>
        /// <param name="damage">The amount of damage the item adds to the player's attack.</param>
        public Item(string name, int damage)
        {
            baseName = name;
            Damage = damage;
        }
        /// <summary>
        /// Abstract method, as item values are unique to each subclass.
        /// </summary>
        public abstract void CreateDescription();
    }
    /// <summary>
    /// This class is used to create potions that grant various bonuses to Creatures.
    /// </summary>
    [Serializable]
    public class Potion : Item
    {
        public int HealthRestore { get; private set; }
        public int HealthBonus { get; private set; }
        /// <summary>
        /// This constructor is used to create a potion with a name, damage, health restore, and health bonus.
        /// </summary>
        /// <param name="name">Inherited from the Item class.
        /// It is the string that is displayed when the game refers to the potion.</param>
        /// <param name="damage">Inherited from the Item class.
        /// It is the amount of damage the potion adds to the player's attack.</param>
        /// <param name="healthRestore">The amount of health restored by the potion (cannot exceed max health).</param>
        /// <param name="healthBonus">The amount of health added to the player's max health.</param>
        public Potion(string name, int damage, int healthRestore, int healthBonus) : base(name, damage)
        {
            HealthRestore = healthRestore;
            HealthBonus = healthBonus;
            CreateDescription();
        }
        /// <summary>
        /// Potion specific override of CreateDescription.
        /// </summary>
        public override void CreateDescription()
        {
            // Only adds attributes if their value > 0.
            if (HealthRestore > 0)
            {
                Description += $"Health Restore: {HealthRestore}";
            }
            if (HealthBonus > 0)
            {
                if (Description != null)
                {
                    Description += ", ";
                }
                Description += $"Health Bonus: {HealthBonus}";
            }
            if (Damage > 0)
            {
                if (Description != null)
                {
                    Description += ", ";
                }
                Description += $"Attack Bonus: {Damage}";
            }
        }
    }
    /// <summary>
    /// This class is used to create weapons for the player to use.
    /// </summary>
    [Serializable]
    public class Weapon : Item
    {
        public float Speed { get; private set; }
        public Weapon(string name, int damage, float speed) : base(name, damage)
        {
            Speed = speed;
            CreateDescription();
        }
        /// <summary>
        /// Weapon specific override of CreateDescription.
        /// </summary>
        public override void CreateDescription()
        {
            // If the speed is >1.33, the weapon is fast, if the speed is <0.66 the weapon is slow, otherwise the weapon has a normal speed.
            string speedDescription = Speed >+ 1.33 ? "Fast" : Speed <+ 0.66 ? "Slow" : "Normal";
            Description = $"Damage: {Damage}, Speed: {speedDescription}";
        }
    }
}

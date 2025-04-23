using System;

namespace DungeonExplorer
{
    /// <summary>
    /// This class is used to encapsulate a Creature's stats.
    /// </summary>
    public class CreatureStats
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int Attack { get; private set; }
        public float Speed { get; private set; }
        public int Level { get; private set; }

        /// <summary>
        /// Basic constructor for the CreatureStats class.
        /// </summary>
        /// <param name="health">Value assigned to MaxHealth and CurrentHealth. The HP of a Creature.</param>
        /// <param name="attack">The value used to determine the damage of a Creature's attacks.</param>
        /// <param name="speed">The value used to indicate the speed of a Creature. Used during combat.</param>
        /// <param name="level">Used to indicate a Monster's difficulty level. Used to show progression for Player.</param>
        public CreatureStats(int health, int attack, float speed, int level)
        {
            MaxHealth = health;
            CurrentHealth = health;
            Attack = attack;
            Speed = speed;
            Level = level;
        }

        /// <summary>
        /// Modifies the MaxHealth of a Creature. MaxHealth cannot be negative.
        /// </summary>
        /// <param name="amount">Integer amount to add to MaxHealth. Can be positive or negative.</param>
        public void ModifyMaxHealth(int amount)
        {
            MaxHealth += amount;
            if (MaxHealth < 0)
            {
                MaxHealth = 0;
            }
        }
        /// <summary>
        /// Modifies the CurrentHealth of a Creature. CurrentHealth cannot be negative and cannot exceed MaxHealth.
        /// </summary>
        /// <param name="amount">Integer amount to add to CurrentHealth. Can be positive or negative.</param>
        public void ModifyCurrentHealth(int amount)
        {
            if (CurrentHealth + amount < 0)
            {
                CurrentHealth = 0;
            }
            else if (CurrentHealth + amount > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                CurrentHealth += amount;
            }
        }
        /// <summary>
        /// Modifies the Attack of a Creature. Attack cannot be negative.
        /// </summary>
        /// <param name="amount">Integer amount added to Attack. Can be positive or negative.</param>
        public void ModifyAttack(int amount)
        {
            Attack += amount;
            if (Attack < 0)
            {
                Attack = 0;
            }
        }
        /// <summary>
        /// Modifies the Speed of a Creature. Speed cannot be negative.
        /// </summary>
        /// <param name="amount">Float amount added to Speed. Can be positive or negative.</param>
        public void ModifySpeed(float amount)
        {
            Speed += amount;
            if (Speed < 0)
            {
                Speed = 0;
            }
        }
        /// <summary>
        /// Modifies the Level of a Creature. Level cannot be negative.
        /// </summary>
        /// <param name="amount">Integer amount added to Level. Can be positive or negative.</param>
        public void ModifyLevel(int amount)
        {
            Level += amount;
            if (Level < 0)
            {
                Level = 0;
            }
        }
    }
}

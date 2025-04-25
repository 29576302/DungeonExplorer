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
        public int XP { get; private set; } = 0;
        private readonly bool isPlayer;
        private readonly int baseHealth;
        private readonly int baseAttack;


        /// <summary>
        /// Basic constructor for the CreatureStats class.
        /// </summary>
        /// <param name="health">Value assigned to MaxHealth and CurrentHealth. The HP of a Creature.</param>
        /// <param name="attack">The value used to determine the damage of a Creature's attacks.</param>
        /// <param name="speed">The value used to indicate the speed of a Creature. Used during combat.</param>
        /// <param name="level">Used to indicate a Monster's difficulty level. Used to show progression for Player.</param>
        public CreatureStats(int health, int attack, float speed, int level, bool player)
        {
            MaxHealth = health;
            CurrentHealth = health;
            baseHealth = health;
            Attack = attack;
            baseAttack = attack;
            Speed = speed;
            Level = level;
            isPlayer = player;
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
        /// <summary>
        /// Modifies the XP of a Creature. XP cannot be negative. When XP exceeds Level * 10,
        /// and the Creature is a Player, they level up.
        /// </summary>
        /// <param name="amount">Float amount added to XP. Can be positive or negative.</param>
        public void ModifyXP(int amount)
        {
            XP += amount;
            if (XP < 0)
            {
                XP = 0;
            }
            if (XP >= Level && isPlayer)
            {
                while (XP >= Level)
                {
                    XP -= Level;
                    LevelUp();
                }
            }
        }
        /// <summary>
        /// Mehtod used to level up Player. Increases MaxHealth and Attack and resets CurrentHealth.
        /// </summary>
        private void LevelUp()
        {
            Level++;
            MaxHealth += (int)(baseHealth * Level/10.0f);
            Attack += (int)(baseAttack * Level/10.0f);
            CurrentHealth = MaxHealth;
            Console.WriteLine($"\nYou've leveled up! You are now level {Level}.");
            Console.ReadKey();
        }
    }
}

using System;

namespace DungeonExplorer
{
    public class CreatureStats
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public int Attack { get; private set; }
        public float Speed { get; private set; }
        public int Level { get; private set; }

        public CreatureStats(int health, int attack, float speed, int level)
        {
            MaxHealth = health;
            CurrentHealth = health;
            Attack = attack;
            Speed = speed;
            Level = level;
        }

        public void ModifyMaxHealth(int amount)
        {
            MaxHealth += amount;
            if (MaxHealth < 0)
            {
                MaxHealth = 0;
            }
        }
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
        public void ModifyAttack(int amount)
        {
            Attack += amount;
            if (Attack < 0)
            {
                Attack = 0;
            }
        }
        public void ModifySpeed(float amount)
        {
            Speed += amount;
            if (Speed < 0)
            {
                Speed = 0;
            }
        }
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

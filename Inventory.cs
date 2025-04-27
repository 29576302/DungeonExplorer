using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// This class is used to store and manage the player's items.
    /// </summary>
    [Serializable]
    public class Inventory
    {
        private List<Weapon> weapons = new List<Weapon>();
        private List<Potion> potions = new List<Potion>();
        private const int maxWeapons = 5;
        private const int maxPotions = 10;
        public bool WeaponIsFull => weapons.Count >= maxWeapons;
        public bool PotionIsFull => potions.Count >= maxPotions;
        public event Action<Weapon> WeaponAdded; // Used to notify Player when a weapon is added to the inventory.
        public Weapon StrongestWeapon
        {
            get
            {
                if (weapons.Count == 0)
                {
                    return null;
                }
                return weapons.OrderByDescending(w => w.Damage).First();
            }
        }
        // Methods for accessing information about inventory contents.
        public int WeaponCount()
        {
            return weapons.Count;
        }
        public int PotionCount()
        {
            return potions.Count;
        }
        public Weapon GetWeapon(int index)
        {
            return weapons[index];
        }
        public Potion GetPotion(int index)
        {
            return potions[index];
        }
        // Methods to add and remove items from the inventory.
        public void AddWeapon(Weapon weapon)
        {
            if (!WeaponIsFull)
            {
                weapons.Add(weapon);
                WeaponAdded?.Invoke(weapon);
            }
        }
        public void AddPotion(Potion potion)
        {
            if (!PotionIsFull)
            {
                potions.Add(potion);
            }
        }
        public void RemoveWeapon(Weapon weapon)
        {
            weapons.Remove(weapon);
        }
        public void RemovePotion(Potion potion)
        {
            potions.Remove(potion);
        }
        /// <summary>
        /// Method returns the contents of the inventory.
        /// </summary> 
        public string Contents()
        {
            string contents = "";
            if (WeaponCount() > 0)
            {
                contents += "\nWeapons:\n";
                for (int i = 0; i < weapons.Count; i++)
                {
                    contents += $"{i + 1}) {weapons[i].Name} \n";
                }
            }
            if (PotionCount() > 0)
            {
                contents += "\nPotions:\n";
                for (int i = 0; i < potions.Count; i++)
                {
                    contents += $"{i + 1}) {potions[i].Name} \n";
                }
            }
            if (contents == "")
            {
                contents = "\nYour inventory is empty.\n";
            }
            return contents;
        }
    }
}

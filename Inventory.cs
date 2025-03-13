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
    public class Inventory
    {
        private List<Weapon> weapons = new List<Weapon>();
        private List<Potion> potions = new List<Potion>();
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
        public void AddWeapon(Weapon weapon)
        {
            weapons.Add(weapon);
        }
        public void AddPotion(Potion potion)
        {
            potions.Add(potion);
        }
        public void RemoveWeapon(Weapon weapon)
        {
            weapons.Remove(weapon);
        }
        public void RemovePotion(Potion potion)
        {
            potions.Remove(potion);
        }
        // Method returns the contents of the inventory.
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

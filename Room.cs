using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// This class is used to create rooms that the player explores.
    /// </summary>
    public class Room : IDescribable
    {
        public Monster Monster { get; private set; }
        public List<Potion> Potions { get; private set; }
        public Weapon Weapon { get; private set; }
        public bool IsBossRoom { get; private set; }
        private string description; // Accessed through methods detailed below.
        /// <summary>
        /// This is the constructor for the Room class.
        /// </summary>
        /// <param name="monster" >The monster that will occupy the room.</param>
        /// <param name="potions">The potions that will be in the room.
        /// A list of potions is used, as more than one can appear in a room.</param>
        /// <param name="weapon">The weapon that will be in the room.</param>
        public Room(Monster monster, List<Potion> potions, Weapon weapon, bool isBossRoom)
        {
            Monster = monster;
            Potions = potions;
            Weapon = weapon;
            description = "";
            CreateDescription();
            IsBossRoom = isBossRoom;
        }
        /// <summary>
        /// Generates a description of the room.
        /// </summary>
        public void CreateDescription()
        {
            description = "Room Contents:";
            description += "\nMonster: ";
            if (Monster == null)
            {
                description += "There is no monster in the room.";
            }
            else
            {
                description += Monster.Name;
            }
            description += "\nPotions: ";
            if (Potions == null)
            {
                description += "There is no potion in the room.";
            }
            else
            {
                description += string.Join(", ", Potions.Select(p => p.Name)); // LINQ to join potion names.
            }
            description += "\nWeapon: ";
            if (Weapon == null)
            {
                description += "There is no weapon in the room.";
            }
            else
            {
                description += Weapon.Name;
            }
        }
        /// <summary>
        /// Returns the description of the room after making sure that it is up to date with the current contents.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            CreateDescription();
            return description;
        }
        // Methods to remove items/monsters from the room.
        public void RemoveWeapon()
        {
            Weapon = null;
        }
        public void RemovePotion(int index)
        {
            Potions.RemoveAt(index);
            if (Potions.Count == 0)
            {
                Potions = null;
            }
        }
        public void RemoveMonster()
        {
            Monster = null;
        }
        public bool IsEmpty()
        {
            return Monster == null && Potions == null && Weapon == null;
        }
    }
}
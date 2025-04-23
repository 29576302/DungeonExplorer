using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// This class is used to test various instantiations and interactions in the game.
    /// </summary> 
    class Test
    {
        public void TestHealth()
        {
            Player player = new Player("Player", 30, 0, 1);
            Debug.Assert(player.CurrentHealth == 30, "Player health should be 30.");

            Monster monster = new Dragon();
            while (player.CurrentHealth > 0)
            {
                Console.WriteLine("The monster attacks the player.");
                monster.AttackTarget(player);
            }
            Debug.Assert(player.CurrentHealth <= 0, "Health should not drop below 0.");

            player.UsePotion(new Potion("Potion", 0, 10, 0));
            Debug.Assert(player.CurrentHealth == 10, "Player health should be 10 after healing.");

            player.UsePotion(new Potion("Potion", 0, 100, 0));
            Debug.Assert(player.CurrentHealth == 30, "Player health should not exceed 30.");
        }
        public void TestInventory()
        {
            Player player = new Player("Player", 30, 0, 1);
            player.PlayerInventory.AddPotion(new Potion("Potion", 0, 10, 0));
            Debug.Assert(player.PlayerInventory.PotionCount() == 1, "Player should have 1 potion in inventory.");

            player.UsePotion(player.PlayerInventory.GetPotion(0));
            Debug.Assert(player.PlayerInventory.PotionCount() == 0, "Player should have 0 potions after using.");

            player.PlayerInventory.AddWeapon(new Weapon("Sword", 10, 1));
            Debug.Assert(player.PlayerInventory.WeaponCount() == 1, "Player should have 1 weapon in inventory.");

            player.EquipWeapon(player.PlayerInventory.GetWeapon(0));
            Debug.Assert(player.EquippedWeapon.Name == "Sword", "Player should have equipped the sword.");
            Debug.Assert(player.Attack == 10, "Player attack should be 10.");
            Debug.Assert(player.PlayerInventory.WeaponCount() == 0, "Player should have 0 weapons in inventory.");

            player.PlayerInventory.AddWeapon(new Weapon("Dagger", 5, 2));
            player.EquipWeapon(player.PlayerInventory.GetWeapon(0));
            Debug.Assert(player.EquippedWeapon.Name == "Dagger", "Player should have equipped the new weapon.");
            Debug.Assert(player.Attack == 5, "Player attack should be 5.");
            Debug.Assert(player.PlayerInventory.WeaponCount() == 1, "Player should have the sword in their inventory.");

            player.UnequipWeapon();
            Debug.Assert(player.EquippedWeapon == null, "Player should have no equipped weapon.");
        }
        public void TestRoom()
        {
            Room room = new Room(new Goblin(), new List<Potion>() { new Potion("Potion", 0, 10, 0) }, new Weapon("Sword", 10, 1));
            Debug.Assert(room.Monster.Name == "Goblin", "A goblin should be in the room.");
            Debug.Assert(room.Potions[0].Name == "Potion", "Potion should be in the room.");
            Debug.Assert(room.Weapon.Name == "Sword", "Sword should be in the room.");

            room.RemoveMonster();
            Debug.Assert(room.Monster == null, "Monster value should be null.");

            room.RemovePotion(0);
            Debug.Assert(room.Potions == null, "Potion value should be null.");

            room.RemoveWeapon();
            Debug.Assert(room.Weapon == null, "Weapon value should be null.");
        }
        public void TestItems()
        {
            Weapon weapon = new Weapon("Sword", 10, 1);
            Debug.Assert(weapon.Name == "Sword", "Weapon name should be 'Sword'.");
            Debug.Assert(weapon.Damage == 10, "Weapon damage should be 10.");
            Debug.Assert(weapon.Speed == 1, "Weapon speed should be 1.");

            string description = weapon.GetDescription();
            Console.WriteLine(description);
            Debug.Assert(description == "Name: Sword\nDamage: 10", "Weapon description should be correct.");

            Potion potion = new Potion("Potion", 0, 10, 0);
            Debug.Assert(potion.Name == "Potion", "Potion name should be 'Potion'.");
            Debug.Assert(potion.HealthRestore == 10, "Potion should restore 10 health.");

            description = potion.GetDescription();
            Console.WriteLine(description);
            Debug.Assert(description == "Name: Potion\nHealth Restore: 10", "Potion description should be correct.");
        }
    }
}

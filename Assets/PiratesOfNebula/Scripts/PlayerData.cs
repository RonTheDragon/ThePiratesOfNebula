using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int Money;
    public bool[] Shop;
    public int[] Inventory;
    public int[] ItemSlots;
    public PlayerData(GameObject Player)
    {
        Currency c = Player.GetComponent<Currency>(); // Set Money and Shop
        Money = c.Money;
        Shop = new bool[c.Items.Count];
        for (int i = 0; i < c.Items.Count; i++)
        {
            Shop[i] = c.Items[i].checkifOwned();
        }

        ShipsManagement s = Player.GetComponent<ShipsManagement>(); //Organize Inventory
        Inventory = new int[s.Weapons.Count];
        for (int i = 0; i < s.Weapons.Count; i++)
        { 
            for (int j = 0; j < s.AllWeaponsInGameList.Count; j++)
            {
                if (s.Weapons[i].GetComponent<Weapon>().Id == s.AllWeaponsInGameList[j].GetComponent<Weapon>().Id) {   Inventory[i] = j; break; }
            }
        }

        PlayerControl p = s.Spaceship.GetComponent<PlayerControl>(); //Set Weapons Slots
        ItemSlots = new int[p.Cannons.Length];
        for (int i = 0; i < p.Cannons.Length; i++)
        {
            for (int j = 0; j < s.AllWeaponsInGameList.Count; j++)
            {
                if (p.Cannons[i].GetComponent<Weapon>().Id == s.AllWeaponsInGameList[j].GetComponent<Weapon>().Id) { ItemSlots[i] = j;  break; }
            }
        }
    }
}

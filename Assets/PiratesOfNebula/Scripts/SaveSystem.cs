using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void Save(GameObject Player, int SaveSlot)
    {
        BinaryFormatter f = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.SaveFile" + SaveSlot.ToString();
        FileStream s = new FileStream(path, FileMode.Create);
        PlayerData d = new PlayerData(Player);
        f.Serialize(s, d);
        s.Close();
    }
    public static PlayerData Load(int SaveSlot)
    {
        string path = Application.persistentDataPath + "/player.SaveFile" + SaveSlot.ToString();
        if (File.Exists(path))
        {
            BinaryFormatter f = new BinaryFormatter();
            FileStream s = new FileStream(path, FileMode.Open);
            PlayerData d = f.Deserialize(s) as PlayerData;
            s.Close();
            return d;
        }
        else
        {
            Debug.LogError($"Savefile not found in {path}");
            return null;
        }
    }
}

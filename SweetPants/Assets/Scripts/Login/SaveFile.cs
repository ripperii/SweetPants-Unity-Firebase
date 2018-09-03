using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    public static bool SaveToFile(string pwd, out string error)
    {
        try
        {
            error = "";
            Debug.Log("File path: " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(Application.persistentDataPath + "/SaveData.dat"))
            {
                error = "File Exists!";
                return false;
            }
            FileStream fs = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.OpenOrCreate);

            SaveData sd = new SaveData(pwd);

            bf.Serialize(fs, sd);

            return true;
        }
        catch (Exception e)
        {
            error = e.ToString();
            return false;
        }
    }
    public static bool LoadFromFile(out string pwd, out string error)
    {
        try
        {
            if (!File.Exists(Application.persistentDataPath + "/SaveData.dat"))
            {
                pwd = "";
                error = "File does not Exist!";
                return false;
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/SaveData.dat", FileMode.Open);

            SaveData sd = (SaveData)bf.Deserialize(fs);

            pwd = sd.password;
            error = "";
            return true;
        }
        catch (Exception e)
        {
            pwd = "";
            error = e.ToString();
            return false;
        }
    }
    [Serializable]
    public class SaveData
    {
        public string password;
        public SaveData(string pwd)
        {
            password = pwd;
        }
    }
}

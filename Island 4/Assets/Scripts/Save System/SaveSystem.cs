using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    #region Save Settings
    public static void SaveSettings(Settings S)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Settings.SMK";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(S);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/Settings.SMK";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
    #endregion Save Settings

    #region Save Game
    public static void SaveGame(GameManager G)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Game.SMK";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(G);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/Game.SMK";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
    #endregion Save Game

    #region Save Buildings
    public static void SaveBuildings(BuildingPlacement B)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Buildings.SMK";
        FileStream stream = new FileStream(path, FileMode.Create);

        BuildingData data = new BuildingData(B);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static BuildingData LoadBuildings()
    {
        string path = Application.persistentDataPath + "/Buildings.SMK";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BuildingData data = formatter.Deserialize(stream) as BuildingData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
    #endregion Save Buildings
}
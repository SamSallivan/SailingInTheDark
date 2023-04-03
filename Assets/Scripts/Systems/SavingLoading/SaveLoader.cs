using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoader
{
    public static void SaveData(SaveManager SM)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Game.weeklyhow";

        FileStream stream = new FileStream(path, FileMode.Create);

        DataFile charData = new DataFile(SM);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static DataFile LoadData()
    {
        string path = Application.persistentDataPath + "/Game.weeklyhow";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataFile charData = formatter.Deserialize(stream) as DataFile;
            Debug.Log(Application.persistentDataPath);

            stream.Close();

            return charData;
        }
        else
        {
            Debug.LogError("either no save file, or save file failed to load");
            return null;
        }
    }

}

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoader
{
    public static void SaveData(SaveManager SM, float current, float max)
    {
        Debug.Log("making a save file");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = $"{Application.persistentDataPath}/Your Save File";

        FileStream stream = new FileStream(path, FileMode.Create);

        DataFile charData = new DataFile(SM, current, max);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static DataFile LoadData()
    {
        string path = $"{Application.persistentDataPath}/Your Save File";

        if (File.Exists(path))
        {
            Debug.Log("loading a save file");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataFile charData = formatter.Deserialize(stream) as DataFile;
            stream.Close();

            return charData;
        }
        else
        {
            Debug.Log("either no save file, or save file failed to load");
            return null;
        }
    }

    public static void DeleteSaveData()
    {
        string path = $"{Application.persistentDataPath}/Your Save File";
        File.Delete(path);
    }
}

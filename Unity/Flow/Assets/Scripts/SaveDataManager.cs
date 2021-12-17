using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveDataManager
{
    [System.Serializable]
    public struct LevelLotData
    {
        public string name;
        public int lastUnlockedLevel;
        public int playedLevels;
        public int[] bestMovesPerLevel;

    }
    [System.Serializable]
    public struct sectionData
    {

        public string name;
        public LevelLotData[] levelLots;
    }

    public SaveDataManager()
    {
        if (!File.Exists(Application.persistentDataPath + "/savedata.json"))
            createNewData();
        else
            load();
    }
    /// <summary>
    /// initializes the internal structure for a new save data
    /// </summary>
    void createNewData()
    {
        Section[] gameManagerSections = GameManager.instance.GetSections();
        sections = new sectionData[gameManagerSections.Length];
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].levelLots = new LevelLotData[gameManagerSections[i].levelLots.Length];
            sections[i].name = gameManagerSections[i].SectionName;
            for (int j = 0; j < sections[i].levelLots.Length; j++)
            {
                string[] separators = { "\n", "\r", "\r\n", "\n\r" };

                int levels = gameManagerSections[i].levelLots[j].LevelLotFile.ToString().Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Length;
                sections[i].levelLots[j].name = gameManagerSections[i].levelLots[j].LevelLotName;
                if (gameManagerSections[i].levelLots[j].UnlockAll)
                    sections[i].levelLots[j].lastUnlockedLevel = levels - 1;
                else
                    sections[i].levelLots[j].lastUnlockedLevel = 0;
                sections[i].levelLots[j].bestMovesPerLevel = new int[levels];
                int[] bestMoves = sections[i].levelLots[j].bestMovesPerLevel;
                for (int k = 0; k < bestMoves.Length; k++)
                {
                    bestMoves[k] = -1;
                }

            }
        }
    }
    /// <summary>
    /// Saves the data with a hash and pepper
    /// </summary>
    /// <returns>True in order to fit the Application wants to quit event</returns>
    public bool save()
    {
        //we generate the json without pepper
        string json = JsonUtility.ToJson(this);
        //we add the pepper
        json += "Q:c8!r7pb2L)<6~A";
        //we generate the hash
        hash = hashFunction(json);
        //we generate the true json with the hash
        json = JsonUtility.ToJson(this);
        //we save the json
        StreamWriter file = new StreamWriter(Application.persistentDataPath + "/savedata.json");
        file.Write(json);
        file.Close();
        return true;
    }
    /// <summary>
    /// Loads the saved data. If the user modified the file, deletes the progress
    /// </summary>
    void load()
    {
        //we read the json
        StreamReader file = new StreamReader(Application.persistentDataPath + "/savedata.json");
        string json = file.ReadLine();
        file.Close();
        //we deserialize the json
        JsonUtility.FromJsonOverwrite(json, this);
        //we store the hash to check the legitimacy of the file and set our hash to empty
        string auxHash = hash;
        hash = "";
        //we serialize the object without the hash and add the pepper
        json = JsonUtility.ToJson(this);
        json += "Q:c8!r7pb2L)<6~A";
        //if the hash we stored previously and the hash we just generated aren't equal, it means the user 
        //tried to cheat, so we delete the progress
        if (hashFunction(json) != auxHash)
        {
            File.Delete(Application.persistentDataPath + "savedata.json");
            createNewData();
        }
    }
    string hashFunction(string input)
    {
        SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Convert byte array to a string   
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
    public int numHints;
    public string hash = "";
    public sectionData[] sections;
}

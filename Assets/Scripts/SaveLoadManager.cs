using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour {

    public GameManager GameManager;

    public void LoadProfiles()
    {
        string profiles = File.ReadAllText(Application.dataPath + "/Profiles.txt");
        GameManager.Profiles = JsonUtility.FromJson<ArrayWraper>(profiles).DataArray;
        for (int i = 0; i < GameManager.Profiles.Length; i++)
        {
            if (GameManager.Profiles[i].Name == "")
            {
                GameManager.Profiles[i] = null;
            }
        }

    }

    public void LoadTopTenList()
    {
        string topTen = File.ReadAllText(Application.dataPath + "/TopTen.txt");
        GameManager.TopTen = JsonUtility.FromJson<ArrayWraper>(topTen).DataArray;
    }

    public void SaveProfiles()
    {
        ArrayWraper temp = new ArrayWraper();
        temp.DataArray = new PlayerData[GameManager.Profiles.Length];
        for (int i = 0; i < GameManager.Profiles.Length; i++)
        {
            if (GameManager.Profiles[i] == null)
                temp.DataArray[i] = null;
            else
                temp.DataArray[i] = GameManager.Profiles[i];
        }
        string profiles = JsonUtility.ToJson(temp);
        File.WriteAllText(Application.dataPath + "/Profiles.txt", profiles);
    }

    public void SaveTopTenList()
    {
        ArrayWraper temp = new ArrayWraper();
        temp.DataArray = GameManager.TopTen;
        string topTen = JsonUtility.ToJson(temp);
        File.WriteAllText(Application.dataPath + "/TopTen.txt", topTen);
        Debug.Log("Saved");
    }
}

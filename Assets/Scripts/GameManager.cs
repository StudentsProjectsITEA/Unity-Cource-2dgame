using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player Player;
    public PowerUpManager PowerUpManager;
    public SaveLoadManager SaveLoadManager;
    public PlatformManager PlatformManager;
    public Text CoinsText, ScoreText, GameOverText, PressContinueText, Best;
    public Text NewGameButtonText;
    public MainMenu MainMenu;
    public int CoinCount;
    public float Score;
    public bool IsGameOver = false;
    public GameObject profileNameList;

    public GameObject TopTenList;
    public InputField PlayerNameInput;
    public int SelectedProfileID = 20;

    [HideInInspector]
    public PlayerData[] Profiles;
    [HideInInspector]
    public PlayerData[] TopTen;

    private PlayerData PlayerData;
    
    private Text[] profileNames;
    private Selectable[] profilesFields;
    private int CurrentProfileID = 20;

    private void Awake()
    { 
        try
        {
            SaveLoadManager.LoadProfiles();
        }
        catch (System.Exception)
        {
            Profiles = new PlayerData[10];
            for (int i = 0; i < Profiles.Length; i++)
            {
                Profiles[i] = null;
            }
            SaveLoadManager.SaveProfiles();
        }
        try
        {
            SaveLoadManager.LoadTopTenList();
        }
        catch (System.Exception)
        {
            TopTen = new PlayerData[10];
            for (int i = 0; i < TopTen.Length; i++)
            {
                TopTen[i] = new PlayerData();
            }
            SaveLoadManager.SaveTopTenList();
        }

        LoadTopTenToGame();
        LoadProfilesToGame();

        PlayerData = new PlayerData();
        PlayerData.Name = "Guest";
        PlayerData.Coins = 0;
        PlayerData.MaxScore = 0;
        Best.text = string.Format("{0}\nMax Score: {1}\nCoins: {2}",PlayerData.Name, PlayerData.MaxScore, PlayerData.Coins);
        NewGameButtonText.text = "NEW GAME";
        Score = 0;
        MainMenu.gameObject.SetActive(false);
        Time.timeScale = 0;
        
        GameOverText.gameObject.SetActive(false);
        IsGameOver = true;
        PlatformManager.PlatformGeneration();
    }

    private void SaveData()
    {
        SaveProfileData();
        SaveLoadManager.SaveProfiles();
        SaveLoadManager.SaveTopTenList();
    }

    private void Update()
    {
        bool isSubMenuActive = MainMenu.OptionsPanel.gameObject.activeSelf ||
                MainMenu.ShopPanel.gameObject.activeSelf ||
                MainMenu.TopTenPanel.gameObject.activeSelf ||
                MainMenu.ProfilesPanel.gameObject.activeSelf ||
                MainMenu.EnterNamePanel.gameObject.activeSelf;

        if (IsGameOver)
            if (Input.GetKeyDown(KeyCode.Space) && !isSubMenuActive && !MainMenu.gameObject.activeSelf)
            {
                StartGame();
            }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (!MainMenu.gameObject.activeSelf && !isSubMenuActive)
            {
                PauseGame();
            }
            else if (isSubMenuActive)
            {
                Debug.Log("Return");
                MainMenu.Return();
            }
            else if (!IsGameOver)
            {
                Debug.Log("Continue");
                StartCoroutine(ContinueGame());
            }
            else
            {
                MainMenu.gameObject.SetActive(false);
            }
        }
        Score += Time.deltaTime * 100;
        ScoreText.text = "Score: " + (int)Score;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        Score = 0;
        IsGameOver = false;
        Best.text = string.Format("{0}\nMax Score: {1}\nCoins: {2}", PlayerData.Name, PlayerData.MaxScore, PlayerData.Coins);
        GameOverText.gameObject.SetActive(false);
        PressContinueText.gameObject.SetActive(false);
        CoinCount = PlayerData.Coins;
        PlatformManager.Clear();
        PlatformManager.PlatformGeneration();
        CoinsText.text = "Coins: " + CoinCount;
        NewGameButtonText.text = "CONTINUE";
        PowerUpManager.NewGame(PlayerData.IsMagnetAvailable, PlayerData.IsPowerJumpAvailable);
        Player.transform.position = new Vector3(-7.3f, -1.9f, 0f);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        PlayerData.Coins = CoinCount;
        PlayerData.MaxScore = (int)Score > PlayerData.MaxScore ? (int)Score : PlayerData.MaxScore;
        PlayerData.IsMagnetAvailable = PowerUpManager.IsMagnetAvailable;
        PlayerData.IsPowerJumpAvailable = PowerUpManager.IsPowerJumpAvailable;
        GameOverText.gameObject.SetActive(true);
        NewGameButtonText.text = "NEW GAME";
        PressContinueText.gameObject.SetActive(true);
        PowerUpManager.StopAllPowerUpCoroutines();
        IsGameOver = true;
        SaveData();
    }

    public void PauseGame()
    {
        MainMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public IEnumerator ContinueGame()
    {
        MainMenu.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }

    //Load Top 10 array to Top10 UI submenu
    private void LoadTopTenToGame()
    {
        if (TopTen != null)
        {
            for (int i = 0; i < TopTen.Length; i++)
            {
                if (TopTen[i].Name != "")
                {
                    TopTenList.transform.GetChild(0).GetComponentsInChildren<Text>()[i].text = TopTen[i].Name;
                    TopTenList.transform.GetChild(1).GetComponentsInChildren<Text>()[i].text = TopTen[i].MaxScore.ToString();
                }
            }
        }
    }

    //Load Profiles array to Profiles UI submenu
    private void LoadProfilesToGame()
    {
        profileNames = profileNameList.GetComponentsInChildren<Text>();
        profilesFields = profileNameList.GetComponentsInChildren<Selectable>();

        for (int i = 0; i < Profiles.Length; i++)
        {
            if (Profiles[i] != null)
            {
                profileNames[i].text = Profiles[i].Name;
            }
            else
            {
                profileNames[i].text = "";
                profilesFields[i].interactable = false;
                Profiles[i] = null;
            }
        }
    }

    private void AddToTopTen(PlayerData playerData)
    {
        PlayerData temp = null;
        for (int i = 0; i < TopTen.Length; i++)
        {
            for (int j = 0; j < TopTen.Length - 1; j++)
            {
                if (TopTen[j].MaxScore < TopTen[j + 1].MaxScore)
                {
                    temp = TopTen[j + 1];
                    TopTen[j + 1] = TopTen[j];
                    TopTen[j] = temp;
                }
            }
        }
       
        for (int i = 0; i < TopTen.Length; i++)
        {
            if (TopTen[i].MaxScore < Score)
            {
                for (int j = TopTen.Length - 1; j > i; j--)
                {
                    TopTen[j] = TopTen[j - 1];
                }
                TopTen[i] = new PlayerData();
                TopTen[i].Name = playerData.Name;
                TopTen[i].MaxScore = (int)Score;
                return;
            }
        }
    }

    //Load Data of selected in UI profile to game
    public void LoadProfileData()
    {
        if (SelectedProfileID < 10)
        {
            CurrentProfileID = SelectedProfileID;
            PlayerData = Profiles[SelectedProfileID];
            PowerUpManager.LoadPlayerData(PlayerData.IsMagnetAvailable, PlayerData.IsPowerJumpAvailable);
        }
        else
        {
            if (PlayerData != null)
            {
                if (PlayerData.Name != "Guest")
                {
                    LoadGuestProfile();
                    CurrentProfileID = SelectedProfileID;
                }
            }
        }
        MainMenu.ProfilesPanel.gameObject.SetActive(false);
        NewGameButtonText.text = "NEW GAME";
        IsGameOver = true;
        PressContinueText.gameObject.SetActive(true);
    }

    public void LoadGuestProfile()
    {
        PlayerData = new PlayerData();
        PlayerData.Name = "Guest";
        PlayerData.Coins = 0;
        PlayerData.MaxScore = 0;
    }

    //Save current results to current Profile
    private void SaveProfileData()
    {
        if (CurrentProfileID < 10)
        {
            Profiles[CurrentProfileID] = PlayerData;
        }
        AddToTopTen(PlayerData);
        LoadTopTenToGame();
    }

    //Open "Add new profile window"
    public void NewProfile()
    {
        for (int i = 0; i < Profiles.Length; i++)
        {
            if (Profiles[i] == null)
            {
                MainMenu.EnterNamePanel.gameObject.SetActive(true);
                return;
            }
        }
        Debug.Log("No empty slots, delete some");
    }

    //Save new added profile
    public void SaveNewProfile()
    {
        for (int i = 0; i < Profiles.Length; i++)
        {
            if (Profiles[i] == null)
            {
                Profiles[i] = new PlayerData();
                Profiles[i].Name = PlayerNameInput.text;
                if (Profiles[i].Name == "")
                {
                    Profiles[i] = null;
                    return;
                }
                profileNameList.transform.GetChild(i).GetComponentInChildren<Text>().text = Profiles[i].Name;
                profileNameList.transform.GetChild(i).GetComponent<Selectable>().interactable = true;
                MainMenu.EnterNamePanel.gameObject.SetActive(false);
                return;
            }
        }
    }

    //Delete selected profile
    public void DeleteProfile()
    {
        if (SelectedProfileID < 10)
        {
            profileNameList.transform.GetChild(SelectedProfileID).GetComponentInChildren<Text>().text = "";
            profileNameList.transform.GetChild(SelectedProfileID).GetComponent<Selectable>().interactable = false;
            profileNameList.transform.GetChild(SelectedProfileID).SetAsLastSibling();
            PlayerData temp = null;
            for (int i = SelectedProfileID; i < Profiles.Length - 1; i++)
            {
                temp = Profiles[i + 1];
                Profiles[i + 1] = Profiles[i];
                Profiles[i] = temp;
            }
            Profiles[Profiles.Length - 1] = null;
        }
        SelectedProfileID = 20;
    }


    //Show selected profile results
    public void InitializeUI()
    {
        Best.text = string.Format("{0}\nMax Score: {1}\nCoins: {2}", Profiles[SelectedProfileID].Name, Profiles[SelectedProfileID].MaxScore, Profiles[SelectedProfileID].Coins);
        PowerUpManager.MagnetImage.gameObject.SetActive(Profiles[SelectedProfileID].IsMagnetAvailable);
        PowerUpManager.PowerJumpImage.gameObject.SetActive(Profiles[SelectedProfileID].IsPowerJumpAvailable);
    }
}

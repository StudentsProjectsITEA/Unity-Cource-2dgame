using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject OptionsPanel, ShopPanel, TopTenPanel, EnterNamePanel, ProfilesPanel;
    public GameManager GameManager;

    private void Awake()
    {
        OptionsPanel.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(false);
        TopTenPanel.gameObject.SetActive(false);
        EnterNamePanel.gameObject.SetActive(false);
        ProfilesPanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void NewGame()
    {
        if (GameManager.IsGameOver)
        {
            GameManager.StartGame();
            this.gameObject.SetActive(false);
        }
        else
        {
            GameManager.StartCoroutine(GameManager.ContinueGame());
        }
    }

    public void ContinueGame()
    {
        this.gameObject.SetActive(false);
    }

    public void Shop()
    {
        ShopPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Profiles()
    {
        ProfilesPanel.SetActive(true);
        this.gameObject.SetActive(false);
        GameManager.SelectedProfileID = 20;
    }

    public void TopTen()
    {
        TopTenPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Options()
    {
        OptionsPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Return()
    {
        
        if (OptionsPanel.gameObject.activeSelf)
        {
            OptionsPanel.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        if (ShopPanel.gameObject.activeSelf)
        {
            ShopPanel.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        if (TopTenPanel.gameObject.activeSelf)
        {
            TopTenPanel.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        if (ProfilesPanel.gameObject.activeSelf && !EnterNamePanel.gameObject.activeSelf)
        {
            ProfilesPanel.gameObject.SetActive(false);
            this.gameObject.SetActive(true);
        }
        if (EnterNamePanel.gameObject.activeSelf)
        {
            EnterNamePanel.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        
    }

    public void OkButton()
    {
        if (EnterNamePanel.gameObject.activeSelf)
        {
            GameManager.SaveNewProfile();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{

    public GameManager GameManager;
    public Player Player;
    //public Magnet Magnet;
    public Image MagnetImage, PowerJumpImage;
    public Button MagnetButton, PowerJumpButton;
    public Text MagnetPriceText, PowerJumpPriceText;
    public float MagnetDuration, PowerJumpDuration;
    public float MagnetCooldown, PowerJumpCooldown;
    public int MagnetPrice, PowerJumpPrice;
    public float PowerJumpForce { get { return powerJumpForce; } }
    public bool IsMagnetAvailable { get { return isMagnetAvailable; } }
    public bool IsPowerJumpAvailable { get { return isPowerJumpAvailable; } }
    public bool IsMagnetWorking { get { return isMagnetWorking; } }
    public bool IsPowerJumpWorking { get { return isPowerJumpWorking; } }

    private CircleCollider2D Magnet;
    private float powerJumpForce;
    private bool isMagnetAvailable, isPowerJumpAvailable;
    private bool isMagnetWorking, isPowerJumpWorking;
    private bool isMagnetOnCooldown, isPowerJumpOnCooldown;
    private Coroutine magnetCoroutine, powerJumpCoroutine;
    private Coroutine magnetCooldownCoroutine, powerJumpCoolDownCoroutine;

    private void Awake()
    {
        Magnet = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        MagnetPriceText.text = MagnetPrice.ToString();
        PowerJumpPriceText.text = PowerJumpPrice.ToString();
        MagnetImage.gameObject.SetActive(false);
        PowerJumpImage.gameObject.SetActive(false);
        isPowerJumpAvailable = false;
        isMagnetAvailable = false;
        isMagnetWorking = false;
        isPowerJumpWorking = false;
        powerJumpForce = 1;
        Magnet.enabled = false;
        //Magnet.gameObject.SetActive(false);
    }

    public void NewGame(bool magnet, bool powerJump)
    {
        if (magnet)
        {
            MagnetButton.interactable = false;
            MagnetImage.gameObject.SetActive(true);
            MagnetImage.fillAmount = 1;
            isMagnetAvailable = true;
            isMagnetWorking = false;
        }
        else
        {
            MagnetButton.interactable = true;
            MagnetImage.gameObject.SetActive(false);
            MagnetImage.fillAmount = 1;
            isMagnetAvailable = false;
            isMagnetWorking = false;
        }

        if (powerJump)
        {
            PowerJumpButton.interactable = false;
            PowerJumpImage.gameObject.SetActive(true);
            PowerJumpImage.fillAmount = 1;
            isPowerJumpAvailable = true;
            isPowerJumpWorking = false;
        }
        else
        {
            PowerJumpButton.interactable = true;
            PowerJumpImage.gameObject.SetActive(false);
            PowerJumpImage.fillAmount = 1;
            isPowerJumpAvailable = false;
            isPowerJumpWorking = false;
        }
        
        //Magnet.gameObject.SetActive(false);
        Magnet.enabled = false;
        powerJumpForce = 1;
    }

    public void LoadPlayerData(bool magnet, bool powerJump)
    {
        isMagnetAvailable = magnet;
        isPowerJumpAvailable = powerJump;
    }

    public void BuyMagnet()
    {
        if (GameManager.CoinCount >= MagnetPrice)
        {
            isMagnetAvailable = true;
            MagnetImage.gameObject.SetActive(true);
            GameManager.CoinCount -= MagnetPrice;
            MagnetButton.interactable = false;
            GameManager.CoinsText.text = "Coins: " + GameManager.CoinCount;
        }
    }

    public void BuyPowerJump()
    {
        if (GameManager.CoinCount >= PowerJumpPrice)
        {
            isPowerJumpAvailable = true;
            PowerJumpImage.gameObject.SetActive(true);
            GameManager.CoinCount -= PowerJumpPrice;
            PowerJumpButton.interactable = false;
            GameManager.CoinsText.text = "Coins: " + GameManager.CoinCount;
        }
    }

    public IEnumerator PowerJumpStart(float duration)
    {
        isPowerJumpWorking = true;
        powerJumpForce = 1.5f;
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            PowerJumpImage.fillAmount = timeLeft / duration;
            yield return null;
        }
        Debug.Log("POWERJUMP OFF");
        powerJumpForce = 1;
        powerJumpCoolDownCoroutine = StartCoroutine(PowerJumpCooldownStart(PowerJumpCooldown));
    }

    public IEnumerator StartMagnet(float duration)
    {
        isMagnetWorking = true;
        //Magnet.gameObject.SetActive(true);
        Magnet.enabled = true;
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            MagnetImage.fillAmount = timeLeft / duration;
            yield return null;
        }
        Debug.Log("MAGNET OFF");
        Magnet.enabled = false;
        magnetCooldownCoroutine = StartCoroutine(MagnetCooldownStart(MagnetCooldown));
    }

    public IEnumerator MagnetCooldownStart(float cooldown)
    {
        isMagnetOnCooldown = true;
        float timeLeft = 0;
        while (timeLeft < cooldown)
        {
            timeLeft += Time.deltaTime;
            MagnetImage.fillAmount = timeLeft / cooldown;
            yield return null;
        }
        isMagnetOnCooldown = false;
        isMagnetWorking = false;
        Debug.Log("MAGNET AVAILABLE");
    }

    public IEnumerator PowerJumpCooldownStart(float cooldown)
    {
        isPowerJumpOnCooldown = true;
        float timeLeft = 0;
        while (timeLeft < cooldown)
        {
            timeLeft += Time.deltaTime;
            PowerJumpImage.fillAmount = timeLeft / cooldown;
            yield return null;
        }
        isPowerJumpWorking = false;
        isPowerJumpOnCooldown = false;
        Debug.Log("POWERJUMP AVAILABLE");
    }

    public void StartMagnetCoroutine(float duration)
    {
        magnetCoroutine = StartCoroutine(StartMagnet(duration));
    }

    public void StartPowerJumpCoroutine(float duration)
    {
        powerJumpCoroutine = StartCoroutine(PowerJumpStart(duration));
    }

    public void StopAllPowerUpCoroutines()
    {
        if (isMagnetWorking)
        {
            StopCoroutine(magnetCoroutine);
            if (isMagnetOnCooldown)
            {
                StopCoroutine(magnetCooldownCoroutine);
            }
            
        }
        if (isPowerJumpWorking)
        {
            StopCoroutine(powerJumpCoroutine);
            if (isPowerJumpOnCooldown)
            {
                StopCoroutine(powerJumpCoolDownCoroutine);

            }
        }

    }
}

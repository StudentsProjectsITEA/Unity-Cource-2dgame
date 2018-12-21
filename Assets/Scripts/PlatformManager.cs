using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    public Platform PlatformPrefab;
    public Coin CoinPrefab;
    public float MinHeight, MaxHeight;
    public float MinVertDistance, MaxVertDistance;
    public float HorizontalDistance;

    private Platform[] platformList;
    private Coin[] coins;
    private float verticalDistance = 0;

    private void Awake()
    {
        platformList = new Platform[5];
        coins = new Coin[5];
    }

    public void PlatformGeneration()
    {
        for (int i = 0; i < platformList.Length; i++)
        {
            verticalDistance = Random.Range(MinVertDistance, MaxVertDistance);
            Platform platform = Instantiate(PlatformPrefab, Vector3.zero, Quaternion.identity, transform);
            Coin coin = Instantiate(CoinPrefab, Vector3.zero, Quaternion.identity, transform);
            platformList[i] = platform;
            coins[i] = coin;
            if (i > 0)
            {
                if (platformList[i - 1].transform.position.y + verticalDistance > MaxHeight || platformList[i - 1].transform.position.y + verticalDistance < MinHeight)
                {
                    verticalDistance *= -1;
                }

                platform.transform.position = new Vector3(platformList[i - 1].transform.position.x + HorizontalDistance, platformList[i - 1].transform.position.y + verticalDistance);
            }
            else
                platform.transform.position = new Vector3(-6f, -3f, 0f);
            coins[i].transform.position = platform.transform.position + Vector3.up * 2;
        }
    }

    public void platformSwitch()
    {
        verticalDistance = Random.Range(MinVertDistance, MaxVertDistance);

        Platform temp = platformList[0];
        for (int i = 1; i < platformList.Length; i++)
        {
            platformList[i - 1] = platformList[i];
        }
        platformList[platformList.Length - 1] = temp;
        if (platformList[platformList.Length - 2].transform.position.y + verticalDistance > MaxHeight ||
            platformList[platformList.Length - 2].transform.position.y + verticalDistance < MinHeight)
        {
            verticalDistance *= -1;
        }
        platformList[platformList.Length - 1].transform.position =
            new Vector3(platformList[platformList.Length - 2].transform.position.x + HorizontalDistance,
            platformList[platformList.Length - 2].transform.position.y + verticalDistance);
        CoinShift();
    }

    public void CoinShift()
    {
        Coin temp = coins[0];
        for (int i = 1; i < coins.Length; i++)
        {
            coins[i - 1] = coins[i];
        }
        coins[coins.Length - 1] = temp;
        coins[coins.Length - 1].transform.position = platformList[platformList.Length - 1].transform.position + Vector3.up * 2;
        coins[coins.Length - 1].gameObject.SetActive(true);
    }

    public void Clear()
    {
        for (int i = 0; i < platformList.Length; i++)
        {
            Destroy(platformList[i].gameObject);
            Destroy(coins[i].gameObject);
        }
    }
}

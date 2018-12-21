using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting : MonoBehaviour
{
    [Range(2, 20)]
    public int objectAmount;
    public float speed;
    public GameObject objectPrefab;
    private GameObject[] objectList;

    private Vector3 temp1, temp2;
    private bool isSorting;

    void Start()
    {
        objectList = new GameObject[objectAmount];
        for (int i = 0; i < objectAmount; i++)
        {
            Vector3 initialPosition = new Vector3(-objectAmount / 2 + i + 0.5f, 0, 0);
            Vector2 initialScale = new Vector2(1f, Random.Range(1, 5));
            GameObject newObject = Instantiate(objectPrefab, initialPosition, Quaternion.identity);
            newObject.transform.localScale = initialScale;
            objectList[i] = newObject;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isSorting)
        {
            StartCoroutine(SortIncrease());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isSorting)
        {
            StartCoroutine(SortDecrease());
        }
    }

    IEnumerator SortIncrease()
    {
        Vector3 temp1 = new Vector3();
        Vector3 temp2 = new Vector3();
        GameObject tempObject;

        for (int i = 0; i < objectAmount; i++)
        {
            for (int j = 0; j < objectAmount - 1; j++)
            {
                if (objectList[j].transform.localScale.y > objectList[j + 1].transform.localScale.y)
                {
                    objectList[j + 1].GetComponent<SpriteRenderer>().sortingOrder = 2;
                    objectList[j].GetComponent<SpriteRenderer>().sortingOrder = 1;

                    tempObject = objectList[j + 1];
                    objectList[j + 1] = objectList[j];
                    objectList[j] = tempObject;
                    
                    temp1 = objectList[j + 1].transform.position;
                    temp2 = objectList[j].transform.position;

                    StartCoroutine(MoveObject(j, temp1));
                    StartCoroutine(MoveObject(j + 1, temp2));
                    yield return new WaitUntil(() => !isSorting);
                }
            }
        }
    }

    IEnumerator SortDecrease()
    {
        Vector3 temp1 = new Vector3();
        Vector3 temp2 = new Vector3();
        GameObject tempObject;
        

        for (int i = 0; i < objectAmount; i++)
        {
            for (int j = 0; j < objectAmount - 1; j++)
            {
                if (objectList[j].transform.localScale.y < objectList[j + 1].transform.localScale.y)
                {
                    objectList[j].GetComponent<SpriteRenderer>().sortingOrder = 2;
                    objectList[j + 1].GetComponent<SpriteRenderer>().sortingOrder = 1;

                    tempObject = objectList[j + 1];
                    objectList[j + 1] = objectList[j];
                    objectList[j] = tempObject;

                    temp1 = objectList[j + 1].transform.position;
                    temp2 = objectList[j].transform.position;

                    StartCoroutine(MoveObject(j, temp1));
                    StartCoroutine(MoveObject(j + 1, temp2));
                    yield return new WaitUntil(() => !isSorting);
                }
            }
        }
    }

    IEnumerator MoveObject(int i, Vector3 target)
    {
        isSorting = true;
        while (!objectList[i].transform.position.Equals(target))
        {
            objectList[i].transform.position = Vector3.MoveTowards(objectList[i].transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        isSorting = false;
    }
}

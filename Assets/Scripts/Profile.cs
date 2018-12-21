using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Profile : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameManager GameManager;

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("Deselect");
        StartCoroutine(DeselectTimer());
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.SelectedProfileID = this.gameObject.transform.GetSiblingIndex();
        GameManager.InitializeUI();
    }

    //delay for button click
    IEnumerator DeselectTimer()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (GameManager.SelectedProfileID < 10 && GameManager.SelectedProfileID == this.gameObject.transform.GetSiblingIndex())
        {
            GameManager.SelectedProfileID = 20;
        }
    }
}

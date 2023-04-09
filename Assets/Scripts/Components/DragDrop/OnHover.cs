using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject objectToDisplay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        objectToDisplay.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        objectToDisplay.SetActive(false);
    }
}
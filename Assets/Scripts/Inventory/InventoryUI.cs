using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Variables
    // List of slots used to anchor invItems
    [SerializeField] private List<GameObject> invSlotsUI;
    // These contain InventoryItem component, change displayed sprite on raw image component
    [SerializeField] private List<GameObject> invItemsUI;
    // Change displayed item count on text component
    [SerializeField] private List<GameObject> invItemCountersUI;
    [SerializeField] private GameObject invItemDiscardUI;

    [SerializeField] private List<GameObject> invHandSlotsUI;
    [SerializeField] private List<GameObject> invHandItemsUI;
    [SerializeField] private List<GameObject> invHandItemCountersUI;
    [SerializeField] private GameObject survivalUI;

    [SerializeField] private List<GameObject> invHandSelectedSlotUI;

    #endregion Variables

    #region GetSet
    public List<GameObject> GetInvSlotsUI()
    {
        return invSlotsUI;
    }

    public List<GameObject> GetInvItemsUI()
    {
        return invItemsUI;
    }

    public List<GameObject> GetInvItemCountersUI()
    {
        return invItemCountersUI;
    }

    public List<GameObject> GetInvHandSlotsUI()
    {
        return invHandSlotsUI;
    }

    public List<GameObject> GetInvHandItemsUI()
    {
        return invHandItemsUI;
    }

    public List<GameObject> GetInvHandItemCountersUI()
    {
        return invHandItemCountersUI;
    }

    public GameObject GetInvItemDiscardUI()
    {
        return invItemDiscardUI;
    }

    public SurvivalUI GetSurvivalUI()
    {
        return survivalUI.GetComponent<SurvivalUI>();
    }

    public List<GameObject> GetInvHandSelectedSlotUI()
    {
        return invHandSelectedSlotUI;
    }

    #endregion GetSet
}
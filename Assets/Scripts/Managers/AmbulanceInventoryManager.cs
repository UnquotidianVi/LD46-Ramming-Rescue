using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceInventoryManager : MonoBehaviour
{
    [SerializeField]
    private List<ItemHolder> itemHolders = new List<ItemHolder>();

    public void WipeInventory()
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].MyItem = null;
            itemHolders[i].UpdateVisuals();
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            if(itemHolders[i].MyItem == null)
            {
                itemHolders[i].MyItem = itemToAdd;
                itemHolders[i].UpdateVisuals();
                return true;
            }  
        }

        //If we got this far, that means there is no free space
        return false;
    }
}

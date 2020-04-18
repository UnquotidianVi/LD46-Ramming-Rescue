using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public Item MyItem { get { return myItem; } set { myItem = value; } }
    [SerializeField]
    private Item myItem;
    [SerializeField]
    private SpriteRenderer myItemSpriteRenderer;
    [SerializeField]
    private GameObject itemInfoUI;
    [SerializeField]
    private Text itemInfoTitleText;
    [SerializeField]
    private Text itemInfoDescriptionText;
    [SerializeField]
    private Text itemInfoEffectsText;
    [SerializeField]
    private Image itemInfoImage;
    [SerializeField]
    private Vector2 itemInfoUIOffset;
    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Button disposeButton;
    [SerializeField]
    private PatientBehaviour patientBehaviour;
    [SerializeField]
    private HoweringText paramedicHowerText;

    private bool openedItemInfoUI;

    public void UpdateVisuals()
    {
        if (myItem != null)
        {
            if (myItem.sprite != null)
                myItemSpriteRenderer.sprite = myItem.sprite;
            else
                myItemSpriteRenderer.sprite = null;
        }
        else
        {
            myItemSpriteRenderer.sprite = null;
        }
    }

    private void OnMouseOver()
    {
        if (myItem == null)
            return;
        itemInfoUI.SetActive(true);
        if(openedItemInfoUI != true)
        {
            itemInfoUI.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + itemInfoUIOffset.x, transform.position.y + itemInfoUIOffset.y));
            UpdateItemInfoUI();
        }
        openedItemInfoUI = true;
    }

    private void OnMouseExit()
    {
        if (myItem == null)
            return;
        itemInfoUI.SetActive(false);
        openedItemInfoUI = false;
    }

    private void UpdateItemInfoUI()
    {
        if (myItem == null)
            return;
        itemInfoTitleText.text = myItem.name;
        itemInfoDescriptionText.text = myItem.description;
        itemInfoImage.sprite = myItem.sprite;
        itemInfoEffectsText.text = myItem.effectText;
        useButton.onClick.RemoveAllListeners();
        disposeButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(OnUseButtonPressed);
        disposeButton.onClick.AddListener(OnDisposeButtonPressed);
    }

    public void OnDisposeButtonPressed()
    {
        paramedicHowerText.ShowText("Threw item away... " + myItem.name);
        OnMouseExit();
        myItem = null;
    }

    public void OnUseButtonPressed()
    {
        if (myItem.consumableItem)
        {
            patientBehaviour.BloodLoss += myItem.effectOnBloofLoss;
            patientBehaviour.Blood += myItem.effectOnBlood;
            patientBehaviour.Awaraness += myItem.effectOnAwareneess;

            paramedicHowerText.ShowText("Used item on patient... " + myItem.name);

            OnMouseExit();
            myItem = null;
        }
        else if (myItem.isExplosive)
        {
            Debug.Log("Boom");
        }
    }
}

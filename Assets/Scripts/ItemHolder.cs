using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class ItemHolder : MonoBehaviour
{
    public Item MyItem { get { return myItem; } set { myItem = value; } }

    [SerializeField]
    private Item myItem;
    [SerializeField]
    private Vector2 itemInfoUIOffset;
    [SerializeField]
    private float playerUseRange;
    [SerializeField]
    private Sound explosionCountdownSound;
    [SerializeField]
    private Sound explosionSound;
    [SerializeField]
    private Light2D itemLight;
    [SerializeField]
    private Sound itemDisposeSound;
    [SerializeField]
    private Sound itemUseSound;
    
    [Header("Script References")]
    [SerializeField]
    private PatientBehaviour patientBehaviour;
    [SerializeField]
    private HoweringText paramedicHowerText;
    [SerializeField]
    private SpriteRenderer myItemSpriteRenderer;
    [SerializeField]
    private ParamedicController paramedicController;
    [SerializeField]
    private AmbulanceController ambulanceController;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private GameManager gameManager;
    [Header("UI References")]
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
    private Button useButton;
    [SerializeField]
    private Button disposeButton;

    private bool openedItemInfoUI;
    private float explosionTime = 9999;
    private float explosionBleepFrequency;
    private float lastTimeBleeped;

    private void Update()
    {
        if (myItem == null)
            return;
        if (myItem.isExplosive)
        {
            if (explosionTime == 9999)
                explosionTime = myItem.explosionTime;
            ExplosiveCountDown();
        }
    }

    public void UpdateVisuals()
    {
        if (myItem != null)
        {
            itemLight.intensity = 0.1f;
            if (myItem.sprite != null)
                myItemSpriteRenderer.sprite = myItem.sprite;
            else
                myItemSpriteRenderer.sprite = null;
        }
        else
        {
            itemLight.intensity = 0;
            myItemSpriteRenderer.sprite = null;
        }
    }

    private void OnMouseOver()
    {
        if (myItem == null)
            return;
        if(Vector2.Distance(transform.position, paramedicController.transform.position) < playerUseRange)
        {
            itemInfoUI.SetActive(true);
            if (openedItemInfoUI != true)
            {
                itemInfoUI.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + itemInfoUIOffset.x, transform.position.y + itemInfoUIOffset.y));
                UpdateItemInfoUI();
            }
            openedItemInfoUI = true;
        }
        else
        {
            OnMouseExit();
        }
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
        soundManager.PlaySound(itemDisposeSound);
        paramedicHowerText.ShowText("Threw item away... " + myItem.name);
        OnMouseExit();
        myItem = null;
        UpdateVisuals();
        explosionTime = 9999;
    }

    public void OnUseButtonPressed()
    {
        soundManager.PlaySound(itemUseSound);
        if (myItem.consumableItem)
        {
            patientBehaviour.BloodLoss += myItem.effectOnBloofLoss;
            patientBehaviour.Blood += myItem.effectOnBlood;
            patientBehaviour.Awaraness += myItem.effectOnAwareneess;

            paramedicHowerText.ShowText("Used item on patient... " + myItem.name);

            OnMouseExit();
            myItem = null;
            UpdateVisuals();
        }
        else if (myItem.isExplosive)
        {
            TriggerExplosive();
            gameManager.DeathUIDescriptionText.text = "... did you just? No you did not... DON'T USE THE HAND GRENADE! THROW IT AWAY!!!!";
        }
    }

    private void TriggerExplosive()
    {
        OnDisposeButtonPressed();
        gameManager.ChangeGameState(GameManager.GameState.Death);
        soundManager.PlaySound(explosionSound);
        ambulanceController.BlowUp();
    }

    private void ExplosiveCountDown()
    {
        explosionTime -= Time.deltaTime;

        if(explosionTime > 4)
        {
            explosionBleepFrequency = 1;
        }
        else if(explosionTime < 3 && explosionTime > 1.5f)
        {
            explosionBleepFrequency = 0.5f;
        }
        else if(explosionTime < 1f)
        {
            explosionBleepFrequency = 0.3f;
        }

        if(lastTimeBleeped + explosionBleepFrequency <= Time.time)
        {
            soundManager.PlaySound(explosionCountdownSound);
            lastTimeBleeped = Time.time;
        }

        if(explosionTime <= 0)
        {
            gameManager.DeathUIDescriptionText.text = "... and so did you. Try to dispose of those grenades next time... Or even better: DON'T PICK THEM UP!";
            TriggerExplosive();
        }
    }
}

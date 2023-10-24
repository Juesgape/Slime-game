using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConvertingItemsInfo : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI title;
    public Button buyButton;
    public Button equipButton;

    public Skin_Controller skinControllerScript;

    int price;
    int totalMoney;

    void Start()
    {
        price = int.Parse(priceText.text);
        equipButton.gameObject.SetActive(false);

        skinControllerScript = GetComponent<Skin_Controller>();
        
    }

    // Update is called once per frame
    void Update()
    {
        totalMoney = PlayerPrefs.GetInt("totalMoney");
        if(price > totalMoney)
        {
            buyButton.interactable = false;
        }
    }

    //Onclick function that buys items whenever the user clicks the buy item button
    public void BuyItem(string itemName)
    {  
        totalMoney -= price;
        PlayerPrefs.SetInt("totalMoney", totalMoney);

        PlayerPrefs.SetString(itemName, itemName);

        //Hiding the buy button cuz we already buyed the item
        buyButton.gameObject.SetActive(false);

        //Making equipButton visible and adding it an eventListener
        equipButton.gameObject.SetActive(true);
    }

    public void ShowEquippedSkin(Grid_item_info itemInfo)
    {
        //Hiding the buy button cuz we already buyed the item
        buyButton.gameObject.SetActive(false);

        //Making equipButton visible and adding it an eventListener
        equipButton.gameObject.SetActive(true);

        //Equip item
        itemInfo.equipped = true;

        equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped :)";
        equipButton.interactable = false;
    }

    public void EquipSkin(Grid_item_info itemInfo, List<Grid_item_info> itemsInformation)
    {

        //Debug.Log("EquipSkin called for item: " + itemInfo.title);

        //delete equipped to the rest of the items
        Grid_item_info[] allItems = FindObjectsOfType<Grid_item_info>();
        foreach (Grid_item_info item in allItems)
        {
            if (item != itemInfo)
            {
                item.equipped = false;
            }
        }

        foreach (var item in itemsInformation)
        {
            if (item != itemInfo)
            {
                // Encuentra el ConvertingItemsInfo correspondiente al item anteriormente equipado
                ConvertingItemsInfo previousItemGridItem = item.convertingItemsInfo;
                if (previousItemGridItem != null)
                {
                    previousItemGridItem.equipButton.interactable = true;
                    previousItemGridItem.equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
                }
            }
        }

        //Equip item
        itemInfo.equipped = true;

        equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped :)";
        equipButton.interactable = false;

        PlayerPrefs.SetString("currentSkin", itemInfo.title.ToLower().Replace(" ", "_"));
    }
}
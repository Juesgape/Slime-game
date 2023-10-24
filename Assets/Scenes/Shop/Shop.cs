using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    //Grid_item_info contains the information of each item such as title, price, etc.
    [SerializeField] List<Grid_item_info> itemsInformation;
    [SerializeField] GameObject prefabItemShop;
    [SerializeField] TextMeshProUGUI totalMoney;

    void Start()
    {
        //In case the player does not have any money grant him with 900
        if (!PlayerPrefs.HasKey("totalMoney"))
        {
            PlayerPrefs.SetInt("totalMoney", 900);
        } else
        {
            PlayerPrefs.SetInt("totalMoney", 900);
        }


        foreach (var item in itemsInformation)
        {
            var gridItem = Instantiate(prefabItemShop, transform).GetComponent<ConvertingItemsInfo>();
            item.convertingItemsInfo = gridItem;
            
            gridItem.image.sprite = item.image;
            gridItem.title.text = item.title;
            gridItem.priceText.text = item.price.ToString();

            //We save the skin names differently to how we show in the shop interface
            string titleConverted = item.title.ToLower().Replace(" ", "_");

            gridItem.buyButton.onClick.AddListener(() => gridItem.BuyItem(titleConverted));
            gridItem.equipButton.onClick.AddListener(() => gridItem.EquipSkin(item, itemsInformation));


            //If PlayerPrefs has the key of the titleConverted, it means the user has already bought the item
            /* if (PlayerPrefs.HasKey(titleConverted))
            {
                Debug.Log($"We have bought the {titleConverted} item");
                //gridItem.buyButton.gameObject.SetActive(false);
                //gridItem.equipButton.gameObject.SetActive(true);

                gridItem.equipButton.onClick.AddListener(() => gridItem.EquipSkin(item, itemsInformation));
                //gridItem.ShowPurchasedItem();
            } */

            //If playerPrefs "currentSkin" is equal the titleConverted, it means that the user has already equipped the skin

            /* if(PlayerPrefs.HasKey("currentSkin"))
            {
                if(PlayerPrefs.GetString("currentSkin") == titleConverted)
                {
                    Debug.Log($"We have equipped the {titleConverted} item");
                    gridItem.ShowEquippedSkin(item);
                    continue;
                }
            } else
            {
                
            } */

        }
    }

    void Update()
    {
        totalMoney.text = PlayerPrefs.GetInt("totalMoney").ToString();
    }
}

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
        }

        var gridItem = prefabItemShop.GetComponent<ConvertingItemsInfo>();

        foreach (var item in itemsInformation)
        {
            gridItem.image.sprite = item.image;
            gridItem.title.text = item.title;
            gridItem.priceText.text = item.price.ToString();

            Instantiate(gridItem, transform);

        }
    }

    void Update()
    {
        totalMoney.text = PlayerPrefs.GetInt("totalMoney").ToString();
    }
}

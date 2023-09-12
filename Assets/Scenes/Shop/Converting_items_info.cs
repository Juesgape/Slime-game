 using System.Collections;
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
    int price;
    int totalMoney;
    // Start is called before the first frame update
    void Start()
    {
        price = int.Parse(priceText.text);
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

    public void BuyItem()
    {
        totalMoney -= price;
        PlayerPrefs.SetInt("totalMoney", totalMoney);
    }
}

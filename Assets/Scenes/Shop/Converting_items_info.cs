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

    int price;
    int totalMoney;

    //What is the current skin?
    private enum CurrentSkin
    {
        defaultSkin,
        redSkin,
        blueSkin
    }

    void Start()
    {
        price = int.Parse(priceText.text);
        equipButton.gameObject.SetActive(false);
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
    public void BuyItem()
    {
        totalMoney -= price;
        PlayerPrefs.SetInt("totalMoney", totalMoney);

        //Hiding the buy button cuz we already buyed the item
        buyButton.gameObject.SetActive(false);

        //Making equipButton visible and adding it an eventListener
        equipButton.gameObject.SetActive(true);
        equipButton.onClick.AddListener(EquipSkin);
    }

    public void EquipSkin()
    {
        equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped :)";
        equipButton.interactable = false;

        Debug.Log("Your new skin is ", title);
    }
}

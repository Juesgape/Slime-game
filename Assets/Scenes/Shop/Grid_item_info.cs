using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "ItemInfo")]
public class Grid_item_info : ScriptableObject
{
    public string title;
    public Sprite image;
    public int price;
    public bool equipped = false;
    public bool buyed = false;
    public ConvertingItemsInfo convertingItemsInfo;
    public GameObject prefab;
}

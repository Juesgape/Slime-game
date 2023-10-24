using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin_Controller : MonoBehaviour
{
    [SerializeField] List<Grid_item_info> itemsInformation;
    
    private Dictionary<string, GameObject> skinPrefabs = new Dictionary<string, GameObject>();
    public GameObject defaultSkin;

    private GameObject player;
    private string currentSkinName;

    void Start()
    {
        if(PlayerPrefs.HasKey("currentSkin")) {
            currentSkinName = PlayerPrefs.GetString("currentSkin");
        }

        //find the current player Object
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        //Inmediately change skin
        ChangeSkin();
    }

    public void ChangeSkin()
    {
        if(currentSkinName == null)
        {
            return;
        }

        //Add the skin if it is not in the list
        
        foreach (var item in itemsInformation)
        {
            string transformedText = item.name.ToLower().Replace(" ", "_");

            if(transformedText == currentSkinName)
            {
                skinPrefabs[currentSkinName] = item.prefab;
                break;
            }
        }


        // Instantiate the selected skin prefab
        GameObject selectedSkinPrefab = skinPrefabs[currentSkinName];
        GameObject newPlayer = Instantiate(selectedSkinPrefab, player.transform.position, player.transform.rotation);

        // Destroy the current player object
        Destroy(player);

        // Update the reference to the player object
        player = newPlayer;
    }
}

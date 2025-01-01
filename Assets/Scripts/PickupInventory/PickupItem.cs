using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Info")]
    public int itemPrice;
    public int itemRadius;
    public string itemTag;
    private GameObject ItemToPick;

    [Header("Player Info")]
    public Player player;

    private void Start()
    {
        ItemToPick = GameObject.FindWithTag(itemTag);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < itemRadius)
        {
            if (Input.GetKeyDown("f"))
            {
                if(itemPrice > player.playerMoney)
                {
                    Debug.Log("You are broke");
                    //Show UI
                }

                else
                {
                    if(itemTag == "HandGunPickup")
                    {
                        player.playerMoney -= itemPrice;

                        Debug.Log(itemTag);
                    }
                    else if (itemTag == "ShotGunPickup")
                    {
                        player.playerMoney -= itemPrice;

                        Debug.Log(itemTag);
                    }
                    else if (itemTag == "UziPickup")
                    {
                        player.playerMoney -= itemPrice;

                        Debug.Log(itemTag);
                    }
                    else if (itemTag == "BazookaPickup")
                    {
                        player.playerMoney -= itemPrice;

                        Debug.Log(itemTag);
                    }
                    ItemToPick.SetActive(false);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Item Slots")]
    public GameObject Weapon1;
    public bool isWeapon1Picked = false;
    public bool isWeapon1Active = false;

    public GameObject Weapon2;
    public bool isWeapon2Picked = false;
    public bool isWeapon2Active = false;

    public GameObject Weapon3;
    public bool isWeapon3Picked = false;
    public bool isWeapon3Active = false;

    public GameObject Weapon4;
    public bool isWeapon4Picked = false;
    public bool isWeapon4Active = false;

    [Header("Weapons to Use")]
    public GameObject HandGun1;
    public GameObject HandGun2;
    public GameObject Shotgun;
    public GameObject UZI;
    public GameObject UZI2;
    public GameObject Bazooka;

    [Header("Scripts")]
    public PlayerScript playerScript;
    public Shotgun shotgunScript;
    public Handgun handgunScript;
    public Handgun2 handgun2Script;
    public Uzi uziScript;
    public Uzi2 uzi2Script;
    public Bazooka bazooka;

    [Header("Inventory")]
    public GameObject inventoryPanel;
    bool isPause = false;

    private void Update()
    {
        if (Input.GetKeyDown("1") && isWeapon1Picked == true)
        {
            isWeapon1Active = true;
            isWeapon2Active = false;
            isWeapon3Active = false;
            isWeapon4Active = false;
            isRiflesActive();
        }
        else if(Input.GetKeyDown("2") && isWeapon2Picked == true)
        {
            isWeapon1Active = false;
            isWeapon2Active = true;
            isWeapon3Active = false;
            isWeapon4Active = false;
            isRiflesActive();
        }
        else if (Input.GetKeyDown("3") && isWeapon3Picked == true)
        {
            isWeapon1Active = false;
            isWeapon2Active = false;
            isWeapon3Active = true;
            isWeapon4Active = false;
            isRiflesActive();
        }
        else if (Input.GetKeyDown("4") && isWeapon4Picked == true)
        {
            isWeapon1Active = false;
            isWeapon2Active = false;
            isWeapon3Active = false;
            isWeapon4Active = true;
            isRiflesActive();
        }
    }

    void isRiflesActive()
    {
        if (isWeapon1Active == true) 
        {
            HandGun1.SetActive(true);
            HandGun2.SetActive(true);
            Shotgun.SetActive(false);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(false);

            playerScript.GetComponent<PlayerScript>().enabled = false;
            handgunScript.GetComponent<Handgun>().enabled = true;
            handgun2Script.GetComponent<Handgun2>().enabled = true;
            shotgunScript.GetComponent<Shotgun>().enabled = false;
            uziScript.GetComponent<Uzi>().enabled = false;
            uzi2Script.GetComponent <Uzi2>().enabled = false;
            bazooka.GetComponent<Bazooka>().enabled = false;
        }
        else if (isWeapon2Active == true)
        {
            HandGun1.SetActive(false);
            HandGun2.SetActive(false);
            Shotgun.SetActive(true);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(false);

            playerScript.GetComponent<PlayerScript>().enabled = false;
            handgunScript.GetComponent<Handgun>().enabled = false;
            handgun2Script.GetComponent<Handgun2>().enabled = false;
            shotgunScript.GetComponent<Shotgun>().enabled = true;
            uziScript.GetComponent<Uzi>().enabled = false;
            uzi2Script.GetComponent<Uzi2>().enabled = false;
            bazooka.GetComponent<Bazooka>().enabled = false;
        }
        else if (isWeapon3Active == true)
        {
            HandGun1.SetActive(false);
            HandGun2.SetActive(false);
            Shotgun.SetActive(false);
            UZI.SetActive(true);
            UZI2.SetActive(true);
            Bazooka.SetActive(false);

            playerScript.GetComponent<PlayerScript>().enabled = false;
            handgunScript.GetComponent<Handgun>().enabled = false;
            handgun2Script.GetComponent<Handgun2>().enabled = false;
            shotgunScript.GetComponent<Shotgun>().enabled = false;
            uziScript.GetComponent<Uzi>().enabled = true;
            uzi2Script.GetComponent<Uzi2>().enabled = true;
            bazooka.GetComponent<Bazooka>().enabled = false;
        }
        else if (isWeapon4Active == true)
        {
            HandGun1.SetActive(false);
            HandGun2.SetActive(false);
            Shotgun.SetActive(false);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(true);

            playerScript.GetComponent<PlayerScript>().enabled = false;
            handgunScript.GetComponent<Handgun>().enabled = false;
            handgun2Script.GetComponent<Handgun2>().enabled = false;
            shotgunScript.GetComponent<Shotgun>().enabled = false;
            uziScript.GetComponent<Uzi>().enabled = false;
            uzi2Script.GetComponent<Uzi2>().enabled = false;
            bazooka.GetComponent<Bazooka>().enabled = true;
        }
    }
}

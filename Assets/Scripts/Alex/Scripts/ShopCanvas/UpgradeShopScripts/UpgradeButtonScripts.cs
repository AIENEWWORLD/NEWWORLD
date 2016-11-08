using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;



public class UpgradeButtonScripts : MonoBehaviour
{
    public Canvas myCanvas;
    public StatsScript playerStats;
    public GameObject[] ButtonList; // I hate these things so much
    public List<GameObject> bList = new List<GameObject>();
    public GameObject currButton;
    public TriggerShop TrigShop;

    public bool chkgld = false;

    public AudioClip PurchaseSound;
    public AudioSource WhereToPlay;

    private int Compare(GameObject _x, GameObject _y)
    {
        ButtonShopScript x = _x.GetComponent<ButtonShopScript>();
        ButtonShopScript y = _y.GetComponent<ButtonShopScript>();
        if (x.number == y.number)
        {
            return 0;
        }
        if (x.number > y.number)
        {
            return 1;
        }
        else return -1;
    }

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>();
        ButtonList = GameObject.FindGameObjectsWithTag("ShopButton");




        for (int i = ButtonList.Length-1; i >= 0; i--)
        {
            bList.Add(ButtonList[i]);
        }

        for (int i = 0; i < bList.Count; i++)
        {
            for (int x = 0; x < bList.Count; x++)
            {
                bList.Sort(Compare);
            }

        }
        


        for (int i = 0; i < ButtonList.Length; i++)
        {
            ButtonShopScript BSS = ButtonList[i].GetComponent<ButtonShopScript>();
            BSS.mainScript = this;
            BSS.gameObject.GetComponentInChildren<Text>().text = BSS.cost.ToString();
        }
        currButton = bList[0];
        _checkGold();
        myCanvas.enabled = false;
    }
	
	void Update ()
    {
        if (chkgld)
        {
            _checkGold();
            chkgld = false;
        }

    }
    public void UpgradeTotalCoins(int cost)
    {
        WhereToPlay.PlayOneShot(PurchaseSound);
        playerStats.gold -= cost;
        playerStats.totalCoins += 1;
    }
    public void ExitShop2()
    {
        TrigShop.resetStuff();
        Camera.main.orthographic = false;
        Camera.main.nearClipPlane = 0.3f;
        myCanvas.enabled = false;
    }
    public void _checkGold()
    {

        for (int i = 0; i < bList.Count; i++)
        {
            ButtonShopScript Bss = bList[i].GetComponent<ButtonShopScript>();
            if (i == 0 && Bss.purchased == false)
            {
                Bss.active = true;
            }
            if (Bss.cost <= playerStats.gold && currButton.GetComponent<ButtonShopScript>().purchased == true)
            {
                //Bss.active = true;
                if (Bss.purchased)
                {
                    Bss.active = false;
                }
                if (i + 1 < bList.Count)
                {
                    currButton.GetComponent<ButtonShopScript>().active = false;

                    currButton = bList[i + 1];

                    bList[i + 1].GetComponent<ButtonShopScript>().active = true;
                    //Debug.Log("nButton " + (i + 1));
                }
            }
            if (currButton == bList[i])
            {
                Bss.myButton.interactable = true;
                Bss.gameObject.GetComponentInChildren<Text>().text = "costs " + Bss.cost + " gold to buy stage " + (Bss.number+1);
            }
            else
            {
                Bss.myButton.interactable = false;
                Bss.gameObject.GetComponentInChildren<Text>().text = "Purchased";
            }
            if (Bss.cost >= playerStats.gold)
            {
                Bss.myButton.interactable = false;
                Bss.gameObject.GetComponentInChildren<Text>().text = "Can't afford " + Bss.cost + " gold";
            }
            if (i == 3 && Bss.active == false && Bss.purchased == true)
            {
                Bss.myButton.interactable = false;
            }
        }
    }
}
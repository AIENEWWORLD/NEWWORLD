﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class coinScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    Image itemImage;
    public int itemNumber;
    CreateInventory inv;

    GameObject mouseover;

    Text mouseoverTextDesc1;
    Text mouseoverTextDesc2;
    // Use this for initialization
    void Start ()
    {
        inv = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<CreateInventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (inv.coinList[itemNumber].itemName != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = inv.coinList[itemNumber].Icon;
        }
        else
        {
            itemImage.enabled = false;
        }
	}

    public void OnPointerDown(PointerEventData data)
    {
        if (coin.isSelected == true)
        {
            coin.isSelected = false;
        }
        else
        {
            GameObject fCam = GameObject.FindGameObjectWithTag("FightCamera");
            if (fCam != null)
            {
                int totalselectedcoins = 0;
                for (int i = 0; i < fCam.GetComponent<AddItem>().coins.Count; i++)
                {
                    if (fCam.GetComponent<AddItem>().coins[i].isSelected == true)
                    {
                        totalselectedcoins++;
                    }
                }
                if (totalselectedcoins < fCam.GetComponent<StatsScript>().totalCoins)
                {
                    coin.isSelected = !coin.isSelected;
                }
            }
        }
        inv.greyoutUnselected();
        //if the amount of selected coins is smaller than the playerstats upgrade amount
    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseover = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().mouseover;
        if (coin.itemName.CompareTo("") != 0)
        {
            mouseover.SetActive(true);
            mouseoverTextDesc1 = GameObject.FindGameObjectWithTag("Desc1").GetComponent<Text>();
            mouseoverTextDesc2 = GameObject.FindGameObjectWithTag("Desc2").GetComponent<Text>();

            //Debug.Log(coin.itemName);
            mouseoverTextDesc1.text = coin.itemName + "\n" + coin.itemDescription;
            mouseoverTextDesc2.text = coin.itemDescription2;
        }
        //name
        //description
        //description 2
    }
    public void OnPointerExit(PointerEventData data)
    {
        mouseover.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ButtonShopScript : MonoBehaviour, IPointerUpHandler
{
    public int number;
    public bool active = true;
    public bool purchased = false;
    public int cost;
    public UpgradeButtonScripts mainScript;
    public Button myButton;

    public void OnPointerUp(PointerEventData eventData)
    {
        mainScript._checkGold();
        if (active && !purchased)
        {
            if(cost <= mainScript.playerStats.gold)
            mainScript.UpgradeTotalCoins(cost);
            purchased = true;
            mainScript._checkGold();
        }
    }

    void Start ()
    {
        myButton = gameObject.GetComponent<Button>();
        gameObject.name = number.ToString();
    }
	

	void Update ()
    {
	    
	}
}
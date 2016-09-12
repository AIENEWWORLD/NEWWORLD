using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CoinStats
{
    public string itemName;
    public string itemDescription;
    public string itemDescription2;
    public Sprite Icon;
    public int itemID;
    //
    public int Heads_attack;
    public int Heads_defence;
    public int Heads_HP; //+ or -.
    //
    public int Tails_attack;
    public int Tails_defence;
    public int Tails_HP; // + or -.

    public bool isHeads;

    public bool isSelected = false;

    public List<StatsScript.enumType> en = new List<StatsScript.enumType>();//only for dropcoins.


    void Start ()
    {
	
	}

    public CoinStats(string _itemName, string _itemDescription, string _itemDescription2, int _itemID, int H_attack, int H_defence, int H_HP, int T_attack, int T_defence, int T_HP)
    {
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemDescription2 = _itemDescription2;
        Icon = Resources.Load<Sprite>(""+itemName);
        itemID = _itemID;
        //
        Heads_attack = H_attack;
        Heads_defence = H_defence;
        Heads_HP = H_HP;
        //
        Tails_attack = T_attack;
        Tails_defence = T_defence;
        Tails_HP = T_HP;
    }

    public CoinStats()
    {
    }

   void Update ()
    {
	
	}
}


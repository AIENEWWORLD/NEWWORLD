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
    public bool activeonHeads;

    public bool isPlayerCoin;

    public bool isFlipCoin;

    public bool isHeads;


    public bool isSelected = false;

    public List<StatsScript.enumType> en = new List<StatsScript.enumType>();//only for dropcoins.

    public int cost = 0;//only for the shop coins that you can buy

    public coinTypes cType;

    public EnemycoinTypes ETypes;

    public enum coinTypes
    {
        none,
        standard,
        flip,
        counter,
        secondChance,
        secondChance_2,
        Double,
        ShieldBash,
        Mother,
        Father,
        Child,
    }//list of player coin types

    public enum EnemycoinTypes //???? what coins do I make???
    {
        none,
        standard,
        combo
    }

    void Start ()
    {


    }

    public CoinStats(string _itemName, string _itemDescription, string _itemDescription2, int _itemID, int H_attack, int H_defence, int H_HP, int T_attack, int T_defence, int T_HP, coinTypes _cType, EnemycoinTypes _Etypes)
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

        cType = _cType;
        ETypes = _Etypes;

    }

    public CoinStats()
    {
    }

   void Update ()
    {
	}

    public void chkMe()
    {
        ///////////////////
        if (cType == coinTypes.standard)
        {
            isFlipCoin = false;
        }
        else
        {
            isFlipCoin = true;
        }

        if (cType != coinTypes.none && ETypes == EnemycoinTypes.none)
        {
            isPlayerCoin = true;
            //Debug.Log(1);
        }
        if (cType == coinTypes.none && ETypes != EnemycoinTypes.none)
        {
            isPlayerCoin = false;
            //Debug.Log(2);
        }
        else if (cType == coinTypes.none && ETypes == EnemycoinTypes.none || cType != coinTypes.none && ETypes != EnemycoinTypes.none)
        {
            Debug.Log("please pick one coin type");
        }
    }
}


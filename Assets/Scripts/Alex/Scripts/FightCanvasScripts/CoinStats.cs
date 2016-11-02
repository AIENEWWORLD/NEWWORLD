using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CoinStats
{
    public string itemName;
    public string itemDescription;
    public int itemID;
    public TextureEnum myTexture;
    //
    public int Heads_attack;
    public int Heads_defence;
    public int Heads_HP; //+ or -.
    //
    public int Tails_attack;
    public int Tails_defence;
    public int Tails_HP; // + or -.
    //[HideInInspector]
    public bool notEmpty = false;
    //ENEMY ONLY COINS

    //I decided to make these bools, rather than enums so that we can use multiple
    [Header("Enemy Coins Only-------------")]
    public bool BleedCoin;        //ENEMY WITH BLEED COINS ONLY USES BLEED COINS deals 1 damage, if unblocked increase counter, if counter reaches 5 deal 5 damage, ------ done
    public bool RegenCoin;        //chance for the enemy to regenerate to X health upon death, should probably remove this coin from the enemylist upon using it 4hp --- done
    public bool DealDmgGainHealth;//ENEMY WITH THIS COIN ONLY USES THESE COINS deal 1 damage, gain 1 damage if unblocked ------ done not tested
    public bool DealDmgDealDmg;   //deal 1 damage, deal 2 damage if player health < x --- done but combat2 needs to be fixed
    public bool DuplicateCoin;    //weird ---- done
    public bool CurseCoin;        //count to 5 if it reaches 5 player loses halfHealth --- done but no UI?


    [Space][Space]
    [Header("Selections")]
    public bool activeonHeads = true;
    [HideInInspector]
    public bool isPlayerCoin;
    [HideInInspector]
    public bool isFlipCoin;

    public bool isHeads;

    public bool isSelected = false;
    [Header("Use this for dropCoins")]
    public List<StatsScript.Enemy> en = new List<StatsScript.Enemy>();//only for dropcoins.
    [Header("cost to buy")]
    public int cost = 0;//only for the shop coins that you can buy
    [Header("use only for player coins")]
    public coinTypes cType;
    [Header("adjust only for enemy coins")]
    public EnemycoinTypes ETypes;

    public enum coinTypes
    {
        none,//done
        standard,//done
        flip,//done
        secondChance, //flips the coin for a 50/50 chance again - done
        Double, //done
        Mother,// done
        Father,// done
        Child,// done
    }//list of player coin types

    public enum EnemycoinTypes //???? what coins do I make???
    {
        none,
        standard,
    }

    public enum TextureEnum
    {
        LiamsCoin,
        Texture_attack,
        Texture_defence,
        Texture_heal,
        Texture_bleed,
        Texture_regen,
        Texture_dealdmggainhealth,
        Texture_dealdmgdealdmg,
        Texture_duplicate,
        Texture_flip,
        Texture_secondChance,
        Texture_Double,
        Texture_Mother,
        Texture_Father,
        Texture_Child,
    }

    void Start ()
    {

    }

    public CoinStats(string _itemName, string _itemDescription, string _itemDescription2, int _itemID, int H_attack, int H_defence, int H_HP, int T_attack, int T_defence, int T_HP, coinTypes _cType, EnemycoinTypes _Etypes, bool _isHeads,bool useSprite,bool isempty, bool _DuplicateCoin)
    {
        DuplicateCoin = _DuplicateCoin;
        itemName = _itemName;
        itemDescription = _itemDescription;
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
        isHeads = _isHeads;
        notEmpty = isempty;

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

    public Texture GetTexture()
    {
        Texture t = GameObject.FindGameObjectWithTag("checkCombat").GetComponent<SetupCoinTextures>().TexSetter[(int)myTexture]; //what even is this
        //Debug.Log((int)myTexture);
        return t;
    }
}


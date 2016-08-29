using UnityEngine;
using System.Collections;

[System.Serializable]
public class CoinStats
{
    public string itemName;
    public string itemDescription;
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

	void Start ()
    {
	
	}

    public CoinStats(string _itemName, string _itemDescription, int _itemID, int H_attack, int H_defence, int H_HP, int T_attack, int T_defence, int T_HP)
    {
        itemName = _itemName;
        itemDescription = _itemDescription;
        Icon = Resources.Load<Sprite>("Coin");
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

    
	
	void Update ()
    {
	
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddItem : MonoBehaviour
{
    //10x5 list
    public List<CoinStats> coins = new List<CoinStats>(); //save this
    


    // Use this for initialization
    void Start ()
    {
        
        //coins.Add(new CoinStats("MyName", "MyDescription", 0, 1, 0, 0, 0, 0, 0));
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void AddCoin(CoinStats coin)
    {
        coins.Add(coin);
    }
}

//remember you can only use a limited number of coins at once,

//list of coins that the player has equipped (size 5 in total) (store list outside of combat), this can be set to the inventory list the very first time it's activated

//add those preset coins to the inventory list once(store list outside of combat) think about how to save my lists so it can be re-loaded

//on "you win" screen when you click button preset coins list it changes to last list of 5 coins. Also not being 5 but upgraded amount
//when you get a new coin from combat/buy one add to list outside of combat

//view inventory out of combat


//attacking:
//go through list of coins in the smallList of 5 coins, add up all the attack, defence and hp, heal first, then deal damage.
//go through enemy list in same way.
//ALWAYS HEAL BEFORE DEALING DAMAGE.

//list of enemies, holds posoffset and index, coinsList for that enemy.
//when the player walks into enemy instantiate at that position using the index of the enemy, take in that enemy list for the combat sequence.
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDropCoins : MonoBehaviour
{

    public List<CoinStats> DropCoins = new List<CoinStats>(); //save this
    CreateInventory cInv;
    int count;

    [HideInInspector]
    public bool dead = false;

    // Use this for initialization
    void Start ()
    {
        cInv = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<CreateInventory>();
        count = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<AddItem>().coins.Count;
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    int getRandom(int rangeMin, int rangeMax)
    {
        return (Random.Range(rangeMin, rangeMax));
    }

    //chance out of rangeMax, minrange (0), rangeMax(100)
    bool getRandom2(int chance, int rangeMin, int rangeMax)//returns true if random number is higher
    {
        return (Random.Range(rangeMin, rangeMax) > rangeMax - chance);
    }

    public void onKilled(StatsScript.Enemy gType, int gold, int droprate)
    {
        CoinStats coin = new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0,CoinStats.coinTypes.standard,CoinStats.EnemycoinTypes.none, false,false,true,false,CoinStats.TextureEnum.Atk);
        dead = true;
        if (getRandom2(droprate, 0, 100))
        {
            if (DropCoins.Count > 0)
            {
                List<CoinStats> newDcoins = new List<CoinStats>();

                for (int i = 0; i < DropCoins.Count; i++)
                {
                    for(int x = 0; x < DropCoins[i].en.Count; x++)
                    {
                        if(DropCoins[i].en[x].CompareTo(gType) == 0 && newDcoins.Contains(DropCoins[i]) == false)
                        {
                            newDcoins.Add(DropCoins[i]);
                            
                        }
                    }
                }
                if(newDcoins.Count > 0)
                {
                    int rand = getRandom(0, newDcoins.Count);

                    newDcoins[rand].itemID += count;
                    GameObject.FindGameObjectWithTag("FightCamera").GetComponent<AddItem>().AddCoin(newDcoins[rand]);
                    cInv.addItem(newDcoins[rand].itemID);
                    //Debug.Log(newDcoins.Count + " you got " + newDcoins[rand].itemName + " for killing " + gType);
                    //gameObject.GetComponent<OnWinLose>().CheckDeath(true, newDcoins[rand]);
                    coin = newDcoins[rand];
                    cInv.fullReset(-320, 170);
                    cInv.reAddItems();
                    DropCoins.Remove(newDcoins[rand]);

                }
            }
        }
        else
        {
            //Debug.Log(gType + " did not drop a coin");
        }
        gameObject.GetComponent<ButtonsPressed>().endcombat();
        gameObject.GetComponent<OnWinLose>().CheckDeath(true, coin, gold);
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Coin_buttonscript : MonoBehaviour, IPointerUpHandler
{
    public GameObject Parent;
    public int myID;
    public CoinStats coin_stats;
    public StatsScript playerstats;
    public AddItem AddI;
    public CreateInventory inv;
    public CoinShopManager CSM;

    public bool active;

	void Start ()
    {
        playerstats = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>();
        AddI = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<AddItem>();
        inv = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<CreateInventory>();

        active = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        //on click buy take away the cost from the players current gold, remove me from the list, add me to the players inventory.
        if (active)
        {
            playerstats.gold -= coin_stats.cost;
            coin_stats.itemID = AddI.coins.Count;
            AddI.AddCoin(coin_stats);
            inv.addItem(AddI.coins.Count);
            inv.fullReset(-260, 180);
            inv.reAddItems();
            CSM.destroyatID(Parent);
            CSM.checkShop();
            active = false;
        }
    }

    void Update ()
    {
        //on enter shop, and after buying check if you can afford this, if not grey me out.
	}

    public void setbuttonText()
    {

        if (active == true)
        {
            gameObject.GetComponentInChildren<Text>().text = "Buy for " + coin_stats.cost.ToString() + " gold";
        }
        else
        {
            gameObject.GetComponentInChildren<Text>().text = "Can't afford " + coin_stats.cost.ToString() + " gold";
        }
    }
}

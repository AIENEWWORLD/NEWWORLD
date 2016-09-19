using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnWinLose : MonoBehaviour
{
    public GameObject FightCanvas;

    public GameObject endCombatCanvas;
    public Text Title;
    public Text RecievedText;
    public Transform CoinPosition;
    public GameObject PlayerCoinPrefab;
    public GameObject item;
    public bool countdown = false;
    public bool counteddown = false;
    public float count = 2;

    void Start ()
    {
        endCombatCanvas.SetActive(false);
	}
	
	void Update ()
    {
	    //if input getbuttondown mouse and fightcanvas isnt enabled and endcombatcanvas is enabled
        //close the canvas
        //destroy the coin
        if(countdown == true && counteddown == false)
        {
            count -= Time.deltaTime;
            if(count <= 0)
            {
                counteddown = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && FightCanvas.activeSelf == false && endCombatCanvas.activeSelf == true && counteddown == true)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            FightCanvas.SetActive(true);
            endCombatCanvas.SetActive(false);
            if (item != null)
            {
                if (item.GetComponent<DisplayCoins>().mouseover != null)
                {
                    item.GetComponent<DisplayCoins>().mouseover.SetActive(false);
                }
                Destroy(item);
            }

        }
	}


    //heal if the amount to heal is smaller than the supplies, or else heal for as much as we can.

    public void CheckDeath(bool dead, CoinStats coin, int gold)
    {

        Title.text = "";
        RecievedText.text = "";

        FightCanvas.SetActive(false);
        if (dead == true)
        {
            Title.text = "You Win";
            gameObject.GetComponent<SetupFight>().playerStats.gold += gold;
            StatsScript playerstats = gameObject.GetComponent<SetupFight>().playerStats;
            int heal = playerstats.maxHealth - playerstats.health;
            int supplies = playerstats.supplies;
            if(playerstats.supplies > heal)
            {
                supplies -= heal;
            }
            else
            {
                heal = supplies;
            }
            playerstats.health += heal;

            RecievedText.text = "you got " + gold + " gold\nTotal gold: " + playerstats.gold;

            RecievedText.text = RecievedText.text + "\nYou used " + heal + " supplies to heal to " + playerstats.health;

            playerstats.supplies -= heal;

            RecievedText.text = RecievedText.text + "\nClick to continue...";
            //healed = supplies - health

            //you used x supplies to heal x health.
            if(coin.itemName.CompareTo("") == 1)
            {
                Debug.Log(coin.itemName);
                item = Instantiate(PlayerCoinPrefab);
                item.GetComponent<DisplayCoins>().coin = coin;
                item.transform.SetParent(CoinPosition);
                item.transform.localScale = new Vector3(2, 2, 2);
                item.transform.localPosition = new Vector3(0, 0, 0);
                //item.transform.localPosition = CoinPosition.position;
                //item.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if(dead == false)
        {
            Title.text = "You Died";
            RecievedText.text = "click to continue...";

            //do stuff here I guess
        }
        endCombatCanvas.SetActive(true);
        counteddown = false;
        countdown = true;
        count = 2;
    }
}

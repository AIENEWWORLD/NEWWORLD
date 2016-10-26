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
    RespawnEnemies respawnscript;

    void Start()
    {
        endCombatCanvas.SetActive(false);
        respawnscript = GameObject.FindGameObjectWithTag("EnemyRespawner").GetComponent<RespawnEnemies>();
    }

    void Update()
    {
        //if input getbuttondown mouse and fightcanvas isnt enabled and endcombatcanvas is enabled
        //close the canvas
        //destroy the coin
        if (countdown == true && counteddown == false)
        {
            count -= Time.deltaTime;
            if (count <= 0)
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
            gameObject.GetComponent<SetupFight>().playerinCombat = false;

        }
    }

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
            float heal = playerstats.maxHealth - playerstats.health;
            float supplies = playerstats.supplies;
            if (playerstats.supplies > heal)
            {
                supplies -= heal;
            }
            else
            {
                heal = (int)supplies;
            }
            playerstats.health += heal;

            RecievedText.text = "you got " + gold + " gold\nTotal gold: " + playerstats.gold;
            if (heal != 0)
            {
                RecievedText.text = RecievedText.text + "\nYou used " + heal + " supplies to heal to " + playerstats.health;
            }
            playerstats.supplies -= heal;

            RecievedText.text = RecievedText.text + "\nClick to continue...";

            if (coin.itemName.CompareTo("") == 1)
            {
                item = Instantiate(PlayerCoinPrefab);
                item.GetComponent<DisplayCoins>().coin = coin;
                item.transform.SetParent(CoinPosition);
                item.transform.localScale = new Vector3(2, 2, 2);
                item.transform.localPosition = new Vector3(0, 0, 0);
                //item.transform.localPosition = CoinPosition.position;
                //item.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (dead == false)
        {
            Title.text = "You Died";
            RecievedText.text = "click to continue...";

            if (gameObject.GetComponent<SetupFight>().Enemy.GetComponent<StatsScript>().guyType == StatsScript.enumType.boss)
            {
                respawnscript.EnemyList.Add(gameObject.GetComponent<SetupFight>().Enemy);
            }
        }
        gameObject.GetComponent<SetupFight>().mouseover.SetActive(false);
        endCombatCanvas.SetActive(true);
        counteddown = false;
        countdown = true;
        count = 2;
        gameObject.GetComponent<SetupFight>().addedCounters = 0;
    }
}

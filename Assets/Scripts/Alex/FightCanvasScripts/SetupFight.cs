using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetupFight : MonoBehaviour
{
    /*TO DO:
     * change enemy name
     * lock some scripts yo
     * visual indication of coins and heads/tails, make cylinder flip and stuff.
     * autoresolve
     * for healing take away supplies from the player at the end of combat using a rate
     * implement death system.
     * clean up script
     * 
     * make animations play only when attacking.
     * shop menu
     * options menu, also options menu doesn't reload from the actual controls, go from menu to options to key bindings change controls, apply, return to menu, go back to options and its the same.
     * make sure my scripts can all save and load
     * totalcoins is a bit broken
     * while in paused menu and in combat, the combat attack buttons are still active.
     * 
     * Notes:
     * combat ends in the enemydropcoins script, referencing the buttonspressed script
     */
    public GameObject Inventory;
    public GameObject FightScreen;
    public bool inventoryisActive = true;

    public List<CoinStats> EnemycoinList = new List<CoinStats>(); //save this
    public List<GameObject> EnemyitemList = new List<GameObject>(); //save this

    public List<CoinStats> PlayercoinList = new List<CoinStats>(); //save this
    public List<GameObject> PlayeritemList = new List<GameObject>(); //save this

    public bool enterCombat = false;

    public GameObject PlayerCoinPrefab;
    public GameObject EnemyCoinPrefab;

    public StatsScript playerStats;
    public StatsScript enemyStats;//the way this is being set could be dodgy///////////////////////////////////////

    float offsetPosX = -130; //player offset coins
    float offsetPosY = -22; //player offset coins

    float EnemyoffsetPosX = -130; //Enemy offset coins
    float EnemyoffsetPosY = -95; //Enemy offset coins

    Transform fightPanel;

    AddItem AddItm;

    [HideInInspector]
    public GameObject mouseover;
    //[HideInInspector]
    public bool playerAttacks = true;
    [HideInInspector]
    public bool enemyAttacks = true;

    public Vector3 screenmousePos;
    public Vector2 MouseOverTextOffset = new Vector2(0, 0); //this needs to offset itself so the mouseover can check if it's over the button.

    public int playerFleeRate = 50;
    public int playerAttack = 0, playerDefence = 0, playerHeal = 0;
    public int enemyAttack = 0, enemyDefence = 0, enemyHeal = 0;
    public bool calcFight = false;
    public Slider playerSlider;
    public Slider EnemySlider;

    // [HideInInspector]
    public bool playerinCombat = false;

    CreateInventory inventory;

    // Use this for initialization
    void Start()
    {
        playerStats = gameObject.GetComponent<StatsScript>();

        //PlayercoinList add this to inventory list.
        fightPanel = GameObject.FindGameObjectWithTag("FightPanel").transform;
        AddItm = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<AddItem>();

        mouseover = GameObject.FindGameObjectWithTag("MouseOverText");
        mouseover.SetActive(false);

        inventory = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<CreateInventory>();


    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryisActive)
        {
            Inventory.SetActive(true);
            FightScreen.SetActive(false);
        }
        else
        {
            Inventory.SetActive(false);
            FightScreen.SetActive(true);
        }

        if (enterCombat)
        {
            onEnterCombat();
            enterCombat = false;
        }

        if (calcFight)
        {
            calculateFight();
            applyFight();
            calcFight = false;
        }
        //mouseover.SetActive(false);

        screenmousePos = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mouseover.transform.position = new Vector3(screenmousePos.x + MouseOverTextOffset.x, screenmousePos.y + MouseOverTextOffset.y, mouseover.transform.position.z);
    }

    public void setEnemyList(List<CoinStats> cList)
    {
        //coinList.Clear();
        EnemycoinList = cList;
    }

    public void setInventory()
    {
        inventoryisActive = !inventoryisActive;
        inventory.greyout = true;
        enterCombat = true;
    }

    public void onEnterCombat()
    {
        playerinCombat = true;
        playerSlider.maxValue = playerStats.maxHealth;
        EnemySlider.maxValue = enemyStats.maxHealth;

        playerSlider.value = playerStats.health;
        EnemySlider.value = enemyStats.health;

        inventory.greyoutUnselected();
        loadPlayerCoins();
        loadEnemyCoins();
    }

    public void onExitCombat()
    {

        // playerAttacks = true;
        clearPlayerCoins();
        clearEnemyCoins();
    }

    void clearPlayerCoins()
    {
        PlayercoinList.Clear();
        for (int i = 0; i < PlayeritemList.Count; i++)
        {
            Destroy(PlayeritemList[i].gameObject);
        }
        PlayeritemList.Clear();
    } //this should be done every time you exit combat
    void clearEnemyCoins()
    {
        for (int i = 0; i < EnemyitemList.Count; i++)
        {
            Destroy(EnemyitemList[i].gameObject);
        }
        EnemyitemList.Clear();
    }//this should be done every time you exit combat, use onExitCombat to access these publicly.

    void loadPlayerCoins()
    {
        clearPlayerCoins();

        offsetPosX = -130;
        offsetPosY = -22;


        for (int i = 0; i < AddItm.coins.Count; i++)
        {
            if (AddItm.coins[i].isSelected == true)
            {
                PlayercoinList.Add(AddItm.coins[i]);
            }
        }
        if (PlayercoinList.Count < playerStats.totalCoins)
        {
            int x = playerStats.totalCoins - PlayercoinList.Count;
            for (int i = 0; i < x; i++)
            {
                //Debug.Log(playerStats.totalCoins - PlayercoinList.Count);
                PlayercoinList.Add(new CoinStats("empty slot", "", "", 0, 0, 0, 0, 0, 0, 0));
            }
        }

        int num = 0;
        for (int x = 0; x < PlayercoinList.Count; x++)
        {
            PlayerCoinPrefab.GetComponent<PlayerCoinsScript>().itemNumber = num;
            GameObject item = Instantiate(PlayerCoinPrefab);
            PlayeritemList.Add(item);
            item.transform.SetParent(fightPanel);
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.localPosition = new Vector3(offsetPosX, offsetPosY, 0);
            offsetPosX += 60;

            PlayeritemList[x].GetComponent<PlayerCoinsScript>().coin = PlayercoinList[x];
            PlayeritemList[x].name = "PlayerCoin: " + PlayeritemList[x].GetComponent<PlayerCoinsScript>().coin.itemName;
            num++;
        }
    }
    void loadEnemyCoins()
    {
        clearEnemyCoins();

        EnemyoffsetPosX = -130;
        EnemyoffsetPosY = -95;

        int num = 0;

        for (int x = 0; x < EnemycoinList.Count; x++)
        {
            EnemyCoinPrefab.GetComponent<EnemyCoinsScript>().itemNumber = num;
            GameObject item = Instantiate(EnemyCoinPrefab);
            EnemyitemList.Add(item);
            item.transform.SetParent(fightPanel);
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.localPosition = new Vector3(EnemyoffsetPosX, EnemyoffsetPosY, 0);
            EnemyoffsetPosX += 60;

            EnemyitemList[x].GetComponent<EnemyCoinsScript>().coin = EnemycoinList[x];
            EnemyitemList[x].name = "EnemyCoin: " + EnemyitemList[x].GetComponent<EnemyCoinsScript>().coin.itemName;
            num++;
        }
    }

    //chance out of rangeMax, minrange (0), rangeMax(100)
    bool getRandom(int chance, int rangeMin, int rangeMax)//returns true if random number is higher
    {
        return (Random.Range(rangeMin, rangeMax) > rangeMax - chance);
    }

    public void calculateFight()
    {
        //play fancy flip animation on 3d model coins hopefully?

        playerAttack = 0; playerDefence = 0; playerHeal = 0;
        enemyAttack = 0; enemyDefence = 0; enemyHeal = 0;
        if (playerAttacks)
        {
            for (int i = 0; i < PlayercoinList.Count; i++)
            {
                PlayercoinList[i].isHeads = getRandom(50, 0, 100);
                if (PlayercoinList[i].isHeads == true)
                {
                    PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    playerAttack += PlayercoinList[i].Heads_attack;
                    playerDefence += PlayercoinList[i].Heads_defence;
                    playerHeal += PlayercoinList[i].Heads_HP;
                }
                else if (PlayercoinList[i].isHeads == false)
                {
                    PlayeritemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    playerAttack += PlayercoinList[i].Tails_attack;
                    playerDefence += PlayercoinList[i].Tails_defence;
                    playerHeal += PlayercoinList[i].Tails_HP;
                }
            }
        }

        if (enemyAttacks)
        {
            for (int i = 0; i < EnemycoinList.Count; i++)
            {
                EnemycoinList[i].isHeads = getRandom(50, 0, 100);

                if (EnemycoinList[i].isHeads == true)
                {
                    EnemyitemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    enemyAttack += EnemycoinList[i].Heads_attack;
                    enemyDefence += EnemycoinList[i].Heads_defence;
                    enemyHeal += EnemycoinList[i].Heads_HP;
                }
                else if (EnemycoinList[i].isHeads == false)
                {
                    EnemyitemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    enemyAttack += EnemycoinList[i].Tails_attack;
                    enemyDefence += EnemycoinList[i].Tails_defence;
                    enemyHeal += EnemycoinList[i].Tails_HP;
                }
            }
        }
    }

    void applyFight()//on death lock it so that the attack button cant be clicked again.
    {
        //player takedamage
        if (playerDefence < enemyAttack)
        {
            enemyAttack -= playerDefence;
        }

        playerStats.health = playerStats.health + (playerHeal - enemyAttack);

        if (playerStats.maxHealth < playerStats.health)
        {
            playerStats.health = playerStats.maxHealth;
        }

        if (playerStats.health <= 0)
        {
            playerStats.health = 0;
            gameObject.GetComponent<OnWinLose>().CheckDeath(false, new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0), 0);
            //do something about death too
        }

        playerSlider.value = playerStats.health;

        ///////////////enemy

        if (enemyDefence < playerAttack)
        {
            playerAttack -= enemyDefence;
        }

        enemyStats.health = enemyStats.health + (enemyHeal - playerAttack);

        if (enemyStats.maxHealth < enemyStats.health)
        {
            enemyStats.health = enemyStats.maxHealth;
        }

        if (enemyStats.health <= 0)
        {
            enemyStats.health = 0;
            //do something about death too
            if (gameObject.GetComponent<EnemyDropCoins>().dead == false)
            {
                gameObject.GetComponent<EnemyDropCoins>().onKilled(enemyStats.guyType, enemyStats.gold, enemyStats.dropRate);
            }

        }

        EnemySlider.value = enemyStats.health;
        playerAttacks = true;
        enemyAttacks = true;
    }
}
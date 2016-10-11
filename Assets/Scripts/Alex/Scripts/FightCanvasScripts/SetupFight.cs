using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetupFight : MonoBehaviour
{
    /* Fixed?: Double check
     * 
     * script execution order:
     * additem
     * createinventory
     * coinscript
     * setupfight
     * playercoinscript
     * statsscript
     * respawnenemies
     * 
     * 
     * options menu, also options menu doesn't reload from the actual controls, go from menu to options to key bindings change controls, apply, return to menu, go back to options and its the same.
     * totalcoins is a bit broken
     * while in paused menu and in combat, the combat attack buttons are still active.
     * shop menu
     * fix flee, fix defence
     * make animations play only when attacking.
     * back button on menu reenters combat screen
     * strange glitch where pimpkin moves to the right when I select a coin????????????????????????? BOX COLLIDER
     * combat stages continue to count after combat ends.
     * attack no longer greyed out when there is no coins selected. THIS IS BECAUSE OF THE EMPTY SLOTS
     * clicking coins giving errors
     * IN PLAYERCOINSSCRIPT CHANGE THE POS.Y ROTATION IN FLIP WHEN THE NEW COIN IS IN-GAME.
     * visual indication of coins and heads/tails, make cylinder flip and stuff.
     * respawn enemies get distance from player to respawn position, if player has made it x distance away, respawn the enemy.
     * risky coin double check, healing symbol + or - needs to be correct for healing(check the risky coin's tails effect)
     * npc shop with text like pokemon
     * space enables/disables the menu sometimes? strange bug (to do with what is selected on the eventsystem). SETTING SELECTIONS SEGMENT
     * spamming space while in the upgrade total coins menu breaks it
     * mouseovertext stayed enabled one time after finishing combat
     * under playerprefab1 playerobject has tag "player", does it need to be there? this breaks my respawn enemies getcomponent<Transform> position
     * supplies in negatives heal negatives after combat
     * can kind of skip the text too fast?
     * 
     * -------------------------------------------------
     * TO DO:
     * death, go back to previous supply thing, lose gold
     * timer on respawn script KINDA WORKS?
     * tutorial script
     * enemies move when in combat, play can move when in combat
     * flipping the coins on attack works by checking flip, then spin coin
     * chkme on each coinscript
     * check out the art, remember there are phases of combat, there is art to help remember and cursed coins activate the things in the bottom right.
     * add all those coins
     * autoresolve
     * reposition the enemy coins
     * lock some scripts yo
     * clean up scripts
     * make sure my scripts can all save and load
     * 
     * maybe saving can be done like this public List<Object> scripts; ?????
     * 
     * Notes:
     * combat ends in the enemydropcoins script, referencing the buttonspressed script
     * 
     * maybe I can grey out all the buttons until the applyfight has finished, run enemy defend and player attack, wait X time run player defend enemy attack, wait X time run heal on both players.
     * While applyfight is running, make the animations play
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

    public GameObject enemySprite;

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
    public Text PlayerName;
    public Text EnemyName;

    public Text Instructions;

    // [HideInInspector]
    public bool playerinCombat = false;

    CreateInventory inventory;

    public float TimeBetweenCombat = 2.0f;
    public int combatStage = 0; //1 attack + defend phase, 2 select phase, 3 heal phase

    public Text PlayerNumbers;
    public Text EnemyNumbers;

    public TextureCycler texcycle;
    public SpriteAnimator sprcycle;
    bool spr;

    public List<CoinStats> pickCoinList;
    public bool picking = false;

    public int emptyCoins = 0;

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

        if (picking)
        {

        }
    }

    public void setEnemyList(List<CoinStats> cList)
    {
        //coinList.Clear();
        EnemycoinList = cList;
    }

    public void setInventory()
    {
        inventory.greyout = true;
        enterCombat = true;
        inventoryisActive = !inventoryisActive;
    }

    public void onEnterCombat()
    {
        playerinCombat = true;
        texcycle = null;
        sprcycle = null;


        if (enemySprite.GetComponent<TextureCycler>() != null)
        {
            texcycle = enemySprite.GetComponent<TextureCycler>();
            texcycle.enable = false;
            spr = false;
        }
        else
        {
            if (enemySprite.GetComponent<SpriteAnimator>() != null)
            {
                sprcycle = enemySprite.GetComponent<SpriteAnimator>();
                sprcycle.enable = false;
                spr = true;
            }
        }

        EnemyName.text = enemyStats.Name;
        PlayerName.text = playerStats.Name;

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
        combatStage = 0;
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
                emptyCoins = x;
                PlayercoinList.Add(new CoinStats("empty slot", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none));
            }
        }
        else
        {
            emptyCoins = 0;
        }
        if (PlayercoinList.Count - emptyCoins < 1)
        {
            gameObject.GetComponent<ButtonsPressed>().FlipButton.interactable = false;
        }
        else
        {
            gameObject.GetComponent<ButtonsPressed>().FlipButton.interactable = true;
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
        //pickCoinList.Clear();
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
                    if (PlayercoinList[i].cType == CoinStats.coinTypes.flip && PlayercoinList[i].activeonHeads == true || PlayercoinList[i].cType == CoinStats.coinTypes.counter && PlayercoinList[i].activeonHeads == true)
                    {
                        PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0.8f, 1);
                        pickCoinList.Add(PlayercoinList[i]);
                    }
                }
                else if (PlayercoinList[i].isHeads == false)
                {
                    PlayeritemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    playerAttack += PlayercoinList[i].Tails_attack;
                    playerDefence += PlayercoinList[i].Tails_defence;
                    playerHeal += PlayercoinList[i].Tails_HP;
                    if (PlayercoinList[i].cType == CoinStats.coinTypes.flip && PlayercoinList[i].activeonHeads == false || PlayercoinList[i].cType == CoinStats.coinTypes.counter && PlayercoinList[i].activeonHeads == false)
                    {
                        PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0.8f, 1);
                        pickCoinList.Add(PlayercoinList[i]);
                    }
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

    //onplayerattacks do player stuff, onenemyattacks, do enemy stuff.
    //make attacking, defending etc string functions and return the amount defended and attacked etc.
    //disable the buttons on the combat screen

    void applyFight()//on death lock it so that the attack button cant be clicked again.
    {

        StartCoroutine(PlayerCombat(0));
    }


    IEnumerator PlayerCombat(float time)
    {
        yield return new WaitForSeconds(time);
        combatStage += 1;
        //Debug.Log(combatStage);

        //take damage
        if (gameObject.GetComponent<OnWinLose>().endCombatCanvas.activeSelf == false)
        {
            if (combatStage == 1)//player attacks + enemy defends
            {
                if (spr)
                {
                    sprcycle.enable = true;
                }
                else
                {
                    texcycle.enable = true;
                }
                if (pickCoinList.Count > 0)
                {
                    combatStage = 0;
                    Instructions.text = "pick a " + pickCoinList[0].cType + " coin";
                    picking = true;
                }

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 2)
            {

                Instructions.text = "Instructions";
                setColoursHT();
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 3)//enemy attacks + player defends
            {

                if (enemyAttacks)
                {

                    enemyAttack -= playerDefence;
                    if (0 > enemyAttack)
                    {
                        enemyAttack = 0;
                    }
                    playerStats.health = playerStats.health - enemyAttack;
                    PlayerNumbers.text = "-" + enemyAttack.ToString();
                    playerSlider.value = playerStats.health;

                }


                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 4)// enemy + player heal
            {
                if (playerStats.health <= 0)
                {
                    playerStats.health = 0;
                    gameObject.GetComponent<OnWinLose>().CheckDeath(false, new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none), 0);
                    //playerStats.dead = true;
                    GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<RespawnPlayer>().FindNearestRespawn();
                    //do something about death too
                }

                if (playerAttacks)
                {

                    playerAttack -= enemyDefence;
                    if (0 > playerAttack)
                    {
                        playerAttack = 0;
                    }
                    enemyStats.health = enemyStats.health - playerAttack;
                    EnemyNumbers.text = "-" + playerAttack.ToString();
                    EnemySlider.value = enemyStats.health;

                }

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 5)
            {
                if (enemyStats.health <= 0)
                {
                    enemyStats.health = 0;
                    //do something about death too
                    if (gameObject.GetComponent<EnemyDropCoins>().dead == false)
                    {
                        gameObject.GetComponent<EnemyDropCoins>().onKilled(enemyStats.guyType, enemyStats.gold, enemyStats.dropRate);
                    }

                }


                playerStats.health = playerStats.health + playerHeal;
                if (playerHeal > 0)
                {
                    PlayerNumbers.text = "+" + playerHeal.ToString();
                }
                else if (playerHeal < 0)
                {
                    PlayerNumbers.text = "" + playerHeal.ToString();
                }
                if (playerStats.maxHealth < playerStats.health)
                {
                    playerStats.health = playerStats.maxHealth;
                }

                enemyStats.health = enemyStats.health + enemyHeal;
                EnemyNumbers.text = "+" + enemyHeal.ToString();
                if (enemyStats.maxHealth < enemyStats.health)
                {
                    enemyStats.health = enemyStats.maxHealth;
                }

                playerSlider.value = playerStats.health;
                EnemySlider.value = enemyStats.health;

                playerAttacks = true;
                enemyAttacks = true;
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 6)
            {
                //RECHECK DEATH since players can heal negative values (risky coins)
                if (playerStats.health <= 0)
                {
                    playerStats.health = 0;
                    gameObject.GetComponent<OnWinLose>().CheckDeath(false, new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none), 0);
                    //playerStats.dead = true;
                    GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<RespawnPlayer>().FindNearestRespawn();
                    //do something about death too
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


                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                Instructions.text = "Instructions";

                if (spr)
                {
                    sprcycle.enable = false;
                }
                else
                {
                    texcycle.enable = false;
                }
                gameObject.GetComponent<ButtonsPressed>().SetInteractable(true);
                combatStage = 0;
            }
        }
        else
        {
            combatStage = 0;
            PlayerNumbers.text = "";
            EnemyNumbers.text = "";
            Instructions.text = "Instructions";
            gameObject.GetComponent<ButtonsPressed>().SetInteractable(true);
        }
    }

    public void checkSelections(CoinStats coinToFlip)
    {
        if (coinToFlip.ETypes == CoinStats.EnemycoinTypes.none)
        {
            for (int x = 0; x < PlayercoinList.Count; x++)
            {
                if (PlayercoinList[x] == coinToFlip)
                {
                    PlayeritemList[x].GetComponent<Image>().color = new Color(1, 0, 1, 1);
                }

            }
        }
        else if (coinToFlip.cType == CoinStats.coinTypes.none)
        {
            for (int x = 0; x < EnemycoinList.Count; x++)
            {
                if (EnemycoinList[x] == coinToFlip)
                {
                    EnemyitemList[x].GetComponent<Image>().color = new Color(1, 0, 1, 1);
                }

            }
        }

        if (pickCoinList[0].cType == CoinStats.coinTypes.flip)
        {
            coinToFlip.isHeads = !coinToFlip.isHeads;
        }
        if (pickCoinList[0].cType == CoinStats.coinTypes.counter)
        {
            coinToFlip.isHeads = !coinToFlip.isHeads;
        }
        pickCoinList.Remove(pickCoinList[0]);
        if (pickCoinList.Count <= 0)
        {
            calculateFight2();
            picking = false;
        }

    }
    void setColoursHT()
    {
        for (int i = 0; i < PlayercoinList.Count; i++)
        {
            if (PlayercoinList[i].isHeads == true)
            {
                PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
            }
            else if (PlayercoinList[i].isHeads == false)
            {
                PlayeritemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
            }
        }
    }

    public void calculateFight2() //temporary fix
    {
        //play fancy flip animation on 3d model coins hopefully?

        playerAttack = 0; playerDefence = 0; playerHeal = 0;
        enemyAttack = 0; enemyDefence = 0; enemyHeal = 0;
        //pickCoinList.Clear();
        if (playerAttacks)
        {
            for (int i = 0; i < PlayercoinList.Count; i++)
            {

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
}



/*
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

    */



/*
 * attack, if you have flip/counter coins display the text saying which coin you are using
 * let the player select from the coins grey out coins that cant be flipped, no flip or counter coins can be flipped apart from myself
 * change the coin colour of the flipped coin
 * check the coins enum to determine which coins it can pick from
 * reset the colours so that people know what happened
 * continue combat.
 */

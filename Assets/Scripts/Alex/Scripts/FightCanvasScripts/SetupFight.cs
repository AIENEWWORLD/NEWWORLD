using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SetupFight : MonoBehaviour
{
    /* Fixed?: Double check
     * 
     * FIXED PROBLEM
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
     * strange glitch where pimpkin moves to the right when I select a coin? BOX COLLIDER
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
     * death, go back to previous supply thing, lose gold
     * enemies move when in combat, play can move when in combat
     * lock some scripts
     * check out the art, remember there are phases of combat, there is art to help remember and cursed coins activate the things in the bottom right.
     * EnemyScript uses the rigidbody to zero out velocity, sometimes the enemy would just run in a random direction.
     * orthographic cameras on shop, fix input after text is fully written
     * make coins flip properly
     * multiple enemies in combat screen? CANT GET THIS TO HAPPEN. fixed on death this could happen, doesn't seem like this was the issue though.
     * ENEMY RUNS IN RANDOM DIRECTION (MAYBE BECAUSE OF RESPAWN) ----------- BECAUSE OF COLLISION PUSHING IT AWAY WHEN IT FINALLY COLLIDES WITH PLAYER
     * With collision between the enemy and player there is sometimes a chance that unity collision will still occur and the enemy will continue to add velocity away from the object it collided with this happens now that the enemies are disabled when they die rather than destroyed. with the rigidbody's mass on the player being 500, and the enemies mass being 0.1 I'd assume this is why the enemy floats away and doesn't seem to slow down.
     * blakes player movement script locks the players Y axis (preventing falling when no keys are being pressed), this might work better without that
     * enemies don't fall + if they unlock the Y axis on rigidbody they can't climb hills, mass 500 works better
     * something kinda messed up with the shop text...
     * enemyCounterCursed counts towards damage even when there is a flip coin that got heads
     * picking flip coin breaking getting random happened twice
     * WHEN YOU DIE CANVAS IS GLITCHY fixed, things weren't being reset
     * calculatecoins2 breaks other coins. Fixed
     * health not resetting upon death fixed
     * merge counter coin and flip coin into one. Done
     * shop text popup "Press" Inputkey - done
     * flip coins and remove the colours - done
     * enemy AI with roaming - done
     * while fleeing spin - done
     * ENEMYAI NEEDS TO BE ATTACHED TO EVERY ENEMY - done
     * camera rotation - done
     * add curse counter coins - done
     * play the idle animation in the CheckinCombatScript (commenting shows where). - done
     * make bosses not respawn - THEY WILL NOT RESPAWN IF THE GUYTYPE ENUM IS SET TO BOSS BUT THEY WILL IF THE PLAYER DIES TO THEM
     * COINS - done necessary coins
     * TEST COMBAT WITH FLIP COINS
     * update the calculatefight2
     * health upgrade, supply upgrade
     * put in the animations
     * smooth movement camera with deadzone kinda like this https://www.youtube.com/watch?v=WL_PaUyRAXQ
     * 
     * -------------------------------------------------
     * TO DO:
     * 
     * tutorial script when you enter combat, goes through images as player clicks
     * Sounds, check sliders
     * PLAYER SLOPE
     * Fix need for sprites
     * bloodthirst weird
     * landmark discovery text popup.
     * 
     * online portfolio
     * schedule
     * technical design doc
     * 
     * 
     * 
     * chkme on each coinscript
     * add all those coins
     * reposition the enemy coins?
     * clean up scripts
     */
    public GameObject Inventory;
    public GameObject FightScreen;
    [HideInInspector]
    public bool inventoryisActive = true;
    [HideInInspector]
    public List<CoinStats> EnemycoinList = new List<CoinStats>(); 
    [HideInInspector]
    public List<GameObject> EnemyitemList = new List<GameObject>();
    [HideInInspector]
    public List<CoinStats> PlayercoinList = new List<CoinStats>();
    [HideInInspector]
    public List<GameObject> PlayeritemList = new List<GameObject>();
    [HideInInspector]
    public bool enterCombat = false;

    public GameObject PlayerCoinPrefab;
    public GameObject EnemyCoinPrefab;
    [HideInInspector]
    public StatsScript playerStats;
    //[HideInInspector]
    public StatsScript enemyStats;
    [HideInInspector]
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
    [HideInInspector]
    public Vector3 screenmousePos;
    public Vector2 MouseOverTextOffset = new Vector2(0, 0); //this needs to offset itself so the mouseover can check if it's over the button.

    public int playerFleeRate = 50;
    [HideInInspector]
    public int playerAttack = 0, playerDefence = 0, playerHeal = 0, familyCounter = 0;
    [HideInInspector]
    public int enemyAttack = 0, enemyDefence = 0, enemyHeal = 0, enemyCounterCursed = 0;
    [HideInInspector]
    public bool enemyRegenCoin = false;
    [HideInInspector]
    public bool calcFight = false;
    public Slider playerSlider;
    public Slider EnemySlider;
    public Text PlayerName;
    public Text EnemyName;

    public float GoldToLosePercentage = 10;

    public Text Instructions;

    [HideInInspector]
    public bool playerinCombat = false;

    CreateInventory inventory;

    public float TimeBetweenCombat = 2.0f;
    public float TimeBeforeFlip = 2.0f;
    [HideInInspector]
    public int combatStage = 0; //1 attack + defend phase, 2 select phase, 3 heal phase

    public Text PlayerNumbers;
    public Text EnemyNumbers;
    bool spr;
    //[HideInInspector]
    public List<CoinStats> pickCoinList;
    [HideInInspector]
    public bool picking = false;
    [HideInInspector]
    public int emptyCoins = 0;

    int DealDmgGainHealthCoins = 0;
    int enemybleedcoin = 0;
    int bleedcoinCounter = 0;
    bool duplicate = false;

    public List<int> dupeList;

    public GameObject CounterPrefab;
    public GameObject BleedPrefab;

    Vector3 CounterStart = new Vector3(140, -255, 0);
    public List<GameObject> counters;
    public int addedCounters = 0;

    public GameObject Enemy;

    public List<StatsScript.Enemy> DiscoveryList;

    public List<GameObject> tempCoinsToDouble;

    public Animator EnemyAnims;

    public bool inList = false;

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
        // && gameObject.GetComponent<OnWinLose>().endCombatCanvas.activeInHierarchy == true
        if (addedCounters < enemyCounterCursed) //two counters can use the same code because there will never be both bleed coins and cursed coins on a single enemy
        {
            int tempInt = enemyCounterCursed - addedCounters;
            for (int i = 0; i < tempInt; i++)
            {
                GameObject s = Instantiate(CounterPrefab) as GameObject;
                s.transform.SetParent(fightPanel);
                s.transform.localScale = new Vector3(1.620384f, 1, 1);
                s.transform.localPosition = new Vector3(CounterStart.x + (45 * enemyCounterCursed), CounterStart.y, CounterStart.z);
                counters.Add(s);
                addedCounters = enemyCounterCursed;
                //Debug.Log("s");
            }
        }
        else if (addedCounters != 0 && enemyCounterCursed == 0 && bleedcoinCounter == 0)
        {
            clearCounters();
        }
        if (addedCounters < bleedcoinCounter )
        {
            int tempInt = bleedcoinCounter - addedCounters;

            for (int i = 0; i < tempInt; i++)
            {
                GameObject s = Instantiate(BleedPrefab) as GameObject;
                s.transform.SetParent(fightPanel);
                s.transform.localScale = new Vector3(1, 1, 1);
                s.transform.localPosition = new Vector3(CounterStart.x + (45 * bleedcoinCounter), CounterStart.y, CounterStart.z);
                counters.Add(s);
                addedCounters = bleedcoinCounter;
                //Debug.Log("s2");
            }
        }
        else if (addedCounters != 0 && bleedcoinCounter == 0 && enemyCounterCursed == 0)
        {
            clearCounters();
        }
    }

    void clearCounters()
    {
        enemyCounterCursed = 0;
        bleedcoinCounter = 0;
        addedCounters = 0;
        for (int i = 0; i < counters.Count; i++)
        {
            Destroy(counters[i].gameObject);
        }
        counters.Clear();
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

    public void chkBoss()
    {
        if (enemyStats.guyType == StatsScript.enumType.boss)
        {
            gameObject.GetComponent<ButtonsPressed>().FleeButton.interactable = false;
        }
    }

    public void onEnterCombat()
    {
        playerinCombat = true;

        EnemyAnims = enemySprite.GetComponent<Animator>();

        EnemyName.text = enemyStats.Name;
        PlayerName.text = playerStats.Name;

        playerSlider.maxValue = playerStats.maxHealth;
        EnemySlider.maxValue = enemyStats.maxHealth;

        playerSlider.value = playerStats.health;
        EnemySlider.value = enemyStats.health;
        gameObject.GetComponent<ButtonsPressed>().FleeButton.interactable = true;
        chkBoss();

        if (enemyStats.Monster != StatsScript.Enemy.none)
        {
            inList = false;
            for (int i = 0; i < DiscoveryList.Count; i++)
            {
                if (enemyStats.Monster == DiscoveryList[i])
                {
                    inList = true;
                }
            }
            if (inList == false)
            {
                DiscoveryList.Add(enemyStats.Monster);
            }
        }

        GameObject.FindGameObjectWithTag("Compass").GetComponent<CompassScript>().Beasts.text = DiscoveryList.Count.ToString();

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
        clearCounters();
        cleartempcoins();
    }
    void cleartempcoins()
    {
        for (int i = 0; i < tempCoinsToDouble.Count; i++)
        {
            tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Heads_attack /= 2;
            tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Heads_defence /= 2;
            tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Heads_HP /= 2;
            tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Tails_attack /= 2;
            tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Tails_defence /= 2;
            tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Tails_HP /= 2;
        }
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
                PlayercoinList.Add(new CoinStats("empty slot", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none, false,null,false,false));
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
        return (UnityEngine.Random.Range(rangeMin, rangeMax) > rangeMax - chance);
    }

    public void calculateFight()
    {
        //play fancy flip animation on 3d model coins hopefully?

        playerAttack = 0; playerDefence = 0; playerHeal = 0; familyCounter = 0;
        enemyAttack = 0; enemyDefence = 0; enemyHeal = 0;
        enemyRegenCoin = false; duplicate = false;
        DealDmgGainHealthCoins = 0; enemybleedcoin = 0;
        dupeList.Clear();

        tempCoinsToDouble.Clear();
        //pickCoinList.Clear();
        if (playerAttacks)
        {
            for (int i = 0; i < PlayercoinList.Count; i++)
            {

                PlayercoinList[i].isHeads = getRandom(50, 0, 100);

                if (PlayercoinList[i].isHeads == true)
                {
                    //PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    playerAttack += PlayercoinList[i].Heads_attack;
                    playerDefence += PlayercoinList[i].Heads_defence;
                    playerHeal += PlayercoinList[i].Heads_HP;
                    if (PlayercoinList[i].cType == CoinStats.coinTypes.flip && PlayercoinList[i].activeonHeads == true || PlayercoinList[i].cType == CoinStats.coinTypes.secondChance && PlayercoinList[i].activeonHeads == true || PlayercoinList[i].cType == CoinStats.coinTypes.Double && PlayercoinList[i].activeonHeads == true)
                    {
                        //PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0.8f, 1);
                        pickCoinList.Add(PlayercoinList[i]);
                    }
                    if(PlayercoinList[i].activeonHeads == true && PlayercoinList[i].cType == CoinStats.coinTypes.Mother || PlayercoinList[i].activeonHeads == true && PlayercoinList[i].cType == CoinStats.coinTypes.Father || PlayercoinList[i].activeonHeads == true && PlayercoinList[i].cType == CoinStats.coinTypes.Child)
                    {
                        familyCounter++;
                    }
                }
                else if (PlayercoinList[i].isHeads == false)
                {
                    //PlayeritemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    playerAttack += PlayercoinList[i].Tails_attack;
                    playerDefence += PlayercoinList[i].Tails_defence;
                    playerHeal += PlayercoinList[i].Tails_HP;
                    if (PlayercoinList[i].cType == CoinStats.coinTypes.flip && PlayercoinList[i].activeonHeads == false || PlayercoinList[i].cType == CoinStats.coinTypes.secondChance && PlayercoinList[i].activeonHeads == false || PlayercoinList[i].cType == CoinStats.coinTypes.Double && PlayercoinList[i].activeonHeads == false)
                    {
                        //PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0.8f, 1);
                        pickCoinList.Add(PlayercoinList[i]);
                    }
                    if (PlayercoinList[i].activeonHeads == false && PlayercoinList[i].cType == CoinStats.coinTypes.Mother || PlayercoinList[i].activeonHeads == false && PlayercoinList[i].cType == CoinStats.coinTypes.Father || PlayercoinList[i].activeonHeads == false && PlayercoinList[i].cType == CoinStats.coinTypes.Child)
                    {
                        familyCounter++;
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
                    //EnemyitemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    enemyAttack += EnemycoinList[i].Heads_attack;
                    enemyDefence += EnemycoinList[i].Heads_defence;
                    enemyHeal += EnemycoinList[i].Heads_HP;
                    if (EnemycoinList[i].CurseCoin == true && pickCoinList.Count == 0)///////////////////////////////////////////////////////////////////////////////////////////////////
                    {
                        enemyCounterCursed += 1;
                        //Debug.Log(enemyCounterCursed);

                    }
                    if (EnemycoinList[i].DuplicateCoin == true && EnemycoinList.Count < 5 && pickCoinList.Count == 0)
                    {
                        //Debug.Log(i);
                        duplicate = true;
                        dupeList.Add(i);


                    }
                    if (EnemycoinList[i].BleedCoin == true && pickCoinList.Count == 0)
                    {
                        enemybleedcoin += 1;
                    }
                    if (EnemycoinList[i].DealDmgGainHealth == true && pickCoinList.Count == 0)
                    {
                        DealDmgGainHealthCoins += 1;
                    }
                    if (EnemycoinList[i].DealDmgDealDmg == true && pickCoinList.Count == 0)/////////////////////////////////////////////////////////////////////////////////////////////
                    {
                        if (playerStats.health <= 10)
                        {
                            enemyAttack += 2;
                        }
                        else
                        {
                            enemyAttack += 1;
                        }
                    }
                    if (EnemycoinList[i].RegenCoin == true && pickCoinList.Count == 0)
                    {
                        enemyRegenCoin = true;
                    }
                }
                else if (EnemycoinList[i].isHeads == false)
                {
                    //EnemyitemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    enemyAttack += EnemycoinList[i].Tails_attack;
                    enemyDefence += EnemycoinList[i].Tails_defence;
                    enemyHeal += EnemycoinList[i].Tails_HP;
                }
                
            }
        }

    }
    void DupeCoin(int i)
    {
        enemyStats.coinList.Add(new CoinStats(EnemycoinList[i].itemName, EnemycoinList[i].itemDescription, "", EnemycoinList[i].itemID, EnemycoinList[i].Heads_attack, EnemycoinList[i].Heads_defence, EnemycoinList[i].Heads_HP, EnemycoinList[i].Tails_attack, EnemycoinList[i].Tails_defence, EnemycoinList[i].Tails_HP, EnemycoinList[i].cType, EnemycoinList[i].ETypes, false, EnemycoinList[i].Icon, true, EnemycoinList[i].DuplicateCoin));
        //Debug.Log("adding x");
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

        //Enemy defends for X(silver)Enemy takes X damage Player defends for X(silver)Player takes X damage Player and enemy heal for X(green)

        //take damage
                    if (gameObject.GetComponent<OnWinLose>().endCombatCanvas.activeSelf == false)
        {
            if (combatStage == 1)
            {


                if (playerAttacks)
                {
                    for (int i = 0; i < PlayercoinList.Count; i++)
                    {
                        PlayeritemList[i].GetComponent<PlayerCoinsScript>().spinrate = 20;
                    }
                }
                if(enemyAttacks)
                {
                    for(int i = 0; i < EnemycoinList.Count; i++)
                    {
                        EnemyitemList[i].GetComponent<EnemyCoinsScript>().spinrate = 20;
                    }
                }
                //time until flip
                StartCoroutine(PlayerCombat(TimeBeforeFlip));
            }
            if (combatStage == 2)
            {
                if (playerAttacks == true)
                {
                    for (int i = 0; i < PlayercoinList.Count; i++)
                    {
                        PlayeritemList[i].GetComponent<PlayerCoinsScript>().spinrate = 5;
                        PlayeritemList[i].GetComponent<PlayerCoinsScript>().flip = true;
                    }
                }
                if(enemyAttacks == true)
                {
                    for(int i = 0; i < EnemycoinList.Count; i++)
                    {
                        EnemyitemList[i].GetComponent<EnemyCoinsScript>().spinrate = 5;
                        EnemyitemList[i].GetComponent<EnemyCoinsScript>().flip = true;
                    }
                }
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 3)//player attacks + enemy defends
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////PLAY ATTACK ANIMATION
                
                
                if (pickCoinList.Count > 0)
                {
                    combatStage = 0;
                    if (pickCoinList[0].cType == CoinStats.coinTypes.flip)
                    {
                        Instructions.text = "pick a coin to reverse";
                    }
                    if (pickCoinList[0].cType == CoinStats.coinTypes.secondChance)
                    {
                        Instructions.text = "pick a coin to flip again";
                    }
                    if (pickCoinList[0].cType == CoinStats.coinTypes.Double)
                    {
                        Instructions.text = "pick a coin to double";
                    }
                    picking = true;
                }

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 4)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                Instructions.text = "Instructions";
                setColoursHT();
                //if (enemyDefence > 0)
                //{
                //    EnemyNumbers.color = new Color(192, 192, 192);
                //    EnemyNumbers.text = enemyDefence.ToString();
                //}
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 5)// enemy + player heal
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                if (playerStats.health <= 0)
                {
                    playerStats.health = 0;
                    gameObject.GetComponent<ButtonsPressed>().endcombat();
                    gameObject.GetComponent<OnWinLose>().CheckDeath(false, new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none,false,null,false,false), 0);
                    playerStats.health = playerStats.maxHealth;
                    playerStats.supplies = playerStats.maxSupply;
                    playerStats.gold = Convert.ToInt32(playerStats.gold * (GoldToLosePercentage / 100));//(int)(playerStats.gold * (GoldToLosePercentage/100));


                    //playerStats.dead = true;
                    GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<RespawnPlayer>().FindNearestRespawn();
                    //do something about death too
                }

                if (playerAttacks)
                {

                    playerAttack -= enemyDefence;
                    if (familyCounter == 1)
                    {
                        playerAttack += 1;
                    }
                    else if (familyCounter == 2)
                    {
                        playerAttack += 5;
                    }
                    else if (familyCounter == 3)
                    {
                        playerAttack += 10;
                    }
                    if (0 > playerAttack)
                    {
                        playerAttack = 0;
                    }
                    enemyStats.health = enemyStats.health - playerAttack;
                    EnemyNumbers.color = Color.red;
                    EnemyNumbers.text = "" + playerAttack.ToString();
                    EnemySlider.value = enemyStats.health;

                }

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 6)//enemy attacks + player defends
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                if (enemyStats.health <= 0)
                {
                    enemyStats.health = 0;
                    //do something about death too
                    if (gameObject.GetComponent<EnemyDropCoins>().dead == false && !enemyRegenCoin)
                    {
                        gameObject.GetComponent<EnemyDropCoins>().onKilled(enemyStats.Monster, enemyStats.gold, enemyStats.dropRate);
                    }
                    else
                    {
                        enemyStats.health = enemyStats.maxHealth / 2;
                    }

                }
                //if (playerDefence > 0)
                //{
                //    PlayerNumbers.color = new Color(192, 192, 192); //silver
                //    PlayerNumbers.text = playerDefence.ToString();
                //}

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if(combatStage == 7)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                if (enemyAttacks)
                {
                    
                    EnemyAnims.Play("Attack");
                    if (DealDmgGainHealthCoins != 0)
                    {
                        DealDmgGainHealthCoins -= playerDefence;////////////////////////////////////////////////////////////////////////////////////////
                        if (DealDmgGainHealthCoins > 0)
                        {
                            enemyHeal += DealDmgGainHealthCoins;
                        }
                    }
                    if (enemybleedcoin != 0)
                    {
                        bleedcoinCounter += enemybleedcoin;
                        //Debug.Log(bleedcoinCounter);
                        if (bleedcoinCounter >= 5)
                        {
                            enemyAttack += 3;
                            bleedcoinCounter = 0;
                        }
                    }
                    enemyAttack -= playerDefence;
                    if (0 > enemyAttack)
                    {
                        enemyAttack = 0;
                    }
                    if (enemyCounterCursed >= 5)///////////////////////////////////////////////////////////////////////////////////////////////////
                    {
                        enemyAttack = (int)playerStats.health;
                        //playerStats.health /= 2;
                        enemyCounterCursed = 0;
                    }
                    playerStats.health = playerStats.health - enemyAttack;


                    PlayerNumbers.color = Color.red;
                    PlayerNumbers.text = "" + enemyAttack.ToString();
                    playerSlider.value = playerStats.health;
                }
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 8)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                if (enemyStats.health <= 0)
                {
                    enemyStats.health = 0;
                    //do something about death too
                    if (gameObject.GetComponent<EnemyDropCoins>().dead == false && !enemyRegenCoin)
                    {
                        gameObject.GetComponent<EnemyDropCoins>().onKilled(enemyStats.Monster, enemyStats.gold, enemyStats.dropRate);
                    }
                    else
                    {
                        enemyStats.health = enemyStats.maxHealth / 2;
                    }

                }

                PlayerNumbers.color = Color.green;
                PlayerNumbers.text = "";
                playerStats.health = playerStats.health + playerHeal;
                if (playerHeal > 0)
                {
                    PlayerNumbers.text = "" + playerHeal.ToString();
                }
                if (playerStats.maxHealth < playerStats.health)
                {
                    playerStats.health = playerStats.maxHealth;
                }

                enemyStats.health = enemyStats.health + enemyHeal;
                EnemyNumbers.color = Color.green;
                if (enemyHeal > 0)
                {
                    EnemyNumbers.text = "" + enemyHeal.ToString();
                }
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
            if (combatStage == 9)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                //RECHECK DEATH since players can heal negative values (risky coins)
                if (playerStats.health <= 0)
                {
                    playerStats.health = 0;
                    gameObject.GetComponent<OnWinLose>().CheckDeath(false, new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none,false,null,false,false), 0);
                    //playerStats.dead = true;
                    GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<RespawnPlayer>().FindNearestRespawn();
                    //do something about death too
                }

                if (enemyStats.health <= 0)
                {
                    enemyStats.health = 0;
                    //do something about death too
                    if (gameObject.GetComponent<EnemyDropCoins>().dead == false && !enemyRegenCoin)
                    {
                        gameObject.GetComponent<EnemyDropCoins>().onKilled(enemyStats.Monster, enemyStats.gold, enemyStats.dropRate);
                    }
                    else
                    {
                        enemyStats.health = enemyStats.maxHealth / 2;
                    }

                }


                Instructions.text = "Instructions";

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////PLAY IDLE ANIMATION
                EnemyAnims.Play("Idle");
                gameObject.GetComponent<ButtonsPressed>().SetInteractable(true);
                chkBoss();
                for (int i = 0; i < PlayercoinList.Count; i++)
                {
                    PlayeritemList[i].GetComponent<PlayerCoinsScript>().flip = false;
                }
                for (int i = 0; i < EnemycoinList.Count; i++)
                {
                    EnemyitemList[i].GetComponent<EnemyCoinsScript>().flip = false;
                }


                for (int i = 0; i < dupeList.Count; i++)
                {
                    if (EnemycoinList.Count < 5)
                        DupeCoin(dupeList[i]);
                    clearEnemyCoins();
                    setEnemyList(enemyStats.coinList);
                    loadEnemyCoins();
                    setColoursHT();
                }

                for (int i = 0; i < tempCoinsToDouble.Count; i++)
                {
                    tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Heads_attack /= 2;
                    tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Heads_defence /= 2;
                    tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Heads_HP /= 2;
                    tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Tails_attack /= 2;
                    tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Tails_defence /= 2;
                    tempCoinsToDouble[i].GetComponent<PlayerCoinsScript>().coin.Tails_HP /= 2;
                }

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
            chkBoss();
        }
    }

    public void checkSelections(CoinStats coinToFlip, GameObject coinGameObject)
    {
        if (coinToFlip.ETypes == CoinStats.EnemycoinTypes.none)
        {
            //for (int x = 0; x < PlayercoinList.Count; x++)
            //{
            //    if (PlayercoinList[x] == coinToFlip)
            //    {
            //        PlayeritemList[x].GetComponent<Image>().color = new Color(1, 0, 1, 1);
            //    }
            //
            //}
        }
        else if (coinToFlip.cType == CoinStats.coinTypes.none)
        {
            //for (int x = 0; x < EnemycoinList.Count; x++)
            //{
            //    if (EnemycoinList[x] == coinToFlip)
            //    {
            //        EnemyitemList[x].GetComponent<Image>().color = new Color(1, 0, 1, 1);
            //    }
            //
            //}
        }
        

        if (pickCoinList[0].cType == CoinStats.coinTypes.flip)
        {
            coinToFlip.isHeads = !coinToFlip.isHeads;
        }
        if (pickCoinList[0].cType == CoinStats.coinTypes.secondChance)
        {
            coinToFlip.isHeads = getRandom(50, 0, 100);
        }
        if (pickCoinList[0].cType == CoinStats.coinTypes.Double)
        {
            tempCoinsToDouble.Add(coinGameObject);

            coinToFlip.Heads_attack *= 2;
            coinToFlip.Heads_defence *= 2;
            coinToFlip.Heads_HP *= 2;
            coinToFlip.Tails_attack *= 2;
            coinToFlip.Tails_defence *= 2;
            coinToFlip.Tails_HP *= 2;

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
        //for (int i = 0; i < PlayercoinList.Count; i++)
        //{
        //    if (PlayercoinList[i].isHeads == true)
        //    {
        //        PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
        //    }
        //    else if (PlayercoinList[i].isHeads == false)
        //    {
        //        PlayeritemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
        //    }
        //}
        //for (int i = 0; i < EnemycoinList.Count; i++)
        //{
        //    if (EnemycoinList[i].isHeads == true)
        //    {
        //        EnemyitemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
        //    }
        //    else if (EnemycoinList[i].isHeads == false)
        //    {
        //        EnemyitemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
        //    }
        //}
    }

    /*
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
    */

    public void calculateFight2()
    {
        //play fancy flip animation on 3d model coins hopefully?

        playerAttack = 0; playerDefence = 0; playerHeal = 0; familyCounter = 0;
        enemyAttack = 0; enemyDefence = 0; enemyHeal = 0;
        enemyRegenCoin = false; duplicate = false;
        DealDmgGainHealthCoins = 0; enemybleedcoin = 0;
        dupeList.Clear();
        //pickCoinList.Clear();
        if (playerAttacks)
        {
            for (int i = 0; i < PlayercoinList.Count; i++)
            {

                if (PlayercoinList[i].isHeads == true)
                {
                    //PlayeritemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    playerAttack += PlayercoinList[i].Heads_attack;
                    playerDefence += PlayercoinList[i].Heads_defence;
                    playerHeal += PlayercoinList[i].Heads_HP;
                    if (PlayercoinList[i].activeonHeads == true && PlayercoinList[i].cType == CoinStats.coinTypes.Mother || PlayercoinList[i].activeonHeads == true && PlayercoinList[i].cType == CoinStats.coinTypes.Father || PlayercoinList[i].activeonHeads == true && PlayercoinList[i].cType == CoinStats.coinTypes.Child)
                    {
                        familyCounter++;
                    }
                }
                else if (PlayercoinList[i].isHeads == false)
                {
                    //PlayeritemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
                    playerAttack += PlayercoinList[i].Tails_attack;
                    playerDefence += PlayercoinList[i].Tails_defence;
                    playerHeal += PlayercoinList[i].Tails_HP;
                    if (PlayercoinList[i].activeonHeads == false && PlayercoinList[i].cType == CoinStats.coinTypes.Mother || PlayercoinList[i].activeonHeads == false && PlayercoinList[i].cType == CoinStats.coinTypes.Father || PlayercoinList[i].activeonHeads == false && PlayercoinList[i].cType == CoinStats.coinTypes.Child)
                    {
                        familyCounter++;
                    }
                }
            }
        }

        if (enemyAttacks)
        {
            for (int i = 0; i < EnemycoinList.Count; i++)
            {
                if (EnemycoinList[i].isHeads == true)
                {
                    //EnemyitemList[i].GetComponent<Image>().color = new Color(0, 1, 0, 1);
                    enemyAttack += EnemycoinList[i].Heads_attack;
                    enemyDefence += EnemycoinList[i].Heads_defence;
                    enemyHeal += EnemycoinList[i].Heads_HP;
                    if (EnemycoinList[i].CurseCoin == true && pickCoinList.Count == 0)///////////////////////////////////////////////////////////////////////////////////////////////////
                    {
                        enemyCounterCursed += 1;
                        //Debug.Log(enemyCounterCursed);

                    }
                    if (EnemycoinList[i].DuplicateCoin == true && EnemycoinList.Count < 5)
                    {
                        //Debug.Log(i);
                        duplicate = true;
                        dupeList.Add(i);


                    }
                    if (EnemycoinList[i].BleedCoin == true)
                    {
                        enemybleedcoin += 1;
                    }
                    if (EnemycoinList[i].DealDmgGainHealth == true)
                    {
                        DealDmgGainHealthCoins += 1;
                    }
                    if (EnemycoinList[i].DealDmgDealDmg == true)/////////////////////////////////////////////////////////////////////////////////////////////
                    {
                        if (playerStats.health <= 10)
                        {
                            enemyAttack += 2;
                        }
                        else
                        {
                            enemyAttack += 1;
                        }
                    }
                    if (EnemycoinList[i].RegenCoin == true)
                    {
                        enemyRegenCoin = true;
                    }
                }
                else if (EnemycoinList[i].isHeads == false)
                {
                    //EnemyitemList[i].GetComponent<Image>().color = new Color(1, 0, 0, 1);
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

/*
                 for (int i = 0; i < tempCoinsToDouble.Count; i++)
            {
                for (int x = 0; x < PlayercoinList.Count; x++)
                {
                    if (tempCoinsToDouble[i] == PlayercoinList[x])
                    {
                        Debug.Log(tempCoinsToDouble[i].itemName);
                        //tempCoinsToDouble.Remove(tempCoinsToDouble[i]);
                    }
                }
            }
*/
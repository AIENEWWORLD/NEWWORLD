using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[System.Serializable]
public class myAudioClass
{
	[Range(0,2)]
	public float audioPitchMin = 1;
	[Range(0,2)]
	public float audioPitchMax = 1;
    public string name;
    public AudioClip soundClip;
}

public class SetupFight : MonoBehaviour
{
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

    public GameObject PlayerSprite;

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

    //[HideInInspector]
    public bool playerinCombat = false;

    CreateInventory inventory;

    public float TimeBetweenCombat = 2.0f;
    public float TimeBeforeFlip = 2.0f;
    //[HideInInspector]
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
    public int enemybleedcoin = 0;
    public int bleedcoinCounter = 0;
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
    Animator PlayerAnims;
    public bool inList = false;
    public TutorialDisplayImages TutorialCanvas;
    [HideInInspector]
    public bool TutorialPlayed = false;

    public List<myAudioClass> sounds; // 0 attack,  1 block, 2 coinflip, 3 healing.
    public AudioSource WheretoPlaySounds;
    public float DelaySoundAfterStartingCombat = 0.5f;
    bool playedSound = false;

    bool pFlipSound = true;

    public int DealdmgGainHealthAttack;
    public List<int> dealdmggainhealthindx;

    Vector2 EnemyinitialXY = new Vector2(160, -102);
    Vector2 initialXY = new Vector2(-160, -102);
    Vector2 offsetXY = new Vector2(25, 50);

    public bool usePlacementobjects = false;
    public GameObject playerPlaceobj;
    public GameObject EnemyPlaceobj;
    public List<Vector2> playerplacements;
    public List<Vector2> enemyplacements;
    public GameObject compass;

    public int quickfix = 1;
	public int tempCursedint = 0;
    public List<CoinStats> tmpCoinList = new List<CoinStats>();

    void Start()
    {
        PlayerAnims = PlayerSprite.GetComponent<Animator>();
        playerStats = gameObject.GetComponent<StatsScript>();

        //PlayercoinList add this to inventory list.
        fightPanel = GameObject.FindGameObjectWithTag("FightPanel").transform;
        AddItm = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<AddItem>();

        mouseover = GameObject.FindGameObjectWithTag("MouseOverText");
        mouseover.SetActive(false);

        inventory = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<CreateInventory>();

        for (int i = 0; i < 5; i++)
        {
            playerplacements.Add(new Vector2(playerPlaceobj.transform.GetChild(i).transform.localPosition.x, playerPlaceobj.transform.GetChild(i).transform.localPosition.y));
        }
        for (int i = 0; i < 5; i++)
        {
            enemyplacements.Add(new Vector2(EnemyPlaceobj.transform.GetChild(i).transform.localPosition.x, EnemyPlaceobj.transform.GetChild(i).transform.localPosition.y));
        }
    }
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
            if (!TutorialPlayed)
            {
                TutorialCanvas.myCanvas.enabled = true;
            }
            onEnterCombat();
            enterCombat = false;
        }

        if (calcFight)
        {
            if (enemyStats != null)
            {
                calculateFight();

                applyFight();
                calcFight = false;
            }
        }

        screenmousePos = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mouseover.transform.position = new Vector3(screenmousePos.x + MouseOverTextOffset.x, screenmousePos.y + MouseOverTextOffset.y, mouseover.transform.position.z);

        if (picking)
        {

        }
        if (addedCounters < enemyCounterCursed) //two counters can use the same code because there will never be both bleed coins and cursed coins on a single enemy
        {
            int tempInt = enemyCounterCursed - addedCounters;
            for (int i = 0; i < tempInt; i++)
            {
                GameObject s = Instantiate(CounterPrefab) as GameObject;
                s.transform.SetParent(fightPanel);
                s.transform.localScale = new Vector3(1.620384f, 1, 1);
                s.transform.localPosition = new Vector3(CounterStart.x + (45 * quickfix), CounterStart.y, CounterStart.z);
                counters.Add(s);
                quickfix++;
                addedCounters = enemyCounterCursed;
            }
        }
        else if (addedCounters != 0 && enemyCounterCursed == 0 && bleedcoinCounter == 0)
        {
            clearCounters();
        }
        if (addedCounters < bleedcoinCounter)
        {
            int tempInt = bleedcoinCounter - addedCounters;

            for (int i = 0; i < tempInt; i++)
            {
                GameObject s = Instantiate(BleedPrefab) as GameObject;
                s.transform.SetParent(fightPanel);
                s.transform.localScale = new Vector3(1, 1, 1);
                s.transform.localPosition = new Vector3(CounterStart.x + (45 * quickfix), CounterStart.y, CounterStart.z);
                counters.Add(s);
                quickfix++;
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
        if (gameObject.GetComponent<ButtonsPressed>().InventoryButton.interactable == true)
        {
            if (gameObject.GetComponent<ButtonsPressed>().CanInventory && gameObject.GetComponent<ButtonsPressed>().clicktocontinue == false)
            {
                if (GameObject.FindGameObjectWithTag("FightCanvas").GetComponent<TutorialDisplayImages>() != null)
                {
                    GameObject.FindGameObjectWithTag("FightCanvas").GetComponent<TutorialDisplayImages>().currentFrame++;
                }
                inventory.greyout = true;
                enterCombat = true;
                inventoryisActive = !inventoryisActive;
            }
        }
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
        if (!playedSound)
            StartCoroutine(PlaySound());
        playedSound = true;
        playerinCombat = true;
        PlayerAnims.Play("PlayerIdle");
        EnemyAnims = enemySprite.GetComponent<Animator>();

        if (tmpCoinList.Count == 0)
        {
            //Debug.Log(tmpCoinList.Count);
            StatsScript SS = Enemy.GetComponent<StatsScript>();
            for (int i = 0; i < SS.coinList.Count; i++)
            {
                tmpCoinList.Add(SS.coinList[i]);
            }

        }

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
        if (compass != null)
        {
            compass.GetComponent<CompassScript>().Beasts.text = DiscoveryList.Count.ToString();
        }
        else
        {
            Debug.Log("attach compass");
        }
        inventory.greyoutUnselected();
        loadPlayerCoins();
        loadEnemyCoins();
    }
    public void onExitCombat()
    {
        if (tmpCoinList.Count != 0)
        {
            StatsScript SS = Enemy.GetComponent<StatsScript>();
            SS.coinList.Clear();
            for (int i = 0; i < tmpCoinList.Count; i++)
            {
                SS.coinList.Add(tmpCoinList[i]);
            }
            tmpCoinList.Clear();
        }
        combatStage = 0;
        clearPlayerCoins();
        clearEnemyCoins();
        clearCounters();
        cleartempcoins();
        playedSound = false;
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

        offsetPosX = initialXY.x;
        offsetPosY = initialXY.y;


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
                PlayercoinList.Add(new CoinStats("Click to select a coin", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none, false, false, true, false,CoinStats.TextureEnum.Atk));
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
            item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);                           //Set scale of player coins on canvas
            item.GetComponent<Button>().onClick.AddListener(() => {
                setInventory();
            });
            if (usePlacementobjects)
            {
                item.transform.localPosition = new Vector3(playerplacements[x].x, playerplacements[x].y, 0);
            }
            else
            {
                item.transform.localPosition = new Vector3(offsetPosX, offsetPosY, 0);
            }
            offsetPosX += offsetXY.x;

            if (x % 2 == 0)
            {
                offsetPosY += offsetXY.y;
            }
            else
            {
                offsetPosY -= offsetXY.y;
            }

            PlayeritemList[x].GetComponent<PlayerCoinsScript>().coin = PlayercoinList[x];
            PlayeritemList[x].name = "PlayerCoin: " + PlayeritemList[x].GetComponent<PlayerCoinsScript>().coin.itemName;
            num++;
        }
    }
    void loadEnemyCoins()
    {
        clearEnemyCoins();

        EnemyoffsetPosX = EnemyinitialXY.x;
        EnemyoffsetPosY = EnemyinitialXY.y;

        int num = 0;

        for (int x = 0; x < EnemycoinList.Count; x++)
        {
            EnemyCoinPrefab.GetComponent<EnemyCoinsScript>().itemNumber = num;
            GameObject item = Instantiate(EnemyCoinPrefab);
            EnemyitemList.Add(item);
            item.transform.SetParent(fightPanel);
            item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);                           //Set scale of enemy coins on canvas
            if (usePlacementobjects)
            {
                item.transform.localPosition = new Vector3(enemyplacements[x].x, enemyplacements[x].y, 0);
            }
            else
            {
                item.transform.localPosition = new Vector3(EnemyoffsetPosX, EnemyoffsetPosY, 0);
            }
            EnemyoffsetPosX -= offsetXY.x;

            if (x % 2 == 0)
            {
                EnemyoffsetPosY += offsetXY.y;
            }
            else
            {
                EnemyoffsetPosY -= offsetXY.y;
            }

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
        enemyAttack = 0; enemyDefence = 0; enemyHeal = 0; pFlipSound = true;
        enemyRegenCoin = false; duplicate = false;
		DealDmgGainHealthCoins = 0; enemybleedcoin = 0; tempCursedint = 0;
        dupeList.Clear(); dealdmggainhealthindx.Clear();

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
                    if (EnemycoinList[i].CurseCoin == true && pickCoinList.Count == 0)
                    {
						tempCursedint += 1;
                        //enemyCounterCursed += 1;
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
                        dealdmggainhealthindx.Add(i);
                    }
                    if (EnemycoinList[i].DealDmgDealDmg == true && pickCoinList.Count == 0)
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
        enemyStats.coinList.Add(new CoinStats(EnemycoinList[i].itemName,
            EnemycoinList[i].itemDescription, "", EnemycoinList[i].itemID,
            EnemycoinList[i].Heads_attack, EnemycoinList[i].Heads_defence,
            EnemycoinList[i].Heads_HP, EnemycoinList[i].Tails_attack,
            EnemycoinList[i].Tails_defence, EnemycoinList[i].Tails_HP,
			EnemycoinList[i].cType, EnemycoinList[i].ETypes, false, true, false, EnemycoinList[i].DuplicateCoin, EnemycoinList[i].myTexture));

		
        //Debug.Log("adding x");
    }
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
                if (playerStats.health <= 0)
                {
                    combatStage = 10;
                }

                if (playerAttacks)
                {
                    for (int i = 0; i < PlayercoinList.Count; i++)
                    {
                        PlayeritemList[i].GetComponent<PlayerCoinsScript>().spinrate = 20;
                    }

                }
                if (enemyAttacks)
                {
                    for (int i = 0; i < EnemycoinList.Count; i++)
                    {
                        EnemyitemList[i].GetComponent<EnemyCoinsScript>().spinrate = 20;
                    }
                }
                if (pFlipSound)
                {
					playClip(sounds[2].soundClip, sounds[2].audioPitchMin, sounds[2].audioPitchMax);
                    pFlipSound = false;
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
                if (enemyAttacks == true)
                {
                    for (int i = 0; i < EnemycoinList.Count; i++)
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


                if (pickCoinList.Count > 0)
                {
                    combatStage = 0;
                    if (pickCoinList[0].cType == CoinStats.coinTypes.flip)
                    {
                        Instructions.text = "Choose a coin to switch the result.";
                    }
                    if (pickCoinList[0].cType == CoinStats.coinTypes.secondChance)
                    {
                        Instructions.text = "Choose a coin to flip it again.";
                    }
                    if (pickCoinList[0].cType == CoinStats.coinTypes.Double)
                    {
                        Instructions.text = "Choose a coin to double its value.";
                    }
                    picking = true;
                }

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 4)
            {
				if (playerAttacks == true)
				{

					for (int i = 0; i < PlayercoinList.Count; i++)
					{
						PlayeritemList[i].GetComponent<PlayerCoinsScript>().spinrate = 5;
						PlayeritemList[i].GetComponent<PlayerCoinsScript>().flip = true;
					}
				}
				if (enemyAttacks == true)
				{
					for (int i = 0; i < EnemycoinList.Count; i++)
					{
						EnemyitemList[i].GetComponent<EnemyCoinsScript>().spinrate = 5;
						EnemyitemList[i].GetComponent<EnemyCoinsScript>().flip = true;
					}
				}
                if (playerAttacks)
                {
                    PlayerAnims.Play("PlayerAttack");

                }
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 5)
            {

                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                Instructions.text = "";
                setColoursHT();

                if (playerAttacks)
					playClip(sounds[0].soundClip, sounds[0].audioPitchMin, sounds[0].audioPitchMax);

                //if (enemyDefence > 0)
                //{
                //    EnemyNumbers.color = new Color(192, 192, 192);
                //    EnemyNumbers.text = enemyDefence.ToString();
                //}
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 6)// enemy + player heal
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                if (playerStats.health <= 0)
                {
                    combatStage = 10;
                }

                if (playerAttacks)
                {
                    if (enemyDefence > 0)
                    {
						playClip(sounds[1].soundClip, sounds[1].audioPitchMin, sounds[1].audioPitchMax);
                    }
                    else
                    {
						playClip(sounds[4].soundClip, sounds[4].audioPitchMin, sounds[4].audioPitchMax);
                    }
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
					if (enemyHeal < 0)
					{
						playerAttack += -enemyHeal;
					}
                    EnemyNumbers.text = "" + playerAttack.ToString();
                    EnemySlider.value = enemyStats.health;

                }

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 7)//enemy attacks + player defends
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";

                if (enemyStats.health <= 0 && !enemyRegenCoin)
                {
                    combatStage = 10;
                }

                if (enemyAttacks)
                {
                    EnemyAnims.Play("Attack");
					playClip(sounds[0].soundClip, sounds[0].audioPitchMin, sounds[0].audioPitchMax);
                }
                //if (playerDefence > 0)
                //{
                //    PlayerNumbers.color = new Color(192, 192, 192); //silver
                //    PlayerNumbers.text = playerDefence.ToString();
                //}

                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 8)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                //PlayerAnims.Play("PlayerIdle");
                if (enemyAttacks)
                {


                    if (DealDmgGainHealthCoins != 0)
                    {
                        DealDmgGainHealthCoins -= playerDefence;
                        if (DealDmgGainHealthCoins > 0)
                        {
                            for (int i = 0; i < dealdmggainhealthindx.Count; i++)
                            {
                                //enemyHeal += DealDmgGainHealthCoins * DealdmgGainHealthAttack;

                                enemyHeal += EnemycoinList[dealdmggainhealthindx[i]].Heads_attack;

                                //enemyHeal += EnemycoinList[dealdmggainhealthindx[i]].DealdmgGainHealthAttack;
                            }
                        }
                    }
					enemybleedcoin -= playerDefence;
                    if (enemybleedcoin >= 0)
                    {
                        bleedcoinCounter += enemybleedcoin;
                        //Debug.Log(bleedcoinCounter);
                        if (bleedcoinCounter >= 5)
                        {
                            enemyAttack += 3;
                            bleedcoinCounter = 0;
                            quickfix = 1;
                        }
                    }
					if (tempCursedint >= 0) 
					{
						enemyCounterCursed += tempCursedint;
					}
                    if (playerDefence > 0)
                    {
						playClip(sounds[1].soundClip, sounds[1].audioPitchMin, sounds[1].audioPitchMax);
                    }
                    else
                    {
						playClip(sounds[4].soundClip, sounds[4].audioPitchMin, sounds[4].audioPitchMax);
                    }
                    enemyAttack -= playerDefence;
                    if (0 > enemyAttack)
                    {
                        enemyAttack = 0;
                    }
                    if (enemyCounterCursed >= 5)
                    {
                        enemyAttack = (int)playerStats.health;
                        //playerStats.health /= 2;
                        enemyCounterCursed = 0;
                    }
                    playerStats.health = playerStats.health - enemyAttack;
					if (playerHeal < 0)
					{
						enemyAttack += -playerHeal;
					}

                    PlayerNumbers.color = Color.red;
                    PlayerNumbers.text = "" + enemyAttack.ToString();
                    playerSlider.value = playerStats.health;
                }
                StartCoroutine(PlayerCombat(TimeBetweenCombat));
            }
            if (combatStage == 9)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                if (enemyStats.health <= 0 && !enemyRegenCoin)
                {
                    combatStage = 10;
                }
                else if (enemyRegenCoin && enemyStats.health <= 0)
                {
                    enemyHeal = (int)enemyStats.maxHealth / 2;
                }

                PlayerNumbers.color = Color.green;
                PlayerNumbers.text = "";
                playerStats.health = playerStats.health + playerHeal;
                if (playerHeal > 0)
                {
                    PlayerNumbers.text = "" + playerHeal.ToString();
					playClip(sounds[3].soundClip, sounds[3].audioPitchMin, sounds[3].audioPitchMax);
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
					playClip(sounds[3].soundClip, sounds[3].audioPitchMin, sounds[3].audioPitchMax);
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
            if (combatStage == 10)
            {
                PlayerNumbers.text = "";
                EnemyNumbers.text = "";
                //RECHECK DEATH since players can heal negative values (risky coins)

                pFlipSound = true;

                Instructions.text = "Press ATTACK or click on a coin slot to change your active coins.";

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
                if (enemyStats.health <= 0)
                {
                    //do something about death too
                    enemyStats.health = 0;
                    if (gameObject.GetComponent<EnemyDropCoins>().dead == false)
                    {
                        if (tmpCoinList.Count != 0)
                        {
                            StatsScript SS = Enemy.GetComponent<StatsScript>();
                            SS.coinList.Clear();
                            for (int i = 0; i < tmpCoinList.Count; i++)
                            {
                                SS.coinList.Add(tmpCoinList[i]);
                            }
                            tmpCoinList.Clear();
                        }
                        //Debug.Log(tmpCoinList.Count);
                        gameObject.GetComponent<EnemyDropCoins>().onKilled(enemyStats.Monster, enemyStats.gold, enemyStats.dropRate);
                    }
                }
                if (playerStats.health <= 0)
                {
                    if (tmpCoinList.Count != 0)
                    {
                        StatsScript SS = Enemy.GetComponent<StatsScript>();
                        SS.coinList.Clear();
                        for (int i = 0; i < tmpCoinList.Count; i++)
                        {
                            SS.coinList.Add(tmpCoinList[i]);
                        }
                        tmpCoinList.Clear();
                    }
                    //Debug.Log(tmpCoinList.Count);
                    playerStats.health = 0;
                    gameObject.GetComponent<OnWinLose>().CheckDeath(false, new CoinStats("", "", "", 0, 0, 0, 0, 0, 0, 0, CoinStats.coinTypes.standard, CoinStats.EnemycoinTypes.none, false, false, false, false,CoinStats.TextureEnum.Atk), 0);
                    //playerStats.dead = true;
                    GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<RespawnPlayer>().FindNearestRespawn();
                    //do something about death too
                    gameObject.GetComponent<ButtonsPressed>().endcombat();
                    //combatStage = 10;
                }
                combatStage = 0;
            }
        }
        else
        {
            combatStage = 0;
            PlayerNumbers.text = "";
            EnemyNumbers.text = "";
            Instructions.text = "Press ATTACK or click on a coin slot to change your active coins.";
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
			if (coinGameObject.GetComponent<PlayerCoinsScript> () != null) 
			{
				coinGameObject.GetComponent<PlayerCoinsScript>().flip = false;
				coinGameObject.GetComponent<PlayerCoinsScript> ().spinrate = 20;
			} 
			else if (coinGameObject.GetComponent<EnemyCoinsScript> () != null) 
			{
				coinGameObject.GetComponent<EnemyCoinsScript>().flip = false;
				coinGameObject.GetComponent<EnemyCoinsScript>().spinrate = 20;
			}
			//PlayeritemList[i].GetComponent<PlayerCoinsScript>().spinrate = 20;
			//next compat stage set it back to 5, set flip to true.
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
    public void calculateFight2()
    {
		playerAttack = 0; playerDefence = 0; playerHeal = 0; familyCounter = 0;
		enemyAttack = 0; enemyDefence = 0; enemyHeal = 0; pFlipSound = true;
		enemyRegenCoin = false; duplicate = false;
		DealDmgGainHealthCoins = 0; enemybleedcoin = 0; tempCursedint = 0;
		dupeList.Clear(); dealdmggainhealthindx.Clear();

		//tempCoinsToDouble.Clear();

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
					if (EnemycoinList[i].CurseCoin == true && pickCoinList.Count == 0)
					{
						tempCursedint += 1;
						//enemyCounterCursed += 1;
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
						dealdmggainhealthindx.Add(i);
					}
					if (EnemycoinList[i].DealDmgDealDmg == true && pickCoinList.Count == 0)
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
    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(DelaySoundAfterStartingCombat);
        if (enemyStats.MySound != null)
        {
			
            playClip(enemyStats.MySound, 1, 1);
        }
        else
        {
            Debug.Log("attach my sound");
        }
    }
	void playClip(AudioClip clip, float pMin, float pMax)
    {
        WheretoPlaySounds.clip = clip;
		WheretoPlaySounds.pitch = UnityEngine.Random.Range(pMin, pMax);
        WheretoPlaySounds.Play();
    }
    public void cancelAudio()
    {
        //StopAllCoroutines();
        gameObject.GetComponent<SetupFight>().WheretoPlaySounds.Stop();
    }
}
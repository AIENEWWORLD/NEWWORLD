using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatsScript : MonoBehaviour
{

    //save pretty much this entire script

    public string Name;
    public float maxHealth;
    public float maxSupply;
    public float health;
    public int gold; //how much gold the player has / how much gold you will recieve for defeating
    public float supplies;
    public int totalCoins; //how many coins can be selected.
    public int dropRate = 50;//the drop rate of the coins
    public enumType guyType;
    public Enemy Monster;
    public int Level;

    //for UI
    public List<CoinStats> coinList = new List<CoinStats>();
    //[HideInInspector]
    public Vector2 UIpos; //the position of the sprite on the prefab. Please make z 0
    public Vector3 UIrotation;
    public Vector3 Scale;
    public int index; //we will compare this to the enemy we run into to spawn it.

    private GameObject FightCamera;
    private GameObject FightPanel;

    public GameObject prefab;

    public bool dead = false;
    Transform playertrans;
    RespawnEnemies respawnscript;
    //[HideInInspector]
    public Vector3 startpos;
    //[HideInInspector]
    public bool counting;

    public AudioClip MySound;


    public enum enumType
    {
        boss,//REMEMBER TO SET THIS ON THE FLEE BUTTON PROPERLY SO THAT CANT FLEE FROM BOSSES
        Enemy,
        player,
    }
    public enum Enemy
    {
        pimpkin,
        tortoise_common,
        tortoise_boss,
        thunderbird_boss,
        Octopie_boss,
        Goat_M,
        Goat_F,
        Dolphin_M,
        Dolphin_F,
        Deer_M,
        Deer_F,
        none,
    }

    void Start()
    {
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
        FightPanel = GameObject.FindGameObjectWithTag("FightPanel");
        if (guyType != enumType.player)
        {
            playertrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        respawnscript = GameObject.FindGameObjectWithTag("EnemyRespawner").GetComponent<RespawnEnemies>();
        startpos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void Update()
    {
    }

    void setScript(StatsScript s)
    {
        Name = s.Name;
        maxHealth = s.maxHealth;
        health = s.health;
        gold = s.gold;
        totalCoins = s.totalCoins;
        dropRate = s.dropRate;
        guyType = s.guyType;
        Monster = s.Monster;
        coinList = s.coinList;
        UIpos = s.UIpos;
        Scale = s.Scale;
        UIrotation = s.UIrotation;
        index = s.index;
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
        FightPanel = GameObject.FindGameObjectWithTag("FightPanel");
        prefab = s.prefab;
        MySound = s.MySound;
    }

    void OnCollisionStay(Collision collision) //using oncollisionstay because oncollisionenter bugs out when its already colliding with the player
    {
        if (guyType != enumType.player && !dead)
        {
            if (collision.gameObject.tag == "Player" && GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().playerinCombat == false)
            {
                //Debug.Log("entering combat");
                if (FightCamera != null)
                {
                    //FightCamera.GetComponent<Camera>().enabled = true;/////////////////////////////////////////////////////////////////////////////////////////
                    Transitions T = GameObject.FindGameObjectWithTag("checkCombat").GetComponent<Transitions>();
                    T.TransCam = FightCamera.GetComponent<Camera>();
                    T.trans = true;

                    FightCamera.GetComponent<SetupFight>().setEnemyList(coinList);

                    FightCamera.GetComponent<SetupFight>().enterCombat = true;
                }
                else
                {
                    Debug.Log("error in stats script");
                }

                if (FightPanel != null)
                {

                    GameObject sprite = Instantiate(prefab);
                    sprite.AddComponent<StatsScript>();
                    sprite.GetComponent<StatsScript>().setScript(this);
                    FightCamera.GetComponent<SetupFight>().enemyStats = sprite.GetComponent<StatsScript>(); //////////////////////////////////////////////////this could be dodgy
                    FightCamera.GetComponent<SetupFight>().enemySprite = sprite;
                    sprite.transform.SetParent(FightPanel.transform);
                    //sprite.transform.localScale = new Vector3(1, 1, 1);
                    sprite.transform.localPosition = new Vector3(UIpos.x, UIpos.y, -94.3f);
                    sprite.transform.localEulerAngles = new Vector3(UIrotation.x, UIrotation.y, UIrotation.z);
                    sprite.transform.localScale = new Vector3(Scale.x,Scale.y,Scale.z);
                }
                else
                {
                    Debug.Log("error in stats script");
                }
                //Destroy(gameObject);
                dead = true;
                transform.position = startpos;
                if (guyType != enumType.boss)
                {
                    respawnscript.EnemyList.Add(gameObject);
                }
                FightCamera.GetComponent<SetupFight>().Enemy = gameObject;
                //gameObject.SetActive(false);
                //gameObject.GetComponent<EnemyScript>().enabled = false;
                if (transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.name = Name + " dead";
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().playerinCombat = true;
            }
        }
    }

    public IEnumerator Respawn(float time, int i)
    {
        //Debug.Log("HI");
        counting = true;
        yield return new WaitForSeconds(time);
        //respawnscript.EnemyList[i].SetActive(true);
        //gameObject.GetComponent<EnemyScript>().enabled = true;
        gameObject.transform.position = gameObject.GetComponent<EnemyAI>().myPos;
        if (transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>() != null)
        {
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        gameObject.GetComponent<Collider>().enabled = true;
        respawnscript.EnemyList.Remove(gameObject);
        dead = false;
        gameObject.name = Name + " alive";
        counting = false;
    }

    public void RespawnEnemy(float time, int i)
    {
        StartCoroutine(Respawn(time, i));
        //Debug.Log("HI2");
    }

    public void CancelRespawn()
    {
        StopAllCoroutines();
    }

        //void OnCollisionEnter(Collision collision)
        //{
        //    if (guyType != enumType.player)
        //    {
        //        if (collision.gameObject.tag == "Player" && GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().playerinCombat == false)
        //        {
        //            if (FightCamera != null)
        //            {
        //                FightCamera.GetComponent<Camera>().enabled = true;
        //
        //                FightCamera.GetComponent<SetupFight>().setEnemyList(coinList);
        //                FightCamera.GetComponent<SetupFight>().enterCombat = true;
        //            }
        //            else
        //            {
        //                Debug.Log("error in stats script");
        //            }
        //
        //            if (FightPanel != null)
        //            {
        //
        //                GameObject sprite = Instantiate(prefab);
        //                sprite.AddComponent<StatsScript>();
        //                sprite.GetComponent<StatsScript>().setScript(this);
        //                FightCamera.GetComponent<SetupFight>().enemyStats = sprite.GetComponent<StatsScript>(); //////////////////////////////////////////////////this could be dodgy
        //                sprite.transform.SetParent(FightPanel.transform);
        //                //sprite.transform.localScale = new Vector3(1, 1, 1);
        //                sprite.transform.localPosition = new Vector3(UIpos.x, UIpos.y, -15);
        //                sprite.transform.localEulerAngles = new Vector3(UIrotation.x, UIrotation.y, UIrotation.z);
        //            }
        //            else
        //            {
        //                Debug.Log("error in stats script");
        //            }
        //            Destroy(gameObject);
        //
        //        }
        //    }
        //}
    }

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatsScript : MonoBehaviour
{

    //save pretty much this entire script

    public string Name;
    public int maxHealth;
    public int health;
    public int gold; //how much gold the player has / how much gold you will recieve for defeating
    public int supplies;
    public int totalCoins; //how many coins can be selected.
    public int dropRate = 50;//the drop rate of the coins
    public enumType guyType;

    //for UI
    public List<CoinStats> coinList = new List<CoinStats>();
    public Vector2 UIpos; //the position of the sprite on the prefab. Please make z 0
    public Vector3 UIrotation;
    public int index; //we will compare this to the enemy we run into to spawn it.

    private GameObject FightCamera;
    private GameObject FightPanel;

    public GameObject prefab;

    public enum enumType
    {
        boss,//REMEMBER TO SET THIS ON THE FLEE BUTTON PROPERLY SO THAT CANT FLEE FROM BOSSES
        pimpkin,
        bird,
        player,
    }

	void Start ()
    {
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
        FightPanel = GameObject.FindGameObjectWithTag("FightPanel");
	}
	
	void Update ()
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
        coinList = s.coinList;
        UIpos = s.UIpos;
        UIrotation = s.UIrotation;
        index = s.index;
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
        FightPanel = GameObject.FindGameObjectWithTag("FightPanel");
        prefab = s.prefab;
    }

    void OnCollisionStay(Collision collision) //using oncollisionstay because oncollisionenter bugs out when its already colliding with the player
    {
        if (guyType != enumType.player)
        {
            if (collision.gameObject.tag == "Player" && GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().playerinCombat == false)
            {
                if (FightCamera != null)
                {
                    FightCamera.GetComponent<Camera>().enabled = true;

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
                    sprite.transform.SetParent(FightPanel.transform);
                    //sprite.transform.localScale = new Vector3(1, 1, 1);
                    sprite.transform.localPosition = new Vector3(UIpos.x, UIpos.y, -15);
                    sprite.transform.localEulerAngles = new Vector3(UIrotation.x, UIrotation.y, UIrotation.z);
                }
                else
                {
                    Debug.Log("error in stats script");
                }
                Destroy(gameObject);

            }
        }
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

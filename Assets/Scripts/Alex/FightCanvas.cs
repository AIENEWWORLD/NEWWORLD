using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class coins
{
    public GameObject attackCoin;
    public GameObject defendCoin;
    public bool ACheads = false;
    public bool DCheads = false;
}

public class FightCanvas : MonoBehaviour
{
    //display the text box on mouseover
    public Vector2 TextOffset;
    public Vector3 screenmousePos;
    public GameObject coinDisplay;
    public Text mouseoverText;
    private bool button = false;

    public Material m_Heads;
    public Material m_Tails;
    public Material m_Default;
    public int chance = 50;//chance out of 100 to get heads
    public List<coins> P_Coins;
    public List<coins> E_Coins;
    //get enemy stats, coins etc from enemy stats script

    public StatsScript enemy;
    public StatsScript player;

    public int p_heads;
    public int e_heads;

    public Slider EnemyHPSlider;
    public Slider PlayerHPSlider;

    public bool runFlip = false;
    public bool playerAttacking = true;

    public bool swapplayerweapons = false;
    public int newAttackCoins = 0;
    public int newDefenceCoins = 0;

    void Start ()
    {
        button = false;
        setupBattle();
	}
	void Update ()
    {


        updateUI();

        if(playerAttacking == true && runFlip)
        {
            FlipPlayerAttacking();
            applyDamagetoEnemy();
            runFlip = false;
            playerAttacking = false;
        }
        if (playerAttacking == false && runFlip)
        {
            flipEnemyAttacking();
            applyDamagetoPlayer();
            runFlip = false;
            playerAttacking = true;
        }
    }
    //random chance for coin flipping
    bool getRandom()
    {
        runFlip = false;
        return (Random.Range(0, 100) > 100-chance);
    }
    //set up the total coins from the stats
    void setupBattle()
    {
        enemy.health = enemy.maxHealth;
        for (int i = 0; i < P_Coins.Count; i++)
        {
            if (i > player.attack_coins-1)
            {
                P_Coins[i].attackCoin.gameObject.SetActive(false);
            }
            if (i > player.defence_coins - 1)
            {
                P_Coins[i].defendCoin.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < E_Coins.Count; i++)
        {
            if (i > enemy.attack_coins-1)
            {
                E_Coins[i].attackCoin.gameObject.SetActive(false);
            }
            if (i > enemy.defence_coins - 1)
            {
                E_Coins[i].defendCoin.gameObject.SetActive(false);
            }
        }
    }
    void FlipPlayerAttacking()
    {
        p_heads = 0;
        e_heads = 0;
        for (int i = 0; i < player.attack_coins; i++)
        {
            if (getRandom() == true)
            {
                p_heads += 1;
                P_Coins[i].ACheads = true;
                P_Coins[i].attackCoin.gameObject.GetComponent<Renderer>().material = m_Heads;
            }
            else
            {
                P_Coins[i].ACheads = false;
                P_Coins[i].attackCoin.gameObject.GetComponent<Renderer>().material = m_Tails;
            }
        }
        for (int i = 0; i < enemy.defence_coins; i++)
        {
            if (getRandom() == true)
            {
                e_heads += 1;
                E_Coins[i].DCheads = true;
                E_Coins[i].defendCoin.gameObject.GetComponent<Renderer>().material = m_Heads;
            }
            else
            {
                E_Coins[i].DCheads = false;
                E_Coins[i].defendCoin.gameObject.GetComponent<Renderer>().material = m_Tails;
            }
        }
    }
    void flipEnemyAttacking()
    {
        p_heads = 0;
        e_heads = 0;
        //player is attacking, enemy defending.
        

        //Enemy is attacking, player defending.
        for (int i = 0; i < enemy.attack_coins; i++)
        {
            if (getRandom() == true)
            {
                e_heads += 1;
                E_Coins[i].ACheads = true;
                E_Coins[i].attackCoin.gameObject.GetComponent<Renderer>().material = m_Heads;
            }
            else
            {
                E_Coins[i].ACheads = false;
                E_Coins[i].attackCoin.gameObject.GetComponent<Renderer>().material = m_Tails;
            }
        }
        for (int i = 0; i < player.defence_coins; i++)
        {
            if (getRandom() == true)
            {
                p_heads += 1;
                P_Coins[i].DCheads = true;
                P_Coins[i].defendCoin.gameObject.GetComponent<Renderer>().material = m_Heads;
            }
            else
            {
                P_Coins[i].DCheads = false;
                P_Coins[i].defendCoin.gameObject.GetComponent<Renderer>().material = m_Tails;
            }
        }
    }
    void applyDamagetoEnemy()
    {
        if (p_heads > e_heads)
        {
            enemy.health -= p_heads;
            EnemyHPSlider.maxValue = enemy.maxHealth;
            EnemyHPSlider.value = enemy.health;
        }
        else
        {

        }
    }
    void applyDamagetoPlayer()
    {
        if (e_heads > p_heads)
        {
            player.health -= e_heads;
            PlayerHPSlider.maxValue = player.maxHealth;
            PlayerHPSlider.value = player.health;
        }
        else
        {

        }
    }
    void swapWeapons()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < newAttackCoins)
            {
                P_Coins[i].attackCoin.gameObject.SetActive(true);
            }
            else
            {
                P_Coins[i].attackCoin.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (i < newDefenceCoins)
            {
                P_Coins[i].defendCoin.gameObject.SetActive(true);
            }
            else
            {
                P_Coins[i].defendCoin.gameObject.SetActive(false);
            }
        }
        player.attack_coins = newAttackCoins;
        player.defence_coins = newDefenceCoins;
    }



    //button stuff

    //mouse over coin stuff
    void updateUI()
    {
        if (button)
        {
            screenmousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            coinDisplay.transform.position = new Vector3(screenmousePos.x + TextOffset.x, screenmousePos.y + TextOffset.y, coinDisplay.transform.position.z);
        }
        else
        {
            coinDisplay.SetActive(false);
        }
    }
    public void mouseoverCoin(string s)
    {
        mouseoverText.text = s;
        coinDisplay.SetActive(true);
        button = true;
    }
    public void mouseExitCoin()
    {
        button = false;
    }
    public void flipClicked()
    {
        //have a check so it's not so spammable?
        runFlip = !runFlip;
    }
    public void swapWeaponsClicked()
    {
        swapWeapons();
    }
}

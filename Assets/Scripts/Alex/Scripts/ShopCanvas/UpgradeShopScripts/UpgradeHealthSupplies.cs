using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeHealthSupplies : MonoBehaviour
{
    public List<GameObject> supplyList;
    public List<GameObject> healList;

    public List<UpgradeHealthSupplies1> supplyScriptList;
    public List<UpgradeHealthSupplies1> healthScriptList;

    int currsupplyID = 0;
    UpgradeHealthSupplies1 currsupplyUHS;
    int currhealthID = 0;
    UpgradeHealthSupplies1 currhealthUHS;

    public UpgradeButtonScripts UBSScripts;

    public Text goldText;

    public AudioClip PurchaseSound;
    public AudioSource WhereToPlay;

    void Start ()
    {
        for (int i = 0; i < healList.Count; i++)
        {
            healthScriptList.Add(healList[i].GetComponent<UpgradeHealthSupplies1>());
            healList[i].transform.GetChild(0).GetComponent<Text>().text = "Click to buy " + healList[i].GetComponent<UpgradeHealthSupplies1>().ExtraHealth + " health for " + healList[i].GetComponent<UpgradeHealthSupplies1>().cost;
        }
        for (int i = 0; i < supplyList.Count; i++)
        {
            supplyScriptList.Add(supplyList[i].GetComponent<UpgradeHealthSupplies1>());
            supplyList[i].transform.GetChild(0).GetComponent<Text>().text = "Click to buy " + supplyList[i].GetComponent<UpgradeHealthSupplies1>().ExtraSupplies + " supplies for \n" + supplyList[i].GetComponent<UpgradeHealthSupplies1>().cost;
        }

        currsupplyUHS = supplyScriptList[0];
        currhealthUHS = healthScriptList[0];

        SetInteractableList(0, supplyList);
        SetInteractableList(0, healList);
    }

    void Update()
    {
        currsupplyUHS = supplyScriptList[currsupplyID];
        currhealthUHS = healthScriptList[currhealthID];
        goldText.text = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold.ToString() + " Gold";
        if (currsupplyUHS.cost <= GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold)
        {
            supplyList[currsupplyID].GetComponent<Button>().interactable = true;
            supplyList[currsupplyID].transform.GetChild(0).GetComponent<Text>().text = "Click to buy " + supplyList[currsupplyID].GetComponent<UpgradeHealthSupplies1>().ExtraSupplies + " supplies for \n" + supplyList[currsupplyID].GetComponent<UpgradeHealthSupplies1>().cost;
        }
        else
        {
            supplyList[currsupplyID].GetComponent<Button>().interactable = false;
            supplyList[currsupplyID].transform.GetChild(0).GetComponent<Text>().text = "can't afford " + supplyList[currsupplyID].GetComponent<UpgradeHealthSupplies1>().cost + " gold";
        }

        if (currhealthUHS.cost <= GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold)
        {
            healList[currhealthID].GetComponent<Button>().interactable = true;
            healList[currhealthID].transform.GetChild(0).GetComponent<Text>().text = "Click to buy " + healList[currhealthID].GetComponent<UpgradeHealthSupplies1>().ExtraHealth + " health for " + healList[currhealthID].GetComponent<UpgradeHealthSupplies1>().cost;
        }
        else
        {
            healList[currhealthID].GetComponent<Button>().interactable = false;
            healList[currhealthID].transform.GetChild(0).GetComponent<Text>().text = "can't afford " + healList[currhealthID].GetComponent<UpgradeHealthSupplies1>().cost + " gold";
        }
    }

    public void ClickHealth(Button b)
    {
        currhealthUHS = b.GetComponent<UpgradeHealthSupplies1>();
        for (int i = 0; i < healList.Count; i++)
        {
            if (currhealthUHS.id == i)
            {
                WhereToPlay.PlayOneShot(PurchaseSound);
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold -= currhealthUHS.cost;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().maxHealth += currhealthUHS.ExtraHealth;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().health = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().maxHealth;
                healList[i].GetComponent<Button>().interactable = false;
                healList[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
                if (i + 1 < healList.Count)
                {
                    currhealthID = currhealthUHS.id + 1;
                    
                    //SetInteractableList(UHS.id + 1, healList);
                }
                else
                {
                    healList[i].GetComponent<Button>().interactable = false;
                    healList[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
                }
            }
        }
        UBSScripts._checkGold();
    }
    public void ClickSupplies(Button b)
    {
        currsupplyUHS = b.GetComponent<UpgradeHealthSupplies1>();
        for (int i = 0; i < supplyList.Count; i++)
        {
            if (currsupplyUHS.id == i)
            {
                WhereToPlay.PlayOneShot(PurchaseSound);
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold -= currsupplyUHS.cost;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().maxSupply += currsupplyUHS.ExtraSupplies;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().supplies = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().maxSupply;
                supplyList[i].GetComponent<Button>().interactable = false;
                supplyList[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
                if (i + 1 < supplyList.Count)
                {
                    currsupplyID = currsupplyUHS.id + 1;
                    
                    //SetInteractableList(UHS.id + 1, supplyList);
                }
                else
                {
                    supplyList[i].GetComponent<Button>().interactable = false;
                    supplyList[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
                }
            }
        }
        UBSScripts._checkGold();
    }


    void SetInteractableList(int id, List<GameObject> s)
    {
        for (int i = 0; i < s.Count; i++)
        {
            
            if (i == id)
            {
                s[i].GetComponent<Button>().interactable = true;
                
            }
            else
            {
                s[i].GetComponent<Button>().interactable = false;
                s[i].transform.GetChild(0).GetComponent<Text>().text = "Buy previous upgrade";
            }
        }
    }
}

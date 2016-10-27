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
        goldText.text = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold.ToString() + " Gold";
        if (currsupplyUHS.cost <= GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold)
        {
            supplyList[currsupplyID].GetComponent<Button>().interactable = true;
        }
        else
        {
            supplyList[currsupplyID].GetComponent<Button>().interactable = false;
        }

        if (currhealthUHS.cost <= GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold)
        {
            healList[currhealthID].GetComponent<Button>().interactable = true;
        }
        else
        {
            healList[currhealthID].GetComponent<Button>().interactable = false;
        }
    }

    public void ClickHealth(Button b)
    {
        currhealthUHS = b.GetComponent<UpgradeHealthSupplies1>();
        for (int i = 0; i < healList.Count; i++)
        {
            if (currhealthUHS.id == i)
            {
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold -= currhealthUHS.cost;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().maxHealth += currhealthUHS.ExtraHealth;
                healList[i].GetComponent<Button>().interactable = false;
                if (i + 1 < healList.Count)
                {
                    currhealthID = currhealthUHS.id + 1;
                    //SetInteractableList(UHS.id + 1, healList);
                }
                else
                {
                    healList[i].GetComponent<Button>().interactable = false;
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
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().gold -= currsupplyUHS.cost;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().maxSupply += currsupplyUHS.ExtraSupplies;
                supplyList[i].GetComponent<Button>().interactable = false;
                if (i + 1 < supplyList.Count)
                {
                    currsupplyID = currsupplyUHS.id + 1;
                    //SetInteractableList(UHS.id + 1, supplyList);
                }
                else
                {
                    supplyList[i].GetComponent<Button>().interactable = false;
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
            }
        }
    }
}

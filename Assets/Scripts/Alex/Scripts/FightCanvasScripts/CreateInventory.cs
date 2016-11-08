using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreateInventory : MonoBehaviour
{
    public List<GameObject> itemList = new List<GameObject>(); //save this
    public List<CoinStats> coinList = new List<CoinStats>(); //save this
    public GameObject Items;
    int _x = -260; int _y = 180;

    public bool greyout = true;

    AddItem addI;

    public float alphaCoins = 0.5f;

    // Use this for initialization
    void Start ()
    {

        addI = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<AddItem>();

        reAddItems();

        greyoutUnselected();
    }

    public void addItem(int id)
    {
        for (int i = 0; i < addI.coins.Count; i++)
        {
            if (addI.coins[i].itemID == id)
            {
                CoinStats item = addI.coins[i];

                for (int x = 0; x < coinList.Count; i++)
                {
                    if (coinList[i].itemName == null)
                    {
                        coinList[i] = item;
                        break;
                    }
                }

                break;
            }
        }
    }

    public void reAddItems()
    {
        coinList.Clear();
        itemList.Clear();
        int num = 0;

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                coinScript cs = Items.GetComponent<coinScript>();
                cs.itemNumber = num;
                GameObject item = Instantiate(Items);
                if(cs.coin.itemName != null)
                {
                    cs.coin.notEmpty = true;
                }
                else
                {
                    cs.coin.notEmpty = false;
                }
                itemList.Add(item);
                coinList.Add(new CoinStats());
                item.transform.SetParent(gameObject.transform);
                item.transform.localScale = new Vector3(1, 1, 1);
                item.transform.localPosition = new Vector3(_x, _y, 0);
                _x += 60;
                if (y == 9)
                {
                    _x = -260;
                    _y -= 60;
                }
                num++;
            }
        }

        for (int i = 0; i < addI.coins.Count; i++)
        {
            addItem(addI.coins[i].itemID);
            coinList[i] = addI.coins[i];
            itemList[i].GetComponent<coinScript>().coin = addI.coins[i];
            

        }
    }

    public void fullReset(int x, int y)
    {
        for(int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
        _x = x;
        _y = y;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(greyout)
        {
            greyoutUnselected();
            greyout = false;
        }
	}

    public void greyoutUnselected()
    {
        int num = 0;
        for (int i = 0; i < coinList.Count; i++)
        {
            if (coinList[i].isSelected == true)
            {
                //itemList[i].GetComponent<Image>().color = new Color(0, 0, 1, 1);
                num += 1;
            }
            else
            {
                itemList[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
        for (int x = 0; x < itemList.Count; x++)
        {
           // if (coinList[x].isSelected != true)
           // {
           //     itemList[x].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
           //     itemList[x].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
           //     //Debug.Log("h");
           // }
           // else
           // {
           //     itemList[x].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
           // }

            if (num >= GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>().totalCoins)
            {
                if (coinList[x].isSelected != true)
                {
                    itemList[x].GetComponent<Image>().color = new Color(1, 1, 1, alphaCoins);
                    itemList[x].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    //Debug.Log("h");
                }
                else
                {
                    itemList[x].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }
            else
            {
               
            }
        }
    }
}

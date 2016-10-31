using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class coinScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    Image itemImage;
    public int itemNumber;
    CreateInventory inv;

    public float spinrate = 0.25f;
    public GameObject myCoin;


    GameObject mouseover;

    Text mouseoverTextDesc1;
    Text mouseoverTextDesc2;
    // Use this for initialization
    void Start ()
    {
        myCoin.SetActive(false);
        inv = GameObject.FindGameObjectWithTag("ItemDatabase").GetComponent<CreateInventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();

        myCoin.transform.Rotate(0, Random.Range(0, 100), 0);
        myCoin.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = coin.GetTexture();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (itemImage.sprite != null)
        {
            myCoin.SetActive(true);

        }
        else
        {
            myCoin.SetActive(false);
        }
        if (coin.isSelected)
        {
            myCoin.transform.Rotate(0, (spinrate), 0);
        }
        if (inv.coinList[itemNumber].itemName != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = inv.coinList[itemNumber].Icon;
        }
        else
        {
            itemImage.enabled = false;
        }
	}

    public void OnPointerDown(PointerEventData data)
    {
        if (coin.isSelected == true)
        {
            coin.isSelected = false;
        }
        else
        {
            inv.greyout = true;
            GameObject fCam = GameObject.FindGameObjectWithTag("FightCamera");
            if (fCam != null)
            {
                int totalselectedcoins = 0;
                for (int i = 0; i < fCam.GetComponent<AddItem>().coins.Count; i++)
                {
                    if (fCam.GetComponent<AddItem>().coins[i].isSelected == true)
                    {
                        totalselectedcoins++;
                    }
                }
                if (totalselectedcoins < fCam.GetComponent<StatsScript>().totalCoins)
                {
                    coin.isSelected = !coin.isSelected;
                }
            }
        }
        inv.greyoutUnselected();
        //if the amount of selected coins is smaller than the playerstats upgrade amount
    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseover = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().mouseover;
        if (coin.itemName.CompareTo("") != 0)
        {
            mouseover.SetActive(true);
            mouseoverTextDesc1 = GameObject.FindGameObjectWithTag("Desc1").GetComponent<Text>();

            //Debug.Log(coin.itemName);
            mouseoverTextDesc1.text = coin.itemName + "\n" + coin.itemDescription;
        }
        //name
        //description
        //description 2
    }
    public void OnPointerExit(PointerEventData data)
    {
        mouseover.SetActive(false);
    }
}

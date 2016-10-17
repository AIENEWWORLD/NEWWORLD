using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyCoinsScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    Image itemImage;
    public int itemNumber;
    SetupFight inv;

    public float spinrate = 0.25f;
    public GameObject myCoin;

    GameObject mouseover;

    Text mouseoverTextDesc1;
    Text mouseoverTextDesc2;

    // Use this for initialization
    void Start()
    {
        myCoin.SetActive(false);
        inv = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();

        myCoin.transform.Rotate(0, 0, Random.Range(0, 100));

    }

    // Update is called once per frame
    void Update()
    {
        if (itemImage.sprite != null)
        {
            myCoin.SetActive(true);
            
        }
        else
        {
            myCoin.SetActive(false);
        }
        myCoin.transform.Rotate(0, 0, (spinrate));

        if (inv.EnemycoinList[itemNumber].itemName != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = inv.EnemycoinList[itemNumber].Icon;
        }
        else
        {
            itemImage.enabled = false;
        }

    }

    public void OnPointerDown(PointerEventData data)
    {
        if (inv.picking && coin.itemName != "empty slot" && inv.pickCoinList[0].cType == CoinStats.coinTypes.flip)
        {
            inv.checkSelections(coin);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseover = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().mouseover;
        mouseover.SetActive(true);
        mouseoverTextDesc1 = GameObject.FindGameObjectWithTag("Desc1").GetComponent<Text>();
        mouseoverTextDesc2 = GameObject.FindGameObjectWithTag("Desc2").GetComponent<Text>();

        //Debug.Log(coin.itemName);
        mouseoverTextDesc1.text = coin.itemName + "\n" + coin.itemDescription;
        mouseoverTextDesc2.text = coin.itemDescription2;
        
        //name
        //description
        //description 2
    }
    public void OnPointerExit(PointerEventData data)
    {
        mouseover.SetActive(false);
    }
}

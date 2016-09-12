using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCoinsScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    Image itemImage;
    public int itemNumber;
    SetupFight inv;

    GameObject mouseover;

    Text mouseoverTextDesc1;
    Text mouseoverTextDesc2;

    // Use this for initialization
    void Start()
    {
        inv = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (inv.PlayercoinList[itemNumber].itemName != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = inv.PlayercoinList[itemNumber].Icon;
        }
        else
        {
            itemImage.enabled = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {

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

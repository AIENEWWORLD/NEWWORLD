using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCoins : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    Image itemImage;
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

    }

    public void OnPointerDown(PointerEventData data)
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseover = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().mouseover;
        mouseover.SetActive(true);
        if (GameObject.FindGameObjectWithTag("Desc1").GetComponent<Text>() != null)
        {
            mouseoverTextDesc1 = GameObject.FindGameObjectWithTag("Desc1").GetComponent<Text>();
        }
        if (GameObject.FindGameObjectWithTag("Desc2").GetComponent<Text>() != null)
        {
            mouseoverTextDesc2 = GameObject.FindGameObjectWithTag("Desc2").GetComponent<Text>();
        }

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

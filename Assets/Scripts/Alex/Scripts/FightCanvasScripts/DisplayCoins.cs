using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCoins : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    public float spinrate = 0.25f;
    float tempspinrate = 0;
    public GameObject myCoin;
    Image itemImage;

    [HideInInspector]
    public GameObject mouseover;

    Text mouseoverTextDesc1;
    Text mouseoverTextDesc2;

    // Use this for initialization
    void Start()
    {
        myCoin.SetActive(false);
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        tempspinrate = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (itemImage.sprite != null)
        {
            myCoin.SetActive(true);
            myCoin.transform.Rotate(0, 0, (spinrate + tempspinrate));
            tempspinrate = 0;
        }
        else
        {
            myCoin.SetActive(false);
        }
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

        //Debug.Log(coin.itemName);
        mouseoverTextDesc1.text = coin.itemName + "\n" + coin.itemDescription;

        //name
        //description
        //description 2
    }
    public void OnPointerExit(PointerEventData data)
    {
        mouseover.SetActive(false);
    }
}

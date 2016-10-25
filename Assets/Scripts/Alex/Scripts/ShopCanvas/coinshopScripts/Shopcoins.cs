using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shopcoins : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CoinStats coin;
    Image itemImage;

    public float spinrate = 0.25f;
    public GameObject myCoin;

    public Canvas myCan;

    [HideInInspector]
    public GameObject mouseover;
    [HideInInspector]
    public Text mouseoverTextDesc1;
    [HideInInspector]
    public Text mouseoverTextDesc2;

    // Use this for initialization
    void Start()
    {
        myCoin.SetActive(false);
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        myCoin.transform.Rotate(0, 0, Random.Range(0, 100));
    }

    // Update is called once per frame
    void Update()
    {
        if (itemImage.sprite != null && myCan.enabled == true)
        {
            myCoin.SetActive(true);

        }
        else
        {
            myCoin.SetActive(false);
        }
        myCoin.transform.Rotate(0, 0, (spinrate));
    }

    public void OnPointerDown(PointerEventData data)
    {

    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseover.SetActive(true);

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

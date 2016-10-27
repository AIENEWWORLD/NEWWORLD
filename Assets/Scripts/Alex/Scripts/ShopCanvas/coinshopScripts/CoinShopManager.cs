using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CoinShopManager : MonoBehaviour
{
    //this script is terrible, please remember to clean



    public Text goldAmount;
    public Vector3 offsetPos;
    public Canvas myCanvas;
    public GameObject itemPrefab;
    public GameObject DisplayCoinPrefab;
    public List<CoinStats> coins = new List<CoinStats>();
    public List<GameObject> coinitemlist = new List<GameObject>();

    public Camera myCamera;
    public GameObject mouseovercoin;
    public Text Desc1;
    public Text Desc2;
    public Vector3 screenmousePos;
    public Vector2 MouseOverTextOffset = new Vector2(0, 0);
    public TriggerShop TrigShop;

    public float dist = 0;

    void Start ()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab);
            newItem.transform.SetParent(gameObject.transform);
            newItem.transform.localPosition = new Vector3(0, 0, 0);
            newItem.transform.localScale = new Vector3(0.90f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            newItem.transform.localRotation = gameObject.transform.localRotation;

            coinitemlist.Add(newItem);

            Transform coinAttachment = newItem.transform.GetChild(0);
            GameObject newDisplaycoin = Instantiate(DisplayCoinPrefab);

            newDisplaycoin.transform.SetParent(coinAttachment);
            newDisplaycoin.transform.localPosition = new Vector3(offsetPos.x, offsetPos.y, offsetPos.z);
            newDisplaycoin.transform.localScale = coinAttachment.localScale;
            newDisplaycoin.transform.localRotation = coinAttachment.localRotation;

            newDisplaycoin.GetComponent<Shopcoins>().myCan = myCanvas;
            newDisplaycoin.GetComponent<Shopcoins>().coin = coins[i];
            newDisplaycoin.GetComponent<Shopcoins>().mouseover = mouseovercoin;
            newDisplaycoin.GetComponent<Shopcoins>().mouseoverTextDesc1 = Desc1;
            newDisplaycoin.GetComponent<Shopcoins>().mouseoverTextDesc2 = Desc2;


            Coin_buttonscript cbs = newItem.GetComponentInChildren<Coin_buttonscript>();
            cbs.coin_stats = coins[i];
            cbs.myID = i;
            cbs.CSM = this;
            cbs.setbuttonText();
        }
        mouseovercoin.SetActive(false);
        myCanvas.enabled = false;
	}


    void Update ()
    {
        //this seems like a pretty fast way of getting the mouse pos on a pers camera, the reason the position is offset is because the object is at -100 on the z
        //screenmousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //mouseovercoin.transform.localPosition = new Vector3(screenmousePos.x + (MouseOverTextOffset.x), screenmousePos.y + (MouseOverTextOffset.y), mouseovercoin.transform.localPosition.z);

        //mouseovercoin.GetComponent<RectTransform>().anchoredPosition = new Vector2(screenmousePos.x + (MouseOverTextOffset.x), screenmousePos.y + (MouseOverTextOffset.y));

        if (myCanvas.enabled)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out localPoint);
            localPoint.x += MouseOverTextOffset.x;
            localPoint.y += MouseOverTextOffset.y;
            mouseovercoin.transform.position = myCanvas.transform.TransformPoint(localPoint);
        }
    }
    public void onclickexitshop()
    {
        TrigShop.resetStuff();
        Camera.main.orthographic = false;
        Camera.main.nearClipPlane = 0.3f;
        myCanvas.enabled = false;
    }

    public void checkShop()
    {
        for (int i = 0; i < coinitemlist.Count; i++)
        {
            Coin_buttonscript CBS = coinitemlist[i].GetComponentInChildren<Coin_buttonscript>();
            if (CBS.coin_stats.cost <= CBS.playerstats.gold)
            {
                CBS.active = true;
                CBS.GetComponent<Button>().interactable = true;
            }
            else
            {
                CBS.active = false;
                CBS.GetComponent<Button>().interactable = false;
            }
            CBS.setbuttonText();
            goldAmount.text = "" + CBS.playerstats.gold.ToString() + " Gold";
        }
    }
    public void destroyatID(GameObject go)
    {
        Destroy(go);
        for (int i = 0; i < coinitemlist.Count; i++)
        {
            if (coinitemlist[i] == go)
            {
                coinitemlist.Remove(go);
                coins.RemoveAt(i);
            }
        }
    }
}

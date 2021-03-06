﻿using UnityEngine;
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

    public bool flip = false;

    private Quaternion lookRot;
    private Vector3 dir;

    GameObject mouseover;

    Text mouseoverTextDesc1;
    Text mouseoverTextDesc2;

    // Use this for initialization
    void Start()
    {
        myCoin.SetActive(false);
        inv = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();

        myCoin.transform.Rotate(0, Random.Range(0, 100), 0);
        myCoin.transform.GetChild(0).GetComponent<Renderer>().material = coin.GetTexture();
    }

    // Update is called once per frame
    void Update()
    {
        if (coin.notEmpty == false)
        {
            myCoin.SetActive(true);
        }
        else
        {
            myCoin.SetActive(false);
        }

        if (inv.EnemycoinList[itemNumber].itemName != null)
        {
            itemImage.enabled = true;
            //itemImage.sprite = inv.EnemycoinList[itemNumber].Icon;
        }
        else
        {
            itemImage.enabled = false;
        }
        if (flip)
        {
            StartCoroutine(flipcoin(coin.isHeads, 0));
        }
        else
        {
            myCoin.transform.Rotate(0, spinrate, 0);
        }

    }

    public void OnPointerDown(PointerEventData data)
    {
        if (inv.picking && coin.itemName != "empty slot" && inv.pickCoinList[0].cType == CoinStats.coinTypes.flip || inv.picking && coin.itemName != "empty slot" && inv.pickCoinList[0].cType == CoinStats.coinTypes.secondChance)
        {
            inv.checkSelections(coin, gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        mouseover = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<SetupFight>().mouseover;
        mouseover.SetActive(true);
        mouseoverTextDesc1 = GameObject.FindGameObjectWithTag("Desc1").GetComponent<Text>();

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

    public IEnumerator flipcoin(bool heads, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (flip)
        {
            //spinCoin = false;
            Vector3 pos = transform.position;
            if (heads)
            {
                pos.z -= 5;
            }
            else
            {
                pos.z += 5;
            }


            dir = (pos - myCoin.transform.position).normalized;
            lookRot = Quaternion.LookRotation(dir);
            myCoin.transform.rotation = Quaternion.Slerp(myCoin.transform.rotation, lookRot, Time.deltaTime * spinrate);
        }
    }
}
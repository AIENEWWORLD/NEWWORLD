using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnWinLose : MonoBehaviour
{
    public GameObject FightCanvas;

    public GameObject endCombatCanvas;
    public Text Title;
    public Text RecievedText;
    public Transform CoinPosition;
    public GameObject PlayerCoinPrefab;
    public GameObject item;
    public bool countdown = false;
    public bool counteddown = false;
    public float count = 2;

    void Start ()
    {
        endCombatCanvas.SetActive(false);
	}
	
	void Update ()
    {
	    //if input getbuttondown mouse and fightcanvas isnt enabled and endcombatcanvas is enabled
        //close the canvas
        //destroy the coin
        if(countdown == true && counteddown == false)
        {
            count -= Time.deltaTime;
            if(count <= 0)
            {
                counteddown = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && FightCanvas.activeSelf == false && endCombatCanvas.activeSelf == true && counteddown == true)
        {
            gameObject.GetComponent<Camera>().enabled = false;
            FightCanvas.SetActive(true);
            endCombatCanvas.SetActive(false);
            item.GetComponent<DisplayCoins>().mouseover.SetActive(false);
            Destroy(item);

        }
	}

    public void CheckDeath(bool dead, CoinStats coin)
    {

        Title.text = "";
        RecievedText.text = "";

        FightCanvas.SetActive(false);
        if (dead == true)
        {
            Title.text = "You Win";
            RecievedText.text = "you got " + 1 +  " gold\nclick to continue...";
            
            if(coin.itemName.CompareTo("") == 1)
            {
                Debug.Log(coin.itemName);
                item = Instantiate(PlayerCoinPrefab);
                item.GetComponent<DisplayCoins>().coin = coin;
                item.transform.SetParent(CoinPosition);
                item.transform.localScale = new Vector3(2, 2, 2);
                item.transform.localPosition = new Vector3(0, 0, 0);
                //item.transform.localPosition = CoinPosition.position;
                //item.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if(dead == false)
        {
            Title.text = "You Died";
            RecievedText.text = "click to continue...";

            //do stuff here I guess
        }
        endCombatCanvas.SetActive(true);
        counteddown = false;
        countdown = true;
        count = 2;
    }
}

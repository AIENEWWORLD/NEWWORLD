using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriggerShop : MonoBehaviour
{
    public Canvas speechCanvas;
    public Canvas myCanvas;
    public CoinShopManager CSM;
    public UpgradeButtonScripts UBS;
    public Text tmptext;
    public string textToType = "";
    public string newTEXT;
    public float TimeBetweenChar;
    public bool runText = false;
    public bool tmpBool = false;
    public int x = 0;
    public float timeBetweenCanvas = 2;
    public bool newbool = false;

    /*
     * when you walk into the shop enable the speech canvas and set its text to the newTEXT.
     * if the string compared to the other string is a match initialise the other coroutine
     * after it's waited, disable this shop and enable the other shop.
     */
    void start()
    {
        speechCanvas.enabled = false;
    }
	void Update ()
    {
        tmptext.text = newTEXT;
        if (tmpBool)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                speechCanvas.enabled = true;
                StartCoroutine(writeText(textToType, TimeBetweenChar));
                runText = false;
            }
        }
        else
        {
            x = 0;
            newTEXT = "";
        }

        if (Input.GetKeyDown(KeyCode.Space) && speechCanvas.enabled == true && tmpBool == true || newTEXT.CompareTo(textToType) == 0 && tmpBool == true)
        {
            newTEXT = textToType;
            newbool = true;
            StartCoroutine(enablemyCanvas(timeBetweenCanvas));
        }


    }

    void OnTriggerEnter(Collider collision)
    {
        tmpBool = true;

    }
    void OnTriggerExit(Collider collision)
    {
        tmpBool = false;
        myCanvas.enabled = false;
        speechCanvas.enabled = false;
        newbool = false;
    }

    public IEnumerator writeText(string txt, float time)
    {
        yield return new WaitForSeconds(time);
        if (x < txt.Length && newbool == false && tmpBool)
        {
            //Debug.Log(txt[x]);
            newTEXT = newTEXT + txt[x];
            x += 1;
            StartCoroutine(writeText(textToType, TimeBetweenChar));
        }
        else
        {
            runText = false;
        }
    }

    public IEnumerator enablemyCanvas(float time)
    {
        yield return new WaitForSeconds(time);
        if (tmpBool && newTEXT.CompareTo(textToType) == 0)
        {
            speechCanvas.enabled = false;
            if (UBS != null)
            {
                UBS._checkGold();
            }
            if (CSM != null)
            {
                CSM.checkShop();
            }
            myCanvas.enabled = true;
        }
    }
}

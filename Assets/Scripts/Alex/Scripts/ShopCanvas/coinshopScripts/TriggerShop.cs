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
    public Text PressToContText;
    public string textToType = "";
    public string newTEXT;
    public float TimeBetweenChar;
    public bool runText = true;
    public bool tmpBool = false;
    public int x = 0;
    public float timeBetweenCanvas = 2;
    public bool newbool = false;
    public bool typing = false;
    public SavedInput InputGameobject;
    public bool enablePresstoCont = false;

    bool starting = true;
    //startup not working well for getcomponent, probably something to do with the script execution order

    /*
     * when you walk into the shop enable the speech canvas and set its text to the newTEXT.
     * if the string compared to the other string is a match initialise the other coroutine
     * after it's waited, disable this shop and enable the other shop.
     */
    void start()
    {
        speechCanvas.enabled = false;
        //PressToContText.enabled = false;
    }
    void Update()
    {
        tmptext.text = newTEXT;
        if (starting)
        {
            InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes").GetComponent<SavedInput>(); //less getcomponents per update
            starting = false;
        }



        if (newTEXT.CompareTo(textToType) == 0)
        {
            runText = false;
        }
        else
        {
            myCanvas.enabled = false;
        }

        if (Input.GetKeyDown(InputGameobject.keycodes["interact"]) && speechCanvas.enabled == true && newTEXT.CompareTo(textToType) == 0 && tmpBool == true)
        {
        
            newbool = true;
            StartCoroutine(enablemyCanvas(0.2f));
        }

        if (Input.GetKeyDown(InputGameobject.keycodes["interact"]) && typing == true)
        {
            newbool = true;
            newTEXT = textToType;
            
        }
        if (tmpBool)
        {
            if (Input.GetKeyDown(InputGameobject.keycodes["interact"]) && myCanvas.enabled == false && runText == true)
            {
                enablePresstoCont = false;
                GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>().inShop = true;
                speechCanvas.enabled = true;
                
                StartCoroutine(writeText(textToType, TimeBetweenChar));
                //runText = false;
            }
        }
        else
        {
            x = 0;
            newTEXT = "";
        }
        //if(enablePresstoCont)
        //{
        //    PressToContText.enabled = true;
        //    PressToContText.text = "Press " + InputGameobject.keycodes["interact"].ToString() + " to enter shop.";
        //}
        //else
        //{
        //    PressToContText.enabled = false;
        //}

    }

    void OnTriggerEnter(Collider collision)
    {
        tmpBool = true;
        if (speechCanvas.enabled == false && myCanvas.enabled == false)
            enablePresstoCont = true;
        
    }
    void OnTriggerStay(Collider collision)
    {
        tmpBool = true;
        if (speechCanvas.enabled == false && myCanvas.enabled == false)
            enablePresstoCont = true;
    }

    public void resetStuff()
    {
        StopAllCoroutines();
        runText = true;
        newTEXT = "";
        tmpBool = false;
        myCanvas.enabled = false;
        speechCanvas.enabled = false;
        newbool = false;
        typing = false;
        enablePresstoCont = false;
        GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>().inShop = false;
    }

    void OnTriggerExit(Collider collision)
    {
        resetStuff();
        Camera.main.orthographic = false;
        Camera.main.nearClipPlane = 0.3f;
    }

    public IEnumerator writeText(string txt, float time)
    {
        typing = true;
        yield return new WaitForSeconds(time);
        if (x < txt.Length && newbool == false && tmpBool && runText == true)
        {

            //Debug.Log(txt[x]);
            newTEXT = newTEXT + txt[x];
            x += 1;
            StartCoroutine(writeText(textToType, TimeBetweenChar));
        }
    }

    public IEnumerator enablemyCanvas(float time)
    {
        yield return new WaitForSeconds(time);
        Camera.main.nearClipPlane = -40;
        Camera.main.orthographic = true;
        if (tmpBool && newTEXT.CompareTo(textToType) == 0 && runText == false)
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

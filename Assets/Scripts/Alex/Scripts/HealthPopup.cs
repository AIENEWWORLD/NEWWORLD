using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthPopup : MonoBehaviour
{
    public string fiftypercentText = "";
    public string zeropercentText = "";
    public string newTEXT;
    public float TimeBetweenChar;
    public int x = 0;
    public GameObject TextPrefab;
    public Text myText;
    Canvas myCanvas;
    GameObject g;

    public float TimeAfterFinsh;

    StatsScript Player;

    public bool hasEnabled = false;

    public string curr;

    void Start()
    {
        g = Instantiate(TextPrefab, Camera.main.transform) as GameObject;
        myCanvas = g.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        myCanvas.worldCamera = Camera.main;
        myText = g.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Player = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<StatsScript>();
    }

    void Update()
    {
        myText.text = newTEXT;
        //myCanvas.enabled = true;
        //StartCoroutine(writeText(fiftypercentText, TimeBetweenChar));
        if (Player.supplies / Player.maxSupply > 0.5f)
        {
            hasEnabled = false;
        }
        else if (Player.supplies / Player.maxSupply < 0.01f)
        {
            if (hasEnabled == true)
            {
                myCanvas.enabled = true;
                x = 0;
                newTEXT = "";
                curr = zeropercentText;
                StartCoroutine(writeText(zeropercentText, TimeBetweenChar));
                hasEnabled = false;
            }
        }
        else
        {
            if (hasEnabled == false)
            {
                myCanvas.enabled = true;
                x = 0;
                newTEXT = "";
                curr = fiftypercentText;
                StartCoroutine(writeText(fiftypercentText, TimeBetweenChar));
                hasEnabled = true;
            }
        }
    }

    public IEnumerator exit()
    {
        yield return new WaitForSeconds(TimeAfterFinsh);
        myCanvas.enabled = false;
    }

    public IEnumerator writeText(string txt, float time)
    {
        yield return new WaitForSeconds(time);
        if (x < txt.Length)
        {
            //Debug.Log(txt[x]);
            newTEXT = newTEXT + txt[x];
            x += 1;
            StartCoroutine(writeText(curr, TimeBetweenChar));
        }
        else
        {
            StartCoroutine(exit());
        }
    }
}

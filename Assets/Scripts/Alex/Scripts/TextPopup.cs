using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextPopup : MonoBehaviour
{
    public string textToType = "";
    public string newTEXT;
    public float TimeBetweenChar;
    public int x = 0;
    public GameObject TextPrefab;
    public Text myText;
    Canvas myCanvas;
    GameObject g;

    public float TimeAfterFinsh;

    void Start()
    {
        g = Instantiate(TextPrefab, Camera.main.transform) as GameObject;
        myCanvas = g.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        myCanvas.worldCamera = Camera.main;
        myText = g.transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        myText.text = newTEXT;
    }

    void OnTriggerEnter()
    {
        myCanvas.enabled = true;
        StartCoroutine(writeText(textToType, TimeBetweenChar));
    }

    public IEnumerator exit()
    {
        yield return new WaitForSeconds(TimeAfterFinsh);
        Destroy(g);
        Destroy(this);
    }

    public IEnumerator writeText(string txt, float time)
    {
        yield return new WaitForSeconds(time);
        if (x < txt.Length)
        {
            //Debug.Log(txt[x]);
            newTEXT = newTEXT + txt[x];
            x += 1;
            StartCoroutine(writeText(textToType, TimeBetweenChar));
        }
        else
        {
            StartCoroutine(exit());
        }
    }
}

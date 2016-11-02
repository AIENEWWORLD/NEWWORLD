using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class BoolClass
{
    public Texture myTexture;
    public bool FleeEnabled;
    public bool AttackEnabled;
    public bool InventoryEnabled;
    public bool ClickToContinue;
}

public class TutorialDisplayImages : MonoBehaviour
{
    public List<BoolClass> TutorialImages;
    public SetupFight SF;
    RawImage ImageHolder;
    public int currentFrame;
    Texture currentImg;
    public GameObject currentGO;
    public bool Destroyme = true;
    [HideInInspector]
    public Canvas myCanvas;
	void Awake ()
    {
        myCanvas = gameObject.GetComponent<Canvas>();
        ImageHolder = currentGO.GetComponent<RawImage>();
        if (currentFrame < TutorialImages.Count)
        {
            currentFrame = 0;
            ImageHolder.texture = TutorialImages[currentFrame].myTexture;
            GameObject.FindGameObjectWithTag("FightCamera").GetComponent<ButtonsPressed>().setButtons(TutorialImages[currentFrame].AttackEnabled, TutorialImages[currentFrame].FleeEnabled, TutorialImages[currentFrame].InventoryEnabled, TutorialImages[currentFrame].ClickToContinue);
            //set bools
        }
        myCanvas.enabled = false;
        if (TutorialImages[currentFrame].ClickToContinue)
        {
            ImageHolder.raycastTarget = true;
        }
        else
        {
            ImageHolder.raycastTarget = false;
        }
    }
	
	void Update ()
    {
	    if(Input.GetMouseButtonUp(0) && myCanvas.enabled == true)
        {

            if (currentFrame < TutorialImages.Count)
            {
                if (TutorialImages[currentFrame].ClickToContinue)
                {
                    ImageHolder.raycastTarget = true;
                    currentFrame++;
                }
                else
                {
                    ImageHolder.raycastTarget = false;
                }
            }
            //Debug.Log(currentFrame + " " + TutorialImages.Count);
            if (currentFrame < TutorialImages.Count)
            {
                ImageHolder.texture = TutorialImages[currentFrame].myTexture;
                GameObject.FindGameObjectWithTag("FightCamera").GetComponent<ButtonsPressed>().setButtons(TutorialImages[currentFrame].AttackEnabled, TutorialImages[currentFrame].FleeEnabled, TutorialImages[currentFrame].InventoryEnabled, TutorialImages[currentFrame].ClickToContinue);
                //set bools
            }
            else if(Destroyme)
            {
                //Debug.Log("S");
                ImageHolder.texture = null;
                myCanvas.enabled = false;
                SF.TutorialPlayed = true;
                currentGO.SetActive(false);
                //Destroy(gameObject);
            }
        }
	}
}

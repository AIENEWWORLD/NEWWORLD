using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialDisplayImages : MonoBehaviour
{
    public List<Texture> TutorialImages;
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
            ImageHolder.texture = TutorialImages[currentFrame];
        }
        myCanvas.enabled = false;
    }
	
	void Update ()
    {
	    if(Input.GetMouseButtonUp(0) && myCanvas.enabled == true)
        {
            currentFrame++;
            //Debug.Log(currentFrame + " " + TutorialImages.Count);
            if (currentFrame < TutorialImages.Count)
            {
                ImageHolder.texture = TutorialImages[currentFrame];
            }
            else if(Destroyme)
            {
                ImageHolder.texture = null;
                myCanvas.enabled = false;
                SF.TutorialPlayed = true;
                currentGO.SetActive(false);
                //Destroy(gameObject);
            }
        }
	}
}

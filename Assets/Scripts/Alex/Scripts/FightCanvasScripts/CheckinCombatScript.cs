using UnityEngine;
using System.Collections;

public class CheckinCombatScript : MonoBehaviour
{ 

    //all the buttons still work even though camera was disabled.

    GameObject FightCamera;
    GameObject FightCanvas;
    [HideInInspector]
    public bool Combatisenabled = false;

	void Start ()
    {
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
        FightCanvas = GameObject.FindGameObjectWithTag("FightCanvas");

        if (GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>().Combatisenabled == false)
        {
            //do stuff if not in combat
        }
	}
	
	void Update ()
    {
        //Debug.Log(isInCombat());
        if (FightCamera != null)
        {
            if(FightCamera.GetComponent<Camera>().enabled == true)
            {
                FightCanvas.GetComponent<Canvas>().enabled = true;

                Combatisenabled = true;
            }
            else if(FightCamera.GetComponent<Camera>().enabled == false)
            {
                FightCanvas.GetComponent<Canvas>().enabled = false;

                Combatisenabled = false;
            }
        }
	}
}



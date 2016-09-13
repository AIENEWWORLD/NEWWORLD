using UnityEngine;
using System.Collections;

public class CheckinCombatScript : MonoBehaviour
{
    //all the buttons still work even though camera was disabled.

    GameObject FightCamera;

	void Start ()
    {
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
	}
	
	void Update ()
    {
	    if(FightCamera != null)
        {
            if(FightCamera.GetComponent<Camera>().enabled == true)
            {
                FightCamera.SetActive(true);

                //set a bunch of bools to false (player movement, enemy movement)
            }
            else if(FightCamera.GetComponent<Camera>().enabled == false)
            {
                FightCamera.SetActive(false);

                //set a bunch of bools to true (player movement, enemy movement)
            }
        }
	}
}

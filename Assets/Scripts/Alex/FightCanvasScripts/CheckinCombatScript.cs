using UnityEngine;
using System.Collections;

public class CheckinCombatScript : MonoBehaviour
{
    //all the buttons still work even though camera was disabled.

    GameObject FightCamera;
    [HideInInspector]
    bool Combatisenabled = false;

	void Start ()
    {
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");

        if(GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>().Combatisenabled == false)
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
                FightCamera.SetActive(true);

                Combatisenabled = true;
            }
            else if(FightCamera.GetComponent<Camera>().enabled == false)
            {
                FightCamera.SetActive(false);

                Combatisenabled = false;
            }
        }
	}

    //returns false if the player isn't in combat
    public bool isInCombat()
    {
        return Combatisenabled;
    }
}



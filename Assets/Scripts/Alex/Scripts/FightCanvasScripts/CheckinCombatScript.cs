﻿using UnityEngine;
using System.Collections;

public class CheckinCombatScript : MonoBehaviour
{ 

    //all the buttons still work even though camera was disabled.

    GameObject FightCamera;
    GameObject FightCanvas;
    GameObject OptionsCamera;
    GameObject MapCamera;
    //[HideInInspector]
    public bool Combatisenabled = false;
    public bool Optionsisenabled = false;
    public bool Mapisenabled = false;
    public bool inShop = false;

    ControlScript PlayerCS;

	void Start ()
    {
        FightCamera = GameObject.FindGameObjectWithTag("FightCamera");
        FightCanvas = GameObject.FindGameObjectWithTag("FightCanvas");
        MapCamera = GameObject.FindGameObjectWithTag("MapCamera");
        OptionsCamera = GameObject.FindGameObjectWithTag("OptionsCamera");

        PlayerCS = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlScript>();

        //if (GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>().Combatisenabled == false)
        //{
        //    //do stuff if not in combat
        //}
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
        if (OptionsCamera.GetComponent<Camera>().enabled == true)
        {
            Optionsisenabled = true;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            Optionsisenabled = false;
        }

        if (MapCamera.GetComponent<Camera>().enabled == true)
        {
            Mapisenabled = true;
        }
        else
        {
            Mapisenabled = false;
        }


        if (Combatisenabled || Optionsisenabled || Mapisenabled || inShop)
        {
            PlayerCS.p_SeizeMovement = true;
            PlayerCS.NotMoving = true;
            //animation play idle
        }
        else
        {
            PlayerCS.p_SeizeMovement = false;
        }
	}
}



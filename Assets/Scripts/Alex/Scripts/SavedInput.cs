using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

//THIS IS GETTING ERRORS WHEN MINIMIZED

//using controls:

[System.Serializable]
public class keys
{
    public KeyCode keycode;
    public string keyname;
}

public class SavedInput : MonoBehaviour
{
    public List<keys> controls; //save this
    public Dictionary<string, KeyCode> keycodes = new Dictionary<string, KeyCode>(); //save this
    public float SoundValue; //save this
    public float soundEffectValue; //save this I guess

    public static SavedInput instance;

    //testing horizontal, vertical.
    public float horizontal = 0;
    public float vertical = 0;


    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < controls.Count; i++)
        {
            keycodes.Add(controls[i].keyname, controls[i].keycode);
            //Debug.Log("adding keyname: " + controls[i].keyname + " keycode: " + controls[i].keycode.ToString());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(keycodes["left"]))
        {
            horizontal = -1;

        }
        else if (Input.GetKey(keycodes["right"]))
        {
            horizontal = 1;

        }
        else
        {
            horizontal = 0;
        }

        if (Input.GetKey(keycodes["forward"]))
        {
            vertical = 1;
        }
        else if (Input.GetKey(keycodes["down"]))
        {
            vertical = -1;
        }
        else
        {
            vertical = 0;
        }


        /*
        GameObject InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes");
        
        velocity.x = InputGameobject.GetComponent<SavedInput>().horizontal;
		velocity.z = InputGameobject.GetComponent<SavedInput>().vertical;
         */
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }
}

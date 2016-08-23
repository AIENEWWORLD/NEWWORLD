using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;


//using controls:

[System.Serializable]
public class keys
{
    public KeyCode keycode;
    public string keyname;
}

public class SavedInput : MonoBehaviour
{
    public List<keys> controls;
    public Dictionary<string, KeyCode> keycodes = new Dictionary<string, KeyCode>();
    public float SoundValue;
    public float soundEffectValue;

    public static SavedInput instance;

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
	    if(Input.GetKeyDown(keycodes["forward"]))
        {
            //Debug.Log("forward");
        }
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

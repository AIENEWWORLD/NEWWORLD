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
    public List<keys> controls; //save this
    public Dictionary<string, KeyCode> keycodes = new Dictionary<string, KeyCode>(); //save this
    public float SoundValue; //save this
    public float soundEffectValue; //save this I guess

    public static SavedInput instance;

    //testing horizontal, vertical.
    public float horizontal = 0;
    public float vertical = 0;

    public float Temphorizontal = 0;
    public float Tempvertical = 0;

    public float smooth = 5;
    [Range(0,1)]
    public float intepAdjustment = 0.05f;

    public bool doesSmooth = true;

    public OptionsMenu OptionsMenuObject;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < controls.Count; i++)
        {
            keycodes.Add(controls[i].keyname, controls[i].keycode);
            //Debug.Log("adding keyname: " + controls[i].keyname + " keycode: " + controls[i].keycode.ToString());
        }
    }

    void smoothVoid()
    {
        Temphorizontal = Mathf.Lerp(Temphorizontal, horizontal, Time.deltaTime*smooth);
        if (Mathf.Abs(Temphorizontal) < intepAdjustment)
        {
            Temphorizontal = 0;
        }
        if (Temphorizontal > 1)
            Temphorizontal = 1;

        if (Temphorizontal < -1)
            Temphorizontal = -1;

        horizontal = Temphorizontal;

        Tempvertical = Mathf.Lerp(Tempvertical, vertical, Time.deltaTime*smooth);
        if (Mathf.Abs(Tempvertical) < intepAdjustment)
        {
            Tempvertical = 0;
        }
        if (Tempvertical > 1)
            Tempvertical = 1;

        if (Tempvertical < -1)
            Tempvertical = -1;

        vertical = Tempvertical;
    }
	
	// Update is called once per frame
	void Update ()
    {
        horizontal = 0;
        vertical = 0;
        if (Input.GetKey(keycodes["left"]))
        {
            horizontal = -1;
            if (Temphorizontal > -intepAdjustment)
            Temphorizontal -= intepAdjustment;
        }
        if (Input.GetKey(keycodes["right"]))
        {
            horizontal += 1;
            if(Temphorizontal < intepAdjustment)
            Temphorizontal += intepAdjustment;
        }
        
        if (Input.GetKey(keycodes["down"]))
        {
            vertical = -1;
            if(Tempvertical > -intepAdjustment)
            Tempvertical -= intepAdjustment;
        }
        if (Input.GetKey(keycodes["forward"]))
        {
            vertical += 1;
            if(Tempvertical < intepAdjustment)
            Tempvertical += intepAdjustment;
        }
        if (doesSmooth)
        {
            smoothVoid();
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
        //OptionsMenuObject = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<OptionsMenu>();

        //OptionsMenuObject.SoundSelectedSlider = SoundValue;

        ///

        //OptionsMenuObject.setKeyCodes();
        //OptionsMenuObject.applycontrols();
    }
}

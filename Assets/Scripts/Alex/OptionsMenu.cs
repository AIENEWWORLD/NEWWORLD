﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

//the left mouse binding is kinda broken.

//the list saves the keys, returning to menu results in reset of controls as it needs to be saved throughout all scenes.

//make sure when checking the list for input there is something that exists with that name

//make selectedbutton's name the same as in inspector

[System.Serializable]
public class keycodes
{
    public KeyCode keycode;
    public string keyname;
    public Button BindingButton;
}

public class OptionsMenu : MonoBehaviour
{

    public RawImage arrow;

    public GameObject GraphicsOptionsChild;
    public GameObject SoundOptionsChild;
    public GameObject KeyBindingsChild;

    public Button GraphicsSelectedButton;
    public Slider SoundSelectedSlider; //need to save this across scenes or at least the float value
    public Slider soundEffectSlider; //need to save this across scenes or at least the float value
    public Button KeybindingsSelectedButton;
    public int selection = 1;
    public int prevselection;
    public bool setselect = false;

    public Toggle vsyncToggle;
    public Toggle FullscreenToggle;
    

    public Text resbutton;

    public bool fullscreen = true;
    public int vsync = 1;
    public int height, width;

    public Resolution[] res;
    private int resindex = 0;

    public Dictionary<string, KeyCode> key = new Dictionary<string, KeyCode>(); //need to save this across scenes
    public List<keycodes> controls;

    public Button selectedButton;
    public Button prevselectedButton;

    public bool DebugControls = false;
    public bool testkeycodes = false;

    void Start()
    {
        res = Screen.resolutions;
        for(int i = 0; i < res.Length; i++)
        {
            if (res[i].width == Screen.width && res[i].height == Screen.height)
            {
                resindex = i;
            }
        }
        vsync = QualitySettings.vSyncCount; //0 = unlimited, 1 = enabled to monitor refresh rate, 2 = half refresh rate.
        FullscreenToggle.isOn = Screen.fullScreen;
        if (vsync == 1)
        {
            vsyncToggle.isOn = true;
        }
        else
        {
            vsyncToggle.isOn = false;
        }
        width = Screen.width;
        height = Screen.height;
        resbutton.text = width + "x" + height;

        setKeyCodes();

        if (DebugControls)//
        {
            for (int i = 0; i < controls.Count; i++)
            {
                key.Add(controls[i].keyname, controls[i].keycode);
                Debug.Log("adding keyname: " + controls[i].keyname + " keycode: " + controls[i].keycode.ToString());
            }
            if (controls.Count < 1)
            {
                Debug.Log("assign controls on the options script");
            }
        }
    }
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, EventSystem.current.currentSelectedGameObject.transform.localPosition.y, 0);
        }

        if(DebugControls)//
        {
            foreach(KeyValuePair<string, KeyCode> k in key)
            {
                Debug.Log(k.Key + " " + k.Value);
            }
            DebugControls = false;
        }

        if(testkeycodes)//
        {
            setkeyControls();
            testkeycodes = false;
        }

        if (Input.GetButtonDown("RightButton") && selection < 3)
        {
            selection += 1;
            setselect = false;
        }

        if (Input.GetButtonDown("LeftButton") && selection > 1)
        {
            selection -= 1;
            setselect = false;
        }

        getSelection();
        getUpdates();

        prevselection = selection;
    }
    void getSelection()
    {
        if(selection == 1 && prevselection != selection)
        {
            //set selection to resolution
            GraphicsOptionsChild.SetActive(true);
            SoundOptionsChild.SetActive(false);
            KeyBindingsChild.SetActive(false);
            if (setselect == false)
            {
                GraphicsSelectedButton.Select();
                setselect = true;
            }

        }
        if(selection == 2 && prevselection != selection)
        {
            //set selection to volume slider
            GraphicsOptionsChild.SetActive(false);
            SoundOptionsChild.SetActive(true);
            KeyBindingsChild.SetActive(false);
            if (setselect == false)
            {
                SoundSelectedSlider.Select();
                setselect = true;
            }

        }
        if(selection == 3 && prevselection != selection)
        {
            //set selection to keybindings
            GraphicsOptionsChild.SetActive(false);
            SoundOptionsChild.SetActive(false);
            KeyBindingsChild.SetActive(true);
            setkeyControls();
            if (setselect == false)
            {
                KeybindingsSelectedButton.Select();
                setselect = true;
            }
        }
    }
    public void ClickGraphics(Button button)
    {
        selection = 1;
        setselect = false;
    }
    public void ClickSound(Button button)
    {
        selection = 2;
        setselect = false;
    }
    public void ClickKeyBindings(Button button)
    {
        selection = 3;
        setselect = false;
    }
    public void clickBack()
    {
        SceneManager.LoadScene(0);
    }
    public void MouseOverButton(Button button)
    {
        button.Select();
    }
    public void MouseOverToggle(Toggle toggle)
    {
        toggle.Select();
    }
    public void MouseOverSlider(Slider slider)
    {
        slider.Select();
    }
    public void onSelectResButton(BaseEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && resindex > 0)
        {
            //Debug.Log(resindex);
            resindex -= 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && resindex < res.Length-1)
        {
            //Debug.Log(resindex);
            resindex += 1;
        }
    }
    public void onClickResArrowLeft()
    {
        if(resindex > 0)
        {
            resindex -= 1;
        }
    }
    public void onClickResArrowRight()
    {
        if(resindex < res.Length-1)
        {
            resindex += 1;
        }
    }
    public void onSelectKeyBindingButton(Button button)
    {
        if(selectedButton != null)
        {
            prevselectedButton = selectedButton;
        }
        selectedButton = button;
        //setKeyCodes();
        button.transform.GetComponentInChildren<Text>().text = "assign a key";
    }
    void getUpdates()
    {
        if (vsyncToggle.isOn)
        {
            vsync = 1;
        }
        if (!vsyncToggle.isOn)
        {
            vsync = 0;
        }

        if (FullscreenToggle.isOn)
        {
            fullscreen = true;
        }
        if (!FullscreenToggle.isOn)
        {
            fullscreen = false;
        }
        resbutton.text = res[resindex].width + "x" + res[resindex].height;
    }
    public void clickApply()
    {
        Camera.main.GetComponent<AudioSource>().volume = SoundSelectedSlider.value;
        Screen.SetResolution(res[resindex].width, res[resindex].height, fullscreen);
       // Resolution.text = "Resolution: " + width + "x" + height;
        QualitySettings.vSyncCount = vsync;

        setKeyCodes();
        applycontrols();
    }
    void OnGUI()
    {
        if(selectedButton != null)
        {
            Event myEvent = Event.current;

            if(myEvent.button == 1 && myEvent.isMouse)
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i].keyname.CompareTo(selectedButton.name) == 0)
                    {
                        
                        controls[i].keycode = KeyCode.Mouse1;
                    }
                }
                setKeyCodes();
                selectedButton = null;
            }

            if (myEvent.button == 0 && myEvent.isMouse)//no idea why this works
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i].keyname.CompareTo(selectedButton.name) == 0)
                    {
                        controls[i].keycode = KeyCode.Mouse0;
                    }
                }
                setKeyCodes();
                selectedButton = null;
            }

            if (myEvent.isKey)
            {
                for(int i = 0; i < controls.Count; i++)
                {
                    if(controls[i].keyname.CompareTo(selectedButton.name) == 0)
                    {
                        controls[i].keycode = myEvent.keyCode;
                    }
                }
                setKeyCodes();
                selectedButton = null;
            }
        }
        
    }
    void setkeyControls()
    {
        int _index = 0;
        foreach (KeyValuePair<string, KeyCode> k in key)
        {
            controls[_index].keycode = k.Value;
            if (k.Value.ToString() != "Mouse0" && k.Value.ToString() != "Mouse1")
            {
                controls[_index].BindingButton.transform.GetComponentInChildren<Text>().text = k.Value.ToString();
            }
            else if(k.Value.ToString() == "Mouse0")
            {
                controls[_index].BindingButton.transform.GetComponentInChildren<Text>().text = "Left mouse";
            }
            else if (k.Value.ToString() == "Mouse1")
            {
                controls[_index].BindingButton.transform.GetComponentInChildren<Text>().text = "Right mouse";
            }
            _index += 1;
        }
    }
    void setKeyCodes()
    {
        key.Clear();
        for(int i = 0; i < controls.Count; i++)
        {
            key.Add(controls[i].keyname, controls[i].keycode);
            if (controls[i].keycode.ToString() != "Mouse0" && controls[i].keycode.ToString() != "Mouse1")
            {
                controls[i].BindingButton.transform.GetComponentInChildren<Text>().text = controls[i].keycode.ToString();
            }
            else if(controls[i].keycode.ToString() == "Mouse0")
            {
                controls[i].BindingButton.transform.GetComponentInChildren<Text>().text = "Left mouse";
            }
            else if(controls[i].keycode.ToString() == "Mouse1")
            {
                controls[i].BindingButton.transform.GetComponentInChildren<Text>().text = "Right mouse";
            }
        }
    }
    void applycontrols()
    {
        GameObject InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes");

        if(InputGameobject != null)
        {
            InputGameobject.GetComponent<SavedInput>().keycodes.Clear();

            for (int i = 0; i < controls.Count; i++)
            {
                InputGameobject.GetComponent<SavedInput>().keycodes.Add(controls[i].keyname, controls[i].keycode);
                //Debug.Log("adding " + controls[i].keyname + " " + controls[i].keycode);
                InputGameobject.GetComponent<SavedInput>().controls[i].keyname = controls[i].keyname;
                InputGameobject.GetComponent<SavedInput>().controls[i].keycode = controls[i].keycode;
            }

            //InputGameobject.GetComponent<SavedInput>().keycodes = key;
            InputGameobject.GetComponent<SavedInput>().soundEffectValue = soundEffectSlider.value;
            InputGameobject.GetComponent<SavedInput>().SoundValue = SoundSelectedSlider.value;
        }
        
    }
}
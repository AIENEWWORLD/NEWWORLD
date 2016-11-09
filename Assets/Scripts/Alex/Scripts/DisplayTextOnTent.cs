using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayTextOnTent : MonoBehaviour
{
    public Vector3 offset;
    public string myText;
    public GameObject prefabText;
    public GameObject parent;
    GameObject instance;
    Text t;
    public bool disableonInteract = false;
    bool enable = true;
    public bool enableImage = false;
    public Vector3 Imageoffset;
    public Canvas ImageHolder;
    GameObject ImageHolderGO;

    GameObject Player;
    Vector3 myRotation;
    SavedInput InputGameobject;
    void Start ()
    {
        if (enableImage)
        {
            ImageHolder = gameObject.transform.GetChild(0).GetComponent<Canvas>();
            ImageHolderGO = gameObject.transform.GetChild(0).gameObject;
            Player = GameObject.FindGameObjectWithTag("Player");
            myRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            ImageHolder.enabled = false;
        }
        InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes").GetComponent<SavedInput>();
    }
	
	void Update ()
    {
        if (enableImage)
        {
            Vector3 temprot_ = new Vector3(myRotation.x, 0 + Player.transform.rotation.eulerAngles.y + myRotation.y, myRotation.z);
            ImageHolderGO.transform.eulerAngles = temprot_;
        }

    }

    void OnTriggerEnter()
    {
        enable = true;
    }

    void OnTriggerStay()
    {
        if (instance == null && enable)
        {
            instance = Instantiate(prefabText, parent.transform) as GameObject;
            instance.transform.SetParent(parent.transform);
            instance.transform.localPosition = Vector3.zero+offset;
            instance.transform.localScale = new Vector3(1, 1, 1);
            instance.transform.localEulerAngles = new Vector3(0, 0, 0);
            t = instance.GetComponent<Text>();
            t.enabled = true;
            if (enableImage)
            {
                ImageHolder.enabled = true;
            }
        }
        else if(t != null)
        {
            t.text = myText;
        }
        if (Input.GetKeyDown(InputGameobject.keycodes["interact"]) && disableonInteract)
        {
            if (instance != null)
            {
                if (enableImage)
                {
                    ImageHolder.enabled = false;
                }
                enable = false;
                Destroy(instance);
            }
        }
    }
    void OnTriggerExit()
    {
        if (instance != null)
        {
            if (enableImage)
            {
                ImageHolder.enabled = false;
            }
            Destroy(instance);
        }
    }
}

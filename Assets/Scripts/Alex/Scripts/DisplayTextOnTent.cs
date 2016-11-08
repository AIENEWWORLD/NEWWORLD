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

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}
    void OnTriggerStay()
    {
        if (instance == null)
        {
            instance = Instantiate(prefabText, parent.transform) as GameObject;
            instance.transform.SetParent(parent.transform);
            instance.transform.localPosition = Vector3.zero+offset;
            instance.transform.localScale = new Vector3(1, 1, 1);
            instance.transform.localEulerAngles = new Vector3(0, 0, 0);
            t = instance.GetComponent<Text>();
            t.enabled = true;
        }
        else
        {
            t.text = myText;
        }
    }
    void OnTriggerExit()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Transitions : MonoBehaviour
{
    public GameObject TransitionPrefab;
    RawImage transimg;
    public Camera TransCam;
    public Camera prevCamera;
    public bool trans = false;
    public float transSpeed = 3;
    [Range(0.01f, 0.99f)]
    public float transMin = 0.01f;
    [Range(0.01f,0.99f)]
    public float transMax = 0.99f;

    GameObject instance;

    bool endTrans = false;

	void Start ()
    {
        transimg = TransitionPrefab.transform.GetChild(0).GetComponent<RawImage>();
        TransCam = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<Camera>();

    }

    void Update()
    {
        if (Camera.current != null)
        {
            prevCamera = Camera.current;
        }
        if (trans)
        {
            if (instance == null && Camera.current != null)
            {
                instance = Instantiate(TransitionPrefab, Camera.current.transform) as GameObject;
                transimg = instance.transform.GetChild(0).GetComponent<RawImage>();
            }
            if (!endTrans && transimg.color.a <= transMax)
            {
                StartTrans();

            }
            else if (Camera.current != null)
            {
                if (!endTrans)
                {
                    GameObject tmp = Instantiate(instance, Camera.current.transform) as GameObject;
                    Destroy(instance);
                    instance = tmp;
                    transimg = instance.transform.GetChild(0).GetComponent<RawImage>();
                    //Debug.Log("destroyed");
                    TransCam.enabled = true;
                    //Camera.main.enabled = false;
                }
                endTrans = true;
                if (transimg.color.a >= transMin)
                {
                    EndTrans();
                }
                else
                {
                    Destroy(instance);
                    endTrans = false;
                    transimg = TransitionPrefab.transform.GetChild(0).GetComponent<RawImage>();
                    instance = null;
                    trans = false;
                }
            }
        }
	}

    void StartTrans()
    {
        //setAlpha(0);
        setAlpha(Mathf.Lerp(transimg.color.a, 1, Time.deltaTime*transSpeed));
    }
    void EndTrans()
    {
        setAlpha(Mathf.Lerp(transimg.color.a, 0, Time.deltaTime * transSpeed));
    }
    void setAlpha(float a)
    {
        transimg.color = new Color(transimg.color.r, transimg.color.g, transimg.color.b, a);
    }
}

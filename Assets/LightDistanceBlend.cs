using UnityEngine;
using System.Collections;

public class LightDistanceBlend : MonoBehaviour
{
    public Color DesiredColour;
    public float DesiredIntensity;
    public GameObject CenterPoint;

    [HideInInspector]
    Color PlainLightC;
    float PlainLightI;
    float PlayerDistance;
    // Use this for initialization
    void Start()

    {
        PlainLightC = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().color;
        PlainLightI = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().intensity;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
        {
            PlayerDistance = Mathf.Abs(CenterPoint.transform.position.x - col.transform.position.x) + Mathf.Abs(CenterPoint.transform.position.z - col.transform.position.z);
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().color = Color.Lerp(DesiredColour, PlainLightC, PlayerDistance * Time.deltaTime) ;
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().intensity = Mathf.Lerp(DesiredIntensity, PlainLightI, PlayerDistance * Time.deltaTime);
        }
    }
    void OnTriggerExit(Collider col)

    {
        if (col.gameObject.tag == ("Player"))
        {
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().color = PlainLightC;
            GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>().intensity = PlainLightI;
        }
    }
}
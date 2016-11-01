using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class donttouch : MonoBehaviour
{

    void Start()
    {
        if (Application.isPlaying)
        {
            Debug.Log("");
        }
        else
        {
            Debug.Log("fixing compass material");
            GameObject.FindGameObjectWithTag("Compass").GetComponent<CompassScript>().Health.SetFloat("_Cutoff", 0);
            GameObject.FindGameObjectWithTag("Compass").GetComponent<CompassScript>().Supplies.SetFloat("_Cutoff", 0);
        }
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FogOfWarUnit))]
public class OnTriggerDefog : MonoBehaviour
{
    //Component requires FogOfWarUnit
    //[HideInInspector]
    public bool hasBeenDiscovered = false;

    void Start()
    {
        gameObject.GetComponent<FogOfWarUnit>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == ("Player") && hasBeenDiscovered == false)
        {
            gameObject.GetComponent<FogOfWarUnit>().enabled = true;
            hasBeenDiscovered = true;
        }
    }
}
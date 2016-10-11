using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FogOfWarUnit))]
public class OnTriggerDefog : MonoBehaviour
{
    //Component requires FogOfWarUnit
    [HideInInspector]
    public bool hasBeenTriggered = false;

    void Start()
    {
        gameObject.GetComponent<FogOfWarUnit>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == ("Player") && hasBeenTriggered == false)
        {
            gameObject.GetComponent<FogOfWarUnit>().enabled = true;
            hasBeenTriggered = true;
        }
    }
}

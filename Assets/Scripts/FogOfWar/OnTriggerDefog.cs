using UnityEngine;
using System.Collections;

public class OnTriggerDefog : MonoBehaviour
{
    //Component requires FogOfWarUnit
    private bool hasBeenTrigger = false;

    void Start()
    {
        gameObject.GetComponent<FogOfWarUnit>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == ("Player") && hasBeenTrigger == false)
        {
            gameObject.GetComponent<FogOfWarUnit>().enabled = true;
            hasBeenTrigger = true;
        }
 
    }


}

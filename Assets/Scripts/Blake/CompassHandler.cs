using UnityEngine;
using System.Collections;

public class CompassHandler : MonoBehaviour
{
    public GameObject LookAtGameOject;
    public GameObject PlayerObject;
    


    //PH
	void Update ()
    {
        //Transform CurrentPosition = PlayerObject.transform;
        //CurrentPosition.LookAt(LookAtGameOject.transform.position, Vector3.up);
        //CurrentPosition.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        //transform.position = CurrentPosition.position - transform.position;

        transform.LookAt(LookAtGameOject.transform.position, Vector3.up);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }
}

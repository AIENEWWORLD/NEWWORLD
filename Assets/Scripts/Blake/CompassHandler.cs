using UnityEngine;
using System.Collections;

public class CompassHandler : MonoBehaviour
{
    public GameObject LookAtGameOject;
    public GameObject PlayerObject;
    public GameObject CompassGO;


    //PH
	void Update ()
    {
        //Transform CurrentPosition = PlayerObject.transform;
        //CurrentPosition.LookAt(LookAtGameOject.transform.position, Vector3.up);
        //CurrentPosition.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

        //transform.position = CurrentPosition.position - transform.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((new Vector3(LookAtGameOject.transform.position.x, transform.position.y, LookAtGameOject.transform.position.z) - new Vector3(PlayerObject.transform.position.x, transform.position.y, PlayerObject.transform.position.z))), 100 * Time.smoothDeltaTime);

        //transform.LookAt(LookAtGameOject.transform.position, Vector3.up);
        //transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
    }
}

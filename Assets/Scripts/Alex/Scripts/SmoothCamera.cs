using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public GameObject Player;
    public Vector3 zOffset;
    public Vector3 tempOffset;
    [Range(0,1)]
    public float smooth;
	void Start ()
    {
	}
	
	void FixedUpdate () //fixed update seems to have fixed my jitters on player for some reason?
    {
        tempOffset = new Vector3(Player.transform.position.x+zOffset.x,Player.transform.position.y+zOffset.y, Player.transform.position.z+zOffset.z);
        transform.position = Vector3.Lerp(transform.position, tempOffset, 1-smooth);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0 + Player.transform.rotation.eulerAngles.y, transform.eulerAngles.z);
        Quaternion rot = Quaternion.Euler(0, Player.transform.eulerAngles.y, 0);
        transform.position = Player.transform.position + (rot * zOffset);
        transform.LookAt(Player.transform);
	}
}

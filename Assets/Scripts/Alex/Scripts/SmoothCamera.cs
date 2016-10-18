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
        Quaternion rot = Quaternion.Euler(0, Player.transform.eulerAngles.y, 0);
        transform.LookAt(Player.transform);
        tempOffset = Player.transform.position + (rot * zOffset);
        transform.position = Vector3.Lerp(transform.position, tempOffset, 1 - smooth);
    }
}


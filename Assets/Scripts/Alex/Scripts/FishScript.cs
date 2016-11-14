using UnityEngine;
using System.Collections;

public class FishScript : MonoBehaviour
{
    public float rotSpeed;
    public float forwardspeed;

    void Start()
    {
    }

    void Update()
    {
        transform.position += -transform.right * Time.deltaTime*forwardspeed;
        transform.Rotate(Vector3.up * Time.deltaTime* rotSpeed, Space.World);
    }

}

using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour
{
    public SavedInput InputGameobject;
    public GameObject Player;
    public Vector3 zOffset;
    //[HideInInspector]
    public Vector3 tempOffset;
    public Vector3 Box;
    [HideInInspector]
    public Vector3 BoxPosition;

    public float LookMaxX;
    public float LookMaxZ;
    [HideInInspector]
    public Vector3 camVelocity;
    public float smoothX;
    public float smoothZ;

    public float CamLookSmooth;

    [HideInInspector]
    public float left, right, up, down;
    [HideInInspector]
    public float targetLookAheadX;
    [HideInInspector]
    public float lookAheadDirX;
    //[HideInInspector]
    public float currentLookAheadX;
    [HideInInspector]
    public bool lookAheadStopped;
    [HideInInspector]
    public float smoothLookVelocityX;
    [HideInInspector]
    public float lookAheadDirZ;
    [HideInInspector]
    public float targetLookAheadZ;
    [HideInInspector]
    public bool lookAheadStoppedZ;
    [HideInInspector]
    public float currentLookAheadZ;
    [HideInInspector]
    public float smoothLookVelocityZ;

    [HideInInspector]
    public Vector3 PlayerVelocity;

    public bool fix = true;

    //bool withinRadius;
    //Vector3 NewLookPos;

    void Start ()
    {
        lookAheadStopped = true;
        BoxPosition = Player.transform.position;
        transform.LookAt(Player.transform);
        zOffset = (transform.position - Player.transform.position);
        tempOffset = zOffset;
        camVelocity = Vector3.zero;
        InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes").GetComponent<SavedInput>();

        left = Player.transform.position.x - Box.x;
        right = Player.transform.position.x + Box.x;
        up = Player.transform.position.z + Box.z;
        down = Player.transform.position.z - Box.z;
    }

    void LateUpdate()
    {
        CheckBox();
        Vector3 focusPosition = BoxPosition;

        PlayerVelocity = Player.GetComponent<Rigidbody>().velocity;

        if (camVelocity.x != 0 || camVelocity.z != 0)
        {
            lookAheadDirX = Mathf.Sign(camVelocity.x);
            lookAheadDirZ = Mathf.Sign(camVelocity.z);
            if (Mathf.Sign(PlayerVelocity.x) == Mathf.Sign(camVelocity.x) && PlayerVelocity.x != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * LookMaxX;

            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * LookMaxX - currentLookAheadX) / 4f;
                }
            }
            if (Mathf.Sign(PlayerVelocity.z) == Mathf.Sign(camVelocity.z) && PlayerVelocity.z != 0)
            {
                lookAheadStoppedZ = false;
                targetLookAheadZ = lookAheadDirZ * LookMaxZ;

            }
            else
            {
                if (!lookAheadStoppedZ)
                {
                    lookAheadStoppedZ = true;
                    targetLookAheadZ = currentLookAheadZ + (lookAheadDirZ * LookMaxZ - currentLookAheadZ) / 4f;
                }
            }
        }



        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, smoothX);
        currentLookAheadZ = Mathf.SmoothDamp(currentLookAheadZ, targetLookAheadZ, ref smoothLookVelocityZ, smoothZ);
        focusPosition += transform.right * currentLookAheadX;
        focusPosition += transform.up * currentLookAheadZ;
        transform.position = focusPosition + tempOffset;

        //transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position - Player.transform.position) + Player.transform.position;


        //transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position + (transform.right * -currentLookAheadX) + (transform.up * -currentLookAheadZ) - Player.transform.position) + Player.transform.position;
        //transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position - Player.transform.position) + Player.transform.position;

        //transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position - Player.transform.position) + Player.transform.position;



        if (fix)
        {
            transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position + (transform.right * -currentLookAheadX) + (transform.up * -currentLookAheadZ) - Player.transform.position) + Player.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((Player.transform.position - transform.position ).normalized), CamLookSmooth * Time.smoothDeltaTime);

        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position + (transform.right * -currentLookAheadX) + (transform.up * -currentLookAheadZ) - Player.transform.position) + Player.transform.position;

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((Player.transform.position - transform.position).normalized), CamLookSmooth * Time.smoothDeltaTime);
            }
            else
            {
                transform.position = Quaternion.Euler(Player.transform.eulerAngles) * (transform.position - Player.transform.position) + Player.transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((new Vector3(Player.transform.position.x+currentLookAheadX,Player.transform.position.y,Player.transform.position.z+currentLookAheadZ) - transform.position).normalized), CamLookSmooth * Time.smoothDeltaTime);
            }
        }
    }

    void resetBox()
    {
        left = Player.transform.position.x - Box.x;
        right = Player.transform.position.x + Box.x;
        up = Player.transform.position.z + Box.z;
        down = Player.transform.position.z - Box.z;
        BoxPosition = Player.transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(BoxPosition, Box*2);
    }

    void CheckBox()// smooth move across since this is called in the LateUpdate   new Vector2(right,0)
    {
        float VX = 0, VZ = 0;

        if (right < Player.transform.position.x)
        {
            VX = Player.transform.position.x - right;
        }
        else if (left > Player.transform.position.x)
        {
            VX = Player.transform.position.x - left;
        }

        if (up < Player.transform.position.z)
        {
            VZ = Player.transform.position.z - up;
        }
        else if (down > Player.transform.position.z)
        {
            VZ = Player.transform.position.z - down;
        }
        left += VX; right += VX;
        down += VZ; up += VZ;

        BoxPosition = new Vector3((left + right)/2,Player.transform.position.y,(up + down)/2);
        camVelocity = new Vector3(VX, 0, VZ);
    }
}


/*
 * 
 *         if (camVelocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(camVelocity.x);
            if (Mathf.Sign(InputGameobject.horizontal) == Mathf.Sign(camVelocity.x) && InputGameobject.horizontal != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * LookMaxX;

            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * LookMaxX - currentLookAheadX) / 4f;
                }
            }
        }
        if (camVelocity.z != 0)
        {
            lookAheadDirZ = Mathf.Sign(camVelocity.z);
            if (Mathf.Sign(InputGameobject.vertical) == Mathf.Sign(camVelocity.z) && InputGameobject.vertical != 0)
            {
                lookAheadStoppedY = false;
                targetLookAheadZ = lookAheadDirZ * LookMaxZ;

            }
            else
            {
                if (!lookAheadStoppedZ)
                {
                    lookAheadStoppedZ = true;
                    targetLookAheadZ = currentLookAheadZ + (lookAheadDirZ * LookMaxZ - currentLookAheadZ) / 4f;
                }
            }
        }

    */
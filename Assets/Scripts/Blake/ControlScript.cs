using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour
{
    [HideInInspector]
    public bool p_SeizeMovement = false;
    Vector3 velocity;
    Vector3 mouseWorldPos;
    public float movementSpeed;
    public float supplyDeincrement;
    public float supplyAmount;
    public float maxSupply;
    public float playerHealth;
    public float maxHealth;
    public StatsScript statsScript;
    //Here<-
    public Rigidbody t_Body;

    public float fallspeed = -20;
    public bool grounded = true;

    public GameObject characterModel;
    public BoxCollider meleeHitbox;
    public GameObject bulletPrefab;

    // public GameObject defogBreadCrumbPrefab;

    //Placeholder for Percentage Fog Tracking
    public GameObject[] VistaObjects;
    [HideInInspector]
    public float visionRadius;
    [HideInInspector]
    public bool FacingDirection = false;
    [HideInInspector]
    public bool NotMoving = false;

    float meleeTimer;
    float reloadTimer;
    float distanceFromCamera;
    //Set in Start
    float percentageMapDiscovered;
    Plane m_plane;
    Ray m_ray;

    float l_Time = 0.0f;
    Vector3 currentPosition = new Vector3();

    GameObject InputGameobject;
    SmoothCamera SmoothCameraObj;

    public float rotLR = 0;
    public float smooth = 5;
    public float maxRotSpeed = 2;
    public float rotationSpeed = 2;
    Vector3 tmpvec;
    float sqrMaxVel;

    void checkDiscoveredPercentage()
    {
        //Using InvokeRepeating method to update only update percentage every x second rather than each update cycle
        int vistaNumber = VistaObjects.Length;
        int discoveredVistas = 0;
        for (int itr = 0; itr < vistaNumber; itr++)
        {
            GameObject visRef = VistaObjects[itr];
            if (visRef.GetComponent<OnTriggerDefog>().hasBeenTriggered == true)
            {
                discoveredVistas++;
            }

            if (discoveredVistas != 0 && VistaObjects.Length != 0)
            {
                percentageMapDiscovered = (discoveredVistas / vistaNumber) * 100.0f;
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        supplyAmount = maxSupply;

        playerHealth = maxHealth;
        m_plane = new Plane(Vector3.up, 0);
        meleeHitbox = GetComponentInChildren<BoxCollider>();
        meleeHitbox.enabled = false;
        percentageMapDiscovered = 0.0f;
        currentPosition = transform.position;
        t_Body = GetComponent<Rigidbody>();
        InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes");
        SmoothCameraObj = Camera.main.GetComponent<SmoothCamera>();
    }

    void LateUpdate()
    {
        if (p_SeizeMovement == false)
        {
            //transform.RotateAround(GameObject.FindGameObjectWithTag("Player").transform.position, Vector3.up, rotLR);
        }
        else
        {
            //Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate()//https://www.reddit.com/r/Unity3D/comments/1yeegm/rigidbody_velocity_cap_for_diagonal_movement/ this helped a lot to keep the movement smooth while still normalizing the velocity
    {
        
        if (t_Body.velocity.sqrMagnitude > sqrMaxVel)
        {
            t_Body.velocity = (tmpvec.normalized * movementSpeed);
        }
            //Debug.Log(t_Body.velocity);

    }

    // Update is called once per frame
    void Update()
    {
        if (p_SeizeMovement == false)
        {
            sqrMaxVel = movementSpeed * movementSpeed;
            //make sure the player is always looking at the mouse
            //m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (m_plane.Raycast(m_ray, out distanceFromCamera))
            //{
            //    mouseWorldPos = m_ray.GetPoint(distanceFromCamera);
            //    mouseWorldPos.y = transform.position.y;
            //}

            //characterModel.transform.LookAt(mouseWorldPos);

            //character movement
            //velocity.x = Input.GetAxis("Horizontal");
            //velocity.z = Input.GetAxis("Vertical");

            if(Input.GetKeyDown(KeyCode.E))
            {
                //rotLR = 1 * rotationSpeed;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 45, transform.eulerAngles.z);
            }
            if(Input.GetKeyDown(KeyCode.Q))
            {
                //rotLR = -1 * rotationSpeed;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 45, transform.eulerAngles.z);
            }

            //if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0)
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //    //if (rotLR < maxRotSpeed)
            //    //{
            //        rotLR = Input.GetAxis("Mouse X") * rotationSpeed;
            //    //}
            //    if (rotLR > 0)
            //    {
            //        rotLR = Mathf.Lerp(rotLR, maxRotSpeed, Time.deltaTime * smooth);
            //    }
            //    else if(rotLR < 0)
            //    {
            //        rotLR = Mathf.Lerp(rotLR, -maxRotSpeed, Time.deltaTime * smooth);
            //    }
            //}
            //else
            //{
            //    Cursor.lockState = CursorLockMode.None;
            //    //rotLR = 0;
            //    
            //    if (Mathf.Abs(rotLR) < 0.1f)
            //    {
            //        rotLR = 0;
            //    }
            //    else
            //    {
            //        rotLR = Mathf.Lerp(rotLR, 0, Time.deltaTime * smooth * 5);
            //    }
            //}

            velocity.x = InputGameobject.GetComponent<SavedInput>().horizontal;
            velocity.z = InputGameobject.GetComponent<SavedInput>().vertical;
            if (!grounded)
            {
                velocity.y = fallspeed;
            }
            //velocity.y = 5;

            //velocity = Vector3.Normalize(velocity);
            //quickfix

            velocity.y = 0;
            if (velocity.x < 0)
            {
                NotMoving = false;
                FacingDirection = true;
            }
            else if (velocity.x > 0)
            {
                NotMoving = false;
                FacingDirection = false;
            }
            else if (velocity.x == 0)
            {
                NotMoving = true;
            }

            

            if (velocity.x != 0 || velocity.z != 0)
            {
                t_Body.constraints = RigidbodyConstraints.None;
                t_Body.constraints = RigidbodyConstraints.FreezeRotation;
                
                Vector3 forward = transform.forward * movementSpeed * velocity.z;
                Vector3 right = transform.right * movementSpeed * velocity.x;

                tmpvec = forward + right;

                if(!grounded)
                {
                    tmpvec.y = fallspeed;
                }

                t_Body.velocity = (tmpvec);
            }
            else
            {
                t_Body.velocity = new Vector3(0, t_Body.velocity.y, 0);
                if(grounded)
                t_Body.constraints = RigidbodyConstraints.FreezeAll;

                //t_Body.constraints = RigidbodyConstraints.FreezePositionX;
                //t_Body.constraints = RigidbodyConstraints.FreezePositionZ;
                //t_Body.constraints = RigidbodyConstraints.FreezeRotationX;
                //t_Body.constraints = RigidbodyConstraints.FreezeRotationY;
                //t_Body.constraints = RigidbodyConstraints.FreezeRotationX;
            }

            //transform.position += velocity * movementSpeed * Time.deltaTime;

            if (velocity.x != 0 || velocity.z != 0)
            {
                if (statsScript.supplies < 0)
                {
                    statsScript.supplies = 0;
                }
                else
                {
                    statsScript.supplies -= (Time.deltaTime * supplyDeincrement);
                }


            }
            if (statsScript.supplies < 0.5)
            {
                if (velocity.x != 0 || velocity.z != 0 && playerHealth > 0)
                {
                    if (playerHealth < 0)
                    {
                        playerHealth = 0;
                    }
                    else
                    {
                        playerHealth -= Time.deltaTime;
                    }

                }

            }
        }
        else
        {
            t_Body.velocity = new Vector3(0, 0, 0);
            
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}

﻿using UnityEngine;
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

    public float rotLR = 0;
    public float smooth = 5;
    public float maxRotSpeed = 2;

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
    }

    void FixedUpdate() //very bad to get input in the fixedupdate, however this is lerping the values anyway so it doesn't really matter if this returns that the key isn't down for small amounts of time.
    {
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            rotLR = Mathf.Lerp(rotLR, maxRotSpeed, Time.deltaTime * smooth);
            transform.RotateAround(GameObject.FindGameObjectWithTag("Player").transform.position, Vector3.up, rotLR);
            //if (rotLR < 0.1f)
            //{
            //    rotLR = 0.1f;
            //}
            //GameObject.FindGameObjectWithTag("Player").transform.rotation *= Quaternion.Euler(0, -90, 0);
        }

        if (Input.GetKey(KeyCode.KeypadMultiply))
        {
            rotLR = Mathf.Lerp(rotLR, -maxRotSpeed, Time.deltaTime * smooth);
            transform.RotateAround(GameObject.FindGameObjectWithTag("Player").transform.position, Vector3.up, rotLR);
            //if (rotLR > -0.1f)
            //{
            //    rotLR = -0.1f;
            //}
        }

        if (!Input.GetKey(KeyCode.KeypadMinus) && !Input.GetKey(KeyCode.KeypadMultiply))
        {
            rotLR = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (p_SeizeMovement == false)
        {
            
            /*
            //this stuff should work, just left it here uncommented

            //check if in combat
            if (GameObject.FindGameObjectWithTag("checkCombat").GetComponent<CheckinCombatScript>().Combatisenabled == false)
            {






                //do stuff if not in combat
            }

            //sets the velocity using the stored input keys
            GameObject InputGameobject = GameObject.FindGameObjectWithTag("SaveAcrossScenes");
            velocity.x = InputGameobject.GetComponent<SavedInput>().horizontal;
            velocity.z = InputGameobject.GetComponent<SavedInput>().vertical;
             */



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

                velocity.x = InputGameobject.GetComponent<SavedInput>().horizontal;
            velocity.z = InputGameobject.GetComponent<SavedInput>().vertical;
            if (!grounded)
            {
                velocity.y = fallspeed;
            }
            //velocity.y = 5;

            Vector3.Normalize(velocity);
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
                Vector3 tmpvec;
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
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}

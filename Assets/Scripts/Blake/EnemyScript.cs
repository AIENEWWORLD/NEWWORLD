using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{

    [HideInInspector]
    public bool p_SeizeMovement = false;
    public Transform playerPos;
    [HideInInspector]
    public Vector3 knockbackPos;
    public Camera FightCamera;

    [HideInInspector]
    public GameObject p_PlayerRef;

    public float knockBackDistance;

    public float aggroRadius;
    public float aggroResetRadius;
    public float movementSpeed;
    public float damage;
    public float enemyHealth;
    public float maxHealth;

    public bool isKnockedBack;
    enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    FacingDirection myDirection = new FacingDirection();
    private Vector3 l_RelativeDistance;
    private float xDif;
    private float zDif;



    // Use this for initialization
    void Start()
    {
        enemyHealth = maxHealth;
        //m_rigidBody = GetComponent<Rigidbody> ();
        isKnockedBack = false;
        p_PlayerRef = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //  p_SeizeMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<ControlScript>().p_SeizeMovement;

        if (p_SeizeMovement == false)
        {
            if (Vector3.Distance(transform.position, knockbackPos) > knockBackDistance)
            {
                isKnockedBack = false;
                //  m_rigidBody.velocity = Vector3.zero;
            }

            //ignore the player if they're too far away
            if (Vector3.Distance(transform.position, playerPos.position) > aggroResetRadius)
            {
                //reset aggro
            }

            else if (Vector3.Distance(transform.position, playerPos.position) > 0.1f && Vector3.Distance(transform.position, playerPos.position) < aggroRadius && isKnockedBack == false)
            {
                transform.position += ((playerPos.position - transform.position).normalized * Time.deltaTime * movementSpeed);

            }
        }

        //Facing direction for enemy creature
        l_RelativeDistance = transform.position - p_PlayerRef.transform.position;

        xDif = l_RelativeDistance.x;
        zDif = l_RelativeDistance.z;

        if (Mathf.Abs(xDif) < Mathf.Abs(zDif))
        {
            if (zDif <= 0)
            {
                myDirection = FacingDirection.Down;
            }
            else
            {
                myDirection = FacingDirection.Up;
            }
        }
        else
        {
            if (xDif <= 0)
            {
                myDirection = FacingDirection.Left;
            }
            else
            {
                myDirection = FacingDirection.Right;
            }
        }


        //Within this switch; animation needs to be changed.
        switch (myDirection)
        {
            case FacingDirection.Up:
               // print("FacingUp");
                break;
            case FacingDirection.Down:
                //print("FacingDown");
                break;
            case FacingDirection.Left:
                //print("FacingLeft");
                break;
            case FacingDirection.Right:
               // print("FacingRight");
                break;
        }
    }
}
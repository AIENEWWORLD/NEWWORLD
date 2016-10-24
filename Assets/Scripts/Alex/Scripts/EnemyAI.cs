using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour
{
    NavMeshAgent me;
    [HideInInspector]
    public Vector3 myPos;
    Vector3 myRotation;
    GameObject Player;
    bool RandomMove = true;
    bool ResetPos = false;
    public Vector3 BoxInner = new Vector3(3, 0, 3);
    public Vector3 BoxOuter = new Vector3(5, 0, 5);
    public float DistanceToPlayer;

    public float RandomMoveSpeed = 2;
    public float chaseSpeed = 8;

    public Vector3 newRandomPosition;

    public Vector3 Velocity = new Vector3(0,0,0);
    Vector3 prevpos;


    void Start ()
    {
        myRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        myPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        me = gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        newRandomPosition = myPos;
    }

    void Update ()
    {
        Velocity = (transform.position - prevpos) / Time.deltaTime;
        Velocity = Velocity.normalized;
        
        GetDirectionofMe();
        prevpos = transform.position;
        Vector3 temprot_ = new Vector3(myRotation.x, 0 + Player.transform.rotation.eulerAngles.y + myRotation.y, myRotation.z);
        gameObject.transform.eulerAngles = temprot_;

        if (gameObject.GetComponent<StatsScript>().dead == true)
        {
            //me.Resume();
            me.speed = chaseSpeed;
            me.destination = myPos;
        }

        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<ControlScript>().p_SeizeMovement && !gameObject.GetComponent<StatsScript>().dead)
        {
            me.Resume();
            Vector3 PlayerPos = Player.transform.position;
            float Dist = Vector3.Distance(transform.position, PlayerPos);
            if (Dist <= DistanceToPlayer)
            {
                me.speed = chaseSpeed;
                RandomMove = false;
                me.destination = PlayerPos;
            }
            else
            {
                RandomMove = true;
                ResetPos = true;
            }

            if(RandomMove)
            {
                
                
                if(Vector3.Distance(transform.position,newRandomPosition) < 2) //Might have to change this to a vector 2
                {
                    me.speed = RandomMoveSpeed;
                    /*
                     * get a new position that is above boxinner.x but below boxouter.x
                     */

                    //me.speed = 8;
                    newRandomPosition = myPos;

                    newRandomPosition.y = transform.position.y;

                    if(Random.Range(0, 2) == 0)//right
                    {
                        newRandomPosition.x = Random.Range(newRandomPosition.x + BoxInner.x, newRandomPosition.x + BoxOuter.x);
                    }
                    else //left
                    {
                        newRandomPosition.x = Random.Range(newRandomPosition.x - BoxInner.x, newRandomPosition.x - BoxOuter.x);
                    }

                    if(Random.Range(0,2) == 0)//up
                    {
                        newRandomPosition.z = Random.Range(newRandomPosition.z + BoxInner.z, newRandomPosition.z + BoxOuter.z);
                    }
                    else //down
                    {
                        newRandomPosition.z = Random.Range(newRandomPosition.z - BoxInner.z, newRandomPosition.z - BoxOuter.z);
                    }

                    //newRandomPosition.x = Random.Range(newRandomPosition.x - BoxOuter.x, newRandomPosition.x + BoxOuter.x);
                    //newRandomPosition.z = Random.Range(newRandomPosition.z - BoxOuter.z, newRandomPosition.z + BoxOuter.z);
                    me.destination = newRandomPosition;
                }
                else
                {
                    if(ResetPos)
                    {
                        me.destination = newRandomPosition;
                        ResetPos = false;
                    }
                }
            }
        }
        else
        {
            //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //me.Stop();
            //me.speed = 0;
        }
	}

    void GetDirectionofMe()
    {
        /*
         * get the players rotation and compare it to enemy current rotation, then figure out which animation to play
         */
        Velocity = transform.InverseTransformDirection(Velocity);

        if (Mathf.Abs(Velocity.x) > Mathf.Abs(Velocity.z))
        {
            if (Velocity.x > 0)
            {
                //Debug.Log("Right");
            }
            if (Velocity.x < 0)
            {
                //Debug.Log("Left");
            }
        }
        else
        {
            if (Velocity.z > 0)
            {
                //Debug.Log("Up");
            }
            if (Velocity.z < 0)
            {
                //Debug.Log("Down");
            }
        }
    }
}

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
    public bool FlipLeftRight = false;
    public bool boss;
    Vector3 prevpos;

    NavMeshPath path;

    Vector3 PlayerPos;

    Animator MyAnimator;

    SpriteRenderer thisRenderer;

    public bool FixHalfPoint = false;

    public Camera CamPos;

    void Start ()
    {
        
        MyAnimator = transform.GetChild(0).GetComponent<Animator>();
        thisRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        path = new NavMeshPath();
        myRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        myPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        me = gameObject.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        newRandomPosition = myPos;
        CamPos = Camera.main;
    }

    void Update ()
    {
        if (!boss)
        {
            Velocity = (transform.position - prevpos) / Time.deltaTime;
            //Velocity = Velocity.normalized;

            GetDirectionofMe();

            prevpos = transform.position;
            Vector3 temprot_ = new Vector3(myRotation.x, 0 + Player.transform.rotation.eulerAngles.y + myRotation.y, myRotation.z);
            gameObject.transform.eulerAngles = temprot_;

            newRandomPosition.y = transform.position.y;

            if (gameObject.GetComponent<StatsScript>().dead == true)
            {
                //me.Resume();
                me.speed = chaseSpeed;
                me.destination = myPos;
            }

            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<ControlScript>().p_SeizeMovement && !gameObject.GetComponent<StatsScript>().dead)
            {
                //me.Resume();
                PlayerPos = Player.transform.position;
                float Dist = Vector3.Distance(transform.position, PlayerPos);
                //NavMesh.CalculatePath(transform.position, PlayerPos, 1, path);
                if (Dist <= DistanceToPlayer)
                {
                    me.SetDestination(PlayerPos);
                    me.speed = chaseSpeed;
                    RandomMove = false;
                    me.destination = PlayerPos;
                }
                else
                {
                    RandomMove = true;
                    ResetPos = true;
                    me.speed = RandomMoveSpeed;
                }

                if (RandomMove)
                {


                    if (Vector3.Distance(transform.position, newRandomPosition) < 2) //Might have to change this to a vector 2
                    {
                        me.speed = RandomMoveSpeed;
                        /*
                         * get a new position that is above boxinner.x but below boxouter.x
                         */

                        //me.speed = 8;
                        newRandomPosition = myPos;

                        newRandomPosition.y = transform.position.y;

                        if (Random.Range(0, 2) == 0)//right
                        {
                            newRandomPosition.x = Random.Range(newRandomPosition.x + BoxInner.x, newRandomPosition.x + BoxOuter.x);
                        }
                        else //left
                        {
                            newRandomPosition.x = Random.Range(newRandomPosition.x - BoxInner.x, newRandomPosition.x - BoxOuter.x);
                        }

                        if (Random.Range(0, 2) == 0)//up
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
                        //Debug.Log("R");
                    }
                    else
                    {
                        if (ResetPos)
                        {
                            if (5 < Vector3.Distance(transform.position, myPos))
                            {
                                me.destination = myPos;
                            }
                            else
                            {
                                me.destination = newRandomPosition;
                            }
                            ResetPos = false;
                            //me.speed = 1;
                        }
                    }
                }
            }
            else
            {
                //RandomMove = true;
                //ResetPos = true;
                me.speed = 0;
            }
        }
        else
        {
            Vector3 temprot_ = new Vector3(myRotation.x, 0 + Player.transform.rotation.eulerAngles.y + myRotation.y, myRotation.z);
            gameObject.transform.eulerAngles = temprot_;
        }
        
    }

    void GetDirectionofMe()
    {
        /*
         * get the players rotation and compare it to enemy current rotation, then figure out which animation to play
         */
        bool zeroVel = false;
        if (Velocity == Vector3.zero)///////////////////
        {
            MyAnimator.speed = 0;
            Vector3 newDir;
            newDir = new Vector3(PlayerPos.x - transform.position.x, 0, PlayerPos.z - transform.position.z);
            Velocity = newDir;
            zeroVel = true;
        }


        Velocity = transform.InverseTransformDirection(Velocity);

        float VX = Mathf.Abs(Velocity.x);
        float VZ = Mathf.Abs(Velocity.z);
        FixHalfPoint = false;
        float fixNum = 0.6f;

        //check if x is within a range of z
        if (VX >= VZ-fixNum && VX <= VZ+fixNum && VX + VZ > 1)
        {
            FixHalfPoint = true;
        }
        if (VX > VZ && VX + VZ > 1 || FixHalfPoint == true)
        {
            if (zeroVel == false)
            {
                MyAnimator.speed = 1;
            }
            if (Velocity.x > 0)
            {
                //Debug.Log("Right");
                MyAnimator.Play("Side");
                if (!FlipLeftRight)
                {
                    thisRenderer.flipX = false;
                }
                else
                {
                    thisRenderer.flipX = true;
                }
            }
            if (Velocity.x < 0)
            {
                //Debug.Log("Left");
                MyAnimator.Play("Side");
                
                if (!FlipLeftRight)
                {
                    thisRenderer.flipX = true;
                }
                else
                {
                    thisRenderer.flipX = false;
                }
            }
        }
        else if(VX < VZ && VX + VZ > 1)
        {
            if (zeroVel == false)
            {
                MyAnimator.speed = 1;
            }
            if (Velocity.z > 0)
            {
                //Debug.Log("Up");
                MyAnimator.Play("Back");
            }
            if (Velocity.z < 0)
            {
                //Debug.Log("Down");
                
                MyAnimator.Play("Front");
                
            }
        }
    }
}

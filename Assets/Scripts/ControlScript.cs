using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour
{
	Vector3 velocity;
	Vector3 mouseWorldPos;
	public float movementSpeed;
	public float endurance;
    public float maxSupply;
	public float playerHealth;
	public float maxHealth;
	public float currentAmmo;
	public float maxAmmo;
	public float meleeAttackDuration;
    public float reloadDuration;
    public float AttackForce;

    [HideInInspector]
    public bool FacingDirection = false;

	float meleeTimer;
	float reloadTimer;
	float distanceFromCamera;

	public GameObject characterModel;
	public BoxCollider meleeHitbox;
	public GameObject bulletPrefab;

	Plane m_plane;
	Ray m_ray;

	// Use this for initialization
	void Start ()
    {
		currentAmmo = maxAmmo;
		playerHealth = maxHealth;
		m_plane = new Plane (Vector3.up, 0);
		meleeHitbox = GetComponentInChildren<BoxCollider> ();
		meleeHitbox.enabled = false;

    }
	
	// Update is called once per frame
	void Update ()
	{
		//make sure the player is always looking at the mouse
		m_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (m_plane.Raycast (m_ray, out distanceFromCamera)) 
		{
			mouseWorldPos = m_ray.GetPoint (distanceFromCamera);
			mouseWorldPos.y = transform.position.y;
		}

		characterModel.transform.LookAt (mouseWorldPos);

		//character movement
		velocity.x = Input.GetAxis ("Horizontal");
		velocity.z = Input.GetAxis ("Vertical");


        Vector3.Normalize(velocity);
        //quickfix


        velocity.y = 0;
        if(velocity.x < 0)
        {
            FacingDirection = true;
        }
        else if (velocity.x > 0)
        {
            FacingDirection = false;
        }
        transform.position += velocity * movementSpeed * Time.deltaTime;
        //Placeholder for shh
        if (velocity.x != 0 || velocity.z != 0 )
        {
            if (endurance < 0)
            {
                endurance = 0;
            }
            else
            {
                endurance -= Time.deltaTime;
            }

        }
        if (endurance < 0.5)
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

		//attacking
		meleeTimer -= Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Mouse0)) 
		{
			Debug.Log ("melee");
			meleeHitbox.enabled = true;
			meleeTimer = meleeAttackDuration;
		}

		reloadTimer -= Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Mouse1) && reloadTimer <= 0) 
		{
			if (currentAmmo > 0) 
			{
				Instantiate (bulletPrefab, transform.position, characterModel.transform.rotation);
				currentAmmo--;
				reloadTimer = reloadDuration;
			}
		}

		if (meleeTimer <= 0)
		{
			meleeTimer = 0;
			meleeHitbox.enabled = false;
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "enemy")
		{
			EnemyScript thisEnemy = collision.gameObject.GetComponentInParent<EnemyScript> ();
			if (meleeHitbox.enabled == true) 
			{
				thisEnemy.enemyHealth -= 1;
				thisEnemy.m_rigidBody.AddForce ((thisEnemy.transform.position - transform.position).normalized * Time.deltaTime * AttackForce);
				thisEnemy.isKnockedBack = true;
				thisEnemy.knockbackPos = transform.position;
			}

			if (thisEnemy.enemyHealth <= 0)
			{
				Destroy (thisEnemy.gameObject);
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public Transform playerPos;
    [HideInInspector]
	public Vector3 knockbackPos;

	public float knockBackDistance;

	public float aggroRadius;
	public float aggroResetRadius;
	public float movementSpeed;
	public float damage;
	public float enemyHealth;
	public float maxHealth;

	public bool isKnockedBack;

	public Rigidbody m_rigidBody;

	// Use this for initialization
	void Start () 
	{
		enemyHealth = maxHealth;
		m_rigidBody = GetComponent<Rigidbody> ();
		isKnockedBack = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Vector3.Distance (transform.position, knockbackPos) > knockBackDistance) 
		{
			isKnockedBack = false;
			m_rigidBody.velocity = Vector3.zero;
		}

		//ignore the player if they're too far away
		if (Vector3.Distance (transform.position, playerPos.position) > aggroResetRadius) 
		{
			//reset aggro
		}

		else if (Vector3.Distance (transform.position, playerPos.position) > 0.1f && Vector3.Distance (transform.position, playerPos.position) < aggroRadius && isKnockedBack == false) 
		{
			transform.position += ((playerPos.position - transform.position).normalized * Time.deltaTime * movementSpeed);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player") 
		{
			ControlScript thisPlayer = collision.gameObject.GetComponentInParent<ControlScript> ();
			thisPlayer.playerHealth -= damage;
		}
	}
}

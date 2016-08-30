using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour 
{
	Vector3 velocity;
	public float projectileSpeed;

	// Use this for initialization
	void Start () 
	{
		Destroy (this.gameObject, 10);
		velocity += transform.forward * projectileSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.localPosition += velocity * Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "enemy")
		{
			EnemyScript thisEnemy = collision.gameObject.GetComponentInParent<EnemyScript> ();
			thisEnemy.enemyHealth -= 1;
			if (thisEnemy.enemyHealth <= 0)
			{
				Destroy (thisEnemy.gameObject);
			}
			Destroy (this.gameObject);
		}
	}
}

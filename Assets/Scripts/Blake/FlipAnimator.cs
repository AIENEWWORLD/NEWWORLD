using UnityEngine;
using System.Collections;

public class FlipAnimator : MonoBehaviour
{
    GameObject Player;
    Animator thisAnimator;
    SpriteRenderer thisRenderer;

	void Start ()
    {
        thisAnimator = GetComponent<Animator>();
        thisRenderer = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        thisRenderer.flipX = Player.GetComponent<ControlScript>().FacingDirection;

        if (Player.GetComponent<ControlScript>().NotMoving == true)
        {
            thisAnimator.speed = 0;
        }
        else
        {
            thisAnimator.speed = 1.5f;
        }

    }
}

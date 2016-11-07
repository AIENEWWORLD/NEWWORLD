using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ground
{
    public string myTag;
    public AudioClip[] FootSteps;
}

public class FootsetpSounds : MonoBehaviour
{
    public List<Ground> AudioClips;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        for (int i = 0; i < AudioClips.Count; i++)
        {
            if (col.gameObject.tag == AudioClips[i].myTag)
            {
                gameObject.GetComponent<ControlScript>().currSounds = AudioClips[i].FootSteps;
            }
        }
    }
}

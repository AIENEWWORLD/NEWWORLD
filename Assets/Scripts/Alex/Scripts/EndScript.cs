using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScript : MonoBehaviour 
{
	VistaTentTracker VTT;
	public bool CompletedText = false;
	public RawImage rawimg;
	public float transSpeed = 1;
	public float TransBack = 3;
	CameraSwitcher Cswitcher;

	void Start () 
	{
		VTT = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<VistaTentTracker>();
		Cswitcher = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<CameraSwitcher>();
		rawimg.enabled = false;
	}

	//make the end display text then transition to an image then transition back to the map camera after x time

	void Update () 
	{
		if (VTT.AllLandmarksDiscovered == true && CompletedText) //if (VTT.AllLandmarksDiscovered == true && CompletedText) 
		{
			GameObject.FindGameObjectWithTag ("Player").GetComponent<ControlScript> ().p_SeizeMovement = false;
			Transitions T = GameObject.FindGameObjectWithTag("checkCombat").GetComponent<Transitions>();
			float tSpeed = T.transSpeed;
			T.transSpeed = transSpeed;
			T.TransCam = Camera.main;
			T.trans = true;
			rawimg.enabled = true;
			T.transSpeed = tSpeed;
			StartCoroutine(TransBackAfterTime (TransBack));

			CompletedText = false;
		}
	}
	IEnumerator TransBackAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		Transitions T = GameObject.FindGameObjectWithTag("checkCombat").GetComponent<Transitions>();
		float tSpeed = T.transSpeed;
		T.transSpeed = transSpeed;
		T.TransCam = Cswitcher.MapCamera;
		T.trans = true;
		Cswitcher.MapCamera.enabled = true;
		Cswitcher.MainCamera.enabled = false;
		Cswitcher.MainCameraActive = false;
		rawimg.enabled = false;
		Destroy (this);
	}
}

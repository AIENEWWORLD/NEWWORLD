using UnityEngine;
using System.Collections;

public class TextureCycler : MonoBehaviour
{
    public Material[] AnimatedFrames;
    Renderer thisObject;
    public float FrameInterval;
    private int NumberOfFrames;
    private int CurrentFrameIndex;
    private Material thisMaterial;

    private float t_Time;

    public bool enable = false;

	// Use this for initialization
	void Start ()
    {
        enable = true;
        CurrentFrameIndex = 0;
        thisObject = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            t_Time += Time.deltaTime;
            AnimatedFrames.GetLength(NumberOfFrames);

            if (t_Time > FrameInterval)
            {

                if (CurrentFrameIndex > NumberOfFrames + 1)
                {
                    CurrentFrameIndex = 0;
                }
                else
                {
                    CurrentFrameIndex += 1;

                }
                thisObject.material = AnimatedFrames[CurrentFrameIndex];
                t_Time = 0.0f;
            }
        }
        else
        {
            thisObject.material = AnimatedFrames[0];
            CurrentFrameIndex = 0;
        }
    }
}

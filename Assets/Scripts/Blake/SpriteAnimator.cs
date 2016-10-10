using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] AnimatedFrames;
    Renderer thisObject;
    public float FrameInterval;
    private int NumberOfFrames;
    private int CurrentFrameIndex;
    private Sprite thisMaterial;

    private float t_Time;

    public bool enable = false;

    void Start()
    {
        enable = true;
        CurrentFrameIndex = 0;
        thisObject = gameObject.GetComponent<Renderer>();
    }

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
                thisObject.GetComponent<SpriteRenderer>().sprite = AnimatedFrames[CurrentFrameIndex];
                t_Time = 0.0f;
            }
        }
        else
        {
            thisObject.GetComponent<SpriteRenderer>().sprite = AnimatedFrames[0];
            CurrentFrameIndex = 0;
        }
    }
}

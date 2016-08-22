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


    // Use this for initialization
    void Start()
    {

        CurrentFrameIndex = 0;
        thisObject = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
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
}

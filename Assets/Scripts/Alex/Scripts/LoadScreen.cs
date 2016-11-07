using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    public int ScenetoLoad = 3;

    AsyncOperation op;
    public Slider LoadSlider;
    public float LoadingDelay = 1;
    public float LoadSmoothing = 0.5f; //I have no idea if I should be doing this...

	void Awake ()
    {
        Time.timeScale = 1;
        op = null;
        StartCoroutine(LoadLevel(LoadingDelay));
    }
	
	void Update ()
    {
        if (op != null)
        {
            LoadSlider.value = op.progress;

            //if (op.progress < 0.95f)
            //{
            //    LoadSlider.value = Mathf.Lerp(LoadSlider.value, op.progress, Time.deltaTime* LoadSmoothing);
            //}
            //else
            //{
            //    LoadSlider.value = 1;
            //}
        }
    }

    IEnumerator LoadLevel(float timer)
    {
        yield return new WaitForSeconds(timer);

        op = SceneManager.LoadSceneAsync(ScenetoLoad);
    }
}

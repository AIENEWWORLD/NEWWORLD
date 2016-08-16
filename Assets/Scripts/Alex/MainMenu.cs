using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public RawImage arrow;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, EventSystem.current.currentSelectedGameObject.transform.localPosition.y, 0);
    }

    public void MouseOver(Button button)
    {
        button.Select();
    }
    public void ClickNewGame(Button button)
    {
        SceneManager.LoadScene(2);
    }
    public void ClickLoadGame(Button button)
    {
        //do magic

    }
    public void ClickOptions(Button button)
    {
        SceneManager.LoadScene(1);
    }
    public void ClickQuit(Button button)
    {
        Application.Quit();
    }
}

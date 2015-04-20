using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class SceneController : MonoBehaviour
{
    public AudioSource PlayAudio;
    public AudioSource ButtonClickAudio;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
    }


    // Update is called once per frame


    public IEnumerator IStartGame(float secondsToWait, string sceneToLoad)
    {
        ButtonClickAudio.Play();
        PlayAudio.Play();
        yield return new WaitForSeconds(secondsToWait);
        Debug.LogFormat("Loading Level: {0}", sceneToLoad);
        Application.LoadLevel(sceneToLoad);
    }

    public void StartGame(string sceneToLoad)
    {
        GameObject.FindObjectsOfType<Button>().ToList().ForEach((button) =>
        {
            button.interactable = false;
        }
        );
        StartCoroutine(IStartGame(2.5f, sceneToLoad));
    }

    public Button PauseButton;
    public CanvasRenderer PausePanel;

    public void SetPause()
    {
        this.PauseButton.interactable = false;
        Time.timeScale = 0;
        this.PausePanel.gameObject.SetActive(true);
    }

    public void Resume()
    {
        this.PauseButton.interactable = true;
        this.PausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

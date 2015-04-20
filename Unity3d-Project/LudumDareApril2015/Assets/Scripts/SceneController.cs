using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class SceneController : MonoBehaviour
{
    public AudioSource PlayAudio;
    public AudioSource ButtonClickAudio;
    private bool HastToStartGame = false;
    private float SecondsElapsedSinceStartRequest = 0;
    private float SecondsOnStartRequest = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SecondsElapsedSinceStartRequest = Time.time - SecondsOnStartRequest;
        if (Input.GetKeyUp(KeyCode.Return))
        {
            StartGame();
        }
        if (this.HastToStartGame && SecondsElapsedSinceStartRequest > 2.5)
        {
            Application.LoadLevel("GameScene");
        }
    }

    public void StartGame()
    {
        GameObject.FindObjectsOfType<Button>().ToList().ForEach((button) => 
        {
            button.interactable = false;
        }
        );
        this.SecondsOnStartRequest = Time.time;
        ButtonClickAudio.Play();
        PlayAudio.Play();
        this.HastToStartGame=true;
    }
}

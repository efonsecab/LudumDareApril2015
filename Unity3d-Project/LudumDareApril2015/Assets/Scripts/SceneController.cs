using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public AudioSource PlayAudio;
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
        this.SecondsOnStartRequest = Time.time;
        PlayAudio.Play();
        this.HastToStartGame=true;
    }
}

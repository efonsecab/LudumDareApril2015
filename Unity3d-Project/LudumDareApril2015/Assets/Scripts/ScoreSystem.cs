using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    public int CurrentScore = 0;
    public int SecondsAlive = 0;
    public Text ScoreTextElement;
    public Text SecondsAliveTextElement;
    private float firstTimeUpdated;
    public int EnemiesDefeated = 0;
    public Text EnemiesDefeatedTextElement;
	// Use this for initialization
	void Start () {
        this.firstTimeUpdated = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        this.SecondsAlive = Mathf.FloorToInt(Time.time - this.firstTimeUpdated);
	}

    void OnGUI()
    {
        this.ScoreTextElement.text = this.CurrentScore.ToString();
        this.SecondsAliveTextElement.text = this.SecondsAlive.ToString();
        this.EnemiesDefeatedTextElement.text = this.EnemiesDefeated.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        this.CurrentScore += pointsToAdd;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {
    public int CurrentScore = 0;
    public Text ScoreTextElement;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        this.ScoreTextElement.text = this.CurrentScore.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        this.CurrentScore += pointsToAdd;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour {

    public float highscore { get { return _highscore; } }
    private float _highscore;

    public float latestscore { get { return _latestScore; } }
    private float _latestScore;

	void Awake () {
        if(FindObjectsOfType<HighscoreManager>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        _highscore = 0;
        _latestScore = 0;
	}

    public void AddScore(int score)
    {
        if(score > highscore)
        {
            this._highscore = score;
        }

        _latestScore = score;
    }
}

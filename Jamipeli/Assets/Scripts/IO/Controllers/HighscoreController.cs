using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour {

    private HighscoreManager manager;

    public Text highScore;
    public Text lastScore;

    void Start()
    {
        this.manager = FindObjectOfType<HighscoreManager>();
    }

    void Update()
    {
        float hs = 0;
        float s = 0;

        if(manager != null)
        {
            hs = manager.highscore;
            s = manager.latestscore;
        }

        highScore.text = "Highscore: " + hs;
        lastScore.text = "Score: " + s;
    }
}

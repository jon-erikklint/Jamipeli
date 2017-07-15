﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private SceneChanger changer;
    private HighscoreManager highscore;

    public int points { get { return _points; } }
    private int _points;

    void Start()
    {
        this.changer = GetComponent<SceneChanger>();
        this.highscore = FindObjectOfType<HighscoreManager>();
        this._points = 0;
    }

    void Update()
    {
        _points++;
    }

    public void EndGame()
    {
        highscore.AddScore(_points);
        changer.LoadScene("End");
    }
}

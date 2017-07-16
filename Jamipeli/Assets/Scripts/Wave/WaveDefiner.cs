using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefiner : MonoBehaviour {

    public float waveLowtime;
    public float timeForOneKill;

    private Wavespawner spawner;
    private GameManager game;

    public int waveNumber { get { return _waveNumber; } }
    private int _waveNumber;
    private float startTime;
    private bool spawning;

	void Start () {
        spawner = GetComponent<Wavespawner>();
        game = FindObjectOfType<GameManager>();

        _waveNumber = 0;
        spawning = false;

        startTime = float.MinValue;
	}
	
	void Update () {
        if (spawning)
        {
            return;
        }

        if (!spawning && Time.time >= startTime + waveLowtime)
        {
            SpawnWave();
        }
	}

    void SpawnWave()
    {
        _waveNumber++;

        Wave wave = new Wave(10 / _waveNumber, (int)Mathf.Ceil(Mathf.Log(_waveNumber * 2)), new DoOnWaveEnded(WaveEnded));
        wave.AddEnemy("soldier", WaveEnemyAmount());

        spawning = true;
        startTime = Time.time;

        spawner.Spawn(wave);
    }

    private int WaveEnemyAmount()
    {
        return _waveNumber;
    }

    public void WaveEnded()
    {
        int pointsFromWave = waveNumber * WaveEnemyAmount() * (int) (timeForOneKill / (Time.time - startTime));
        game.AddPoints(pointsFromWave);

        spawning = false;

        startTime = Time.time;
    }
}

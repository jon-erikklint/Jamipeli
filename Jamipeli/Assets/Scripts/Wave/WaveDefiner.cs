using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefiner : MonoBehaviour {

    public float waveLowtime;
    public float timeForOneKill;

    private Wavespawner spawner;
    private GameManager game;
    private WaveEndExtraManager extra;

    public int waveNumber { get { return _waveNumber; } }
    private int _waveNumber;
    private float startTime;
    public bool spawning { get { return _spawning; } }
    private bool _spawning;

	void Start () {
        spawner = GetComponent<Wavespawner>();
        game = FindObjectOfType<GameManager>();
        extra = FindObjectOfType<WaveEndExtraManager>();

        _waveNumber = 0;
        _spawning = false;

        startTime = float.MinValue;
	}
	
	void Update () {
        if (_spawning)
        {
            return;
        }

        if (!_spawning && TimeToNextWave() <= 0)
        {
            SpawnWave();
        }
	}

    void SpawnWave()
    {
        extra.OnWaveStart();
        _waveNumber++;

        Wave wave = new Wave(10 / _waveNumber, (int)Mathf.Ceil(Mathf.Log(_waveNumber * 2)), new DoOnWaveEnded(WaveEnded));
        wave.AddEnemy("soldier", WaveEnemyAmount());

        _spawning = true;
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

        _spawning = false;
        extra.OnWaveEnd();
        startTime = Time.time;
    }

    public float TimeToNextWave()
    {
        return startTime + waveLowtime - Time.time;
    }
}

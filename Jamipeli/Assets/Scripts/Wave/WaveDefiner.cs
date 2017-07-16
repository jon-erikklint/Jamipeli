using System;
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

        Wave wave = new Wave(10 / _waveNumber, _waveNumber / 2 + 1, new DoOnWaveEnded(WaveEnded));
        ChooseEnemies(wave);

        _spawning = true;
        startTime = Time.time;

        spawner.Spawn(wave);
    }

    private void ChooseEnemies(Wave wave)
    {
        wave.AddEnemy("soldier", WaveSoldiers());
        int addAmount = WaveRocketeers();
        if(addAmount != 0)
        {
            wave.AddEnemy("rocketeer", addAmount);
        }
        addAmount = WaveDynamiters();
        if (addAmount != 0)
        {
            wave.AddEnemy("dynamiter", addAmount);
        }
        addAmount = WaveSuiciders();
        if (addAmount != 0)
        {
            wave.AddEnemy("suicider", addAmount);
        }
        addAmount = WaveSprayers();
        if (addAmount != 0)
        {
            wave.AddEnemy("sprayer", addAmount);
        }
        if (BossLevel())
        {
            wave.AddEnemy("boss", Math.Max(1, waveNumber / 18));
        }
    }

    private int WaveSoldiers()
    {
        return 1 + (_waveNumber - 1) * 2;
    }

    private int WaveRocketeers()
    {
        return waveNumber % 2 == 0 && waveNumber > 2 ? waveNumber / 2 : 0;
    }

    private int WaveDynamiters()
    {
        return waveNumber > 3 && !(waveNumber % 2 == 0 && waveNumber % 4 != 0) ? waveNumber / 2 : 0;
    }

    private int WaveSuiciders()
    {
        return waveNumber > 1 ? (_waveNumber - 1) * 3 : 0;
    }

    private int WaveSprayers()
    {
        return waveNumber > 4 && waveNumber % 3 != 0 ? waveNumber : 0;
    }

    private bool BossLevel()
    {
        return waveNumber % 6 == 0;
    }

    private int BossLevelAmount()
    {
        return (waveNumber / 6 - 1) * 2; 
    }

    private int WaveEnemies()
    {
        int sum = 0;
        sum += WaveSoldiers();
        sum += WaveRocketeers();
        sum += WaveSprayers();
        sum += WaveSuiciders();
        sum += WaveDynamiters();

        return sum;
    }

    public void WaveEnded()
    {
        int pointsFromWave = waveNumber * WaveEnemies() * (int) (timeForOneKill / (Time.time - startTime));
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

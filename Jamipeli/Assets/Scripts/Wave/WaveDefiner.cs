using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefiner : MonoBehaviour {

    public float waveLowtime;

    private Wavespawner spawner;

    public int waveNumber { get { return _waveNumber; } }
    private int _waveNumber;
    private float startTime;
    private bool spawning;

	void Start () {
        spawner = GetComponent<Wavespawner>();

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
        wave.AddEnemy("soldier", _waveNumber);

        spawning = true;

        spawner.Spawn(wave);
    }

    public void WaveEnded()
    {
        spawning = false;

        startTime = Time.time;
    }
}

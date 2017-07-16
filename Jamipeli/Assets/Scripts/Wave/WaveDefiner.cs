using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefiner : MonoBehaviour {

    public float waveLowtime;

    private Wavespawner spawner;

    private int waveNumber;
    private float startTime;
    private bool spawning;

	void Start () {
        spawner = GetComponent<Wavespawner>();

        waveNumber = 0;
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
        waveNumber++;
        Wave wave = new Wave(10 / waveNumber, (int)Mathf.Ceil(Mathf.Log(waveNumber * 2)), new DoOnWaveEnded(WaveEnded));
        wave.AddEnemy("soldier", waveNumber);

        spawning = true;

        spawner.Spawn(wave);
    }

    public void WaveEnded()
    {
        spawning = false;

        startTime = Time.time;
    }
}

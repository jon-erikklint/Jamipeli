using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    private List<Wavespawner> wavespawners;

    private int waveNumber;
    public int waveNo { get { return waveNumber; } }

    private Wave wave;

    private void Awake()
    {
        wavespawners = new List<Wavespawner>();
        Wavespawner[] spawnerTable = FindObjectsOfType<Wavespawner>();
        foreach (var spawner in spawnerTable)
            wavespawners.Add(spawner);
        waveNumber = 1;
    }

    public void StartWave()
    {
        if (wave == null)
            wave = NextWave();
        GetRandomWavespawner().Spawn(wave);
    }

    private void WaveEnded()
    {
        waveNumber++;
        NextWave();
    }

    private Wave NextWave()
    {
        return null;
    }

    private Wavespawner GetRandomWavespawner()
    {
        return wavespawners[Mathf.FloorToInt(Random.value * wavespawners.Count)];
    }
}

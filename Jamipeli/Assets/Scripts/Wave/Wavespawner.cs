﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavespawner : MonoBehaviour {

    public List<string> names;
    public List<GameObject> prefabs;

    public GameObject spawnPoints;

    private Creator creator;
    private EnemyDeathManager deathManager;

    private Dictionary<string, GameObject> enemies;
    private List<Transform> spawns;

    private Timer miniWaveTimer;
    private int spawned;
    private bool spawning;
    private Wave wave;

	void Start () {
        creator = FindObjectOfType<Creator>();
        deathManager = FindObjectOfType<EnemyDeathManager>();

        enemies = new Dictionary<string, GameObject>();
        for(int i = 0; i < names.Count; i++)
        {
            enemies.Add(names[i], prefabs[i]);
        }

        spawns = new List<Transform>();

        for (int i = 0 ; i < spawnPoints.transform.childCount; i++)
        {
            spawns.Add(spawnPoints.transform.GetChild(i));
        }
	}

    public void Spawn(Wave wave)
    {
        this.wave = wave;

        GameObject timerObject = new GameObject();
        miniWaveTimer = timerObject.AddComponent<Timer>();
        miniWaveTimer.AddAction(new DoOnTimeout(SpawnWave));
        miniWaveTimer.purpose = "Trigger timer";
        timerObject.transform.parent = transform;

        spawned = 0;
        spawning = true;
        SpawnWave();
    }


    public void SpawnWave()
    {
        List<Transform> availableSpawns = new List<Transform>(spawns);

        int leftToSpawn = wave.spawnAmount;
        while (leftToSpawn > 0 && availableSpawns.Count > 0 && wave.enemies > 0)
        {
            GameObject enemy = RandomEnemy();
            enemy.GetComponent<Enemy>().Destroyed += deathManager.OnEnemyDeath;
            //enemy.GetComponent<Enemy>().AddDestroyListener(new DoOnDestroy(deathManager.OnEnemyDeath));
            SetSpawn(enemy, availableSpawns);
            spawned++;
            leftToSpawn--;
        }

        if(wave.enemies != 0)
        {
            miniWaveTimer.StartTimer(wave.spawnRate);
        }
        else
        {
            spawning = false;
        }
    }

    private GameObject RandomEnemy()
    {
        int rand = Rand(wave.enemies);
        GameObject enemyPrefab = enemies[wave.NthEnemy(rand)];

        return creator.Instantiate(enemyPrefab, Vector3.zero, transform.rotation);
    }

    private void SetSpawn(GameObject enemy, List<Transform> availableSpawns)
    {
        Transform spawn = availableSpawns[Rand(availableSpawns.Count)];
        enemy.transform.position = spawn.position;
        enemy.transform.rotation = spawn.rotation;
        availableSpawns.Remove(spawn);
    }

    private int Rand(int max)
    {
        return (int) Mathf.Floor(Random.value * max);
    }

    public void SpawnedDie()
    {
        spawned--;
        if(!spawning && spawned == 0)
        {
            wave.EndWave();
        }
    }
}

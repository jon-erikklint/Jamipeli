using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DoOnWaveEnded();

public class Wave  {

    public Dictionary<string, int> enemyTypes;

    public float spawnRate;
    public int spawnAmount;

    public int enemies;

    public DoOnWaveEnded WaveEnded;

	public Wave(float spawnRate = 10, int spawnAmount = 0)
    {
        this.spawnRate = spawnRate;
        this.spawnAmount = spawnAmount;

        enemyTypes = new Dictionary<string, int>();
        enemies = 0;
    }

    public void AddEnemy(string name, int amount)
    {
        if (enemyTypes.ContainsKey(name)) amount += enemyTypes[name];
        enemyTypes.Add(name, amount);
        enemies += amount;
    }

    public string NthEnemy(int n)
    {
        foreach(string key in enemyTypes.Keys)
        {
            n -= enemyTypes[key];
            if(n <= 0)
            {
                if(enemyTypes[key] == 1)
                {
                    enemyTypes.Remove(key);
                }
                else
                {
                    enemyTypes.Add(key, enemyTypes[key] - 1);
                }

                enemies--;
                return key;
            }
        }

        return "";
    }

    private void EndWave()
    {
        WaveEnded();
    }

}

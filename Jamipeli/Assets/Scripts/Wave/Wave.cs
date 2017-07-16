using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DoOnWaveEnded();

public class Wave  {

    public Dictionary<string, int> enemyTypes;

    public float spawnRate;
    public int spawnAmount;

    public int enemies;

    public delegate DoOnWaveEnded WaveEnded();
    event DoOnWaveEnded End;
	public Wave(float spawnRate, int spawnAmount, DoOnWaveEnded doOnEnd)
    {
        End += doOnEnd;

        this.spawnRate = spawnRate;
        this.spawnAmount = spawnAmount;

        enemyTypes = new Dictionary<string, int>();
        enemies = 0;
    }

    public void AddEnemy(string name, int amount)
    {
        if (enemyTypes.ContainsKey(name))
        {
            enemyTypes[name] += amount;
        }
        else
        {
            enemyTypes.Add(name, amount);
        }
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
                    enemyTypes[key] --;
                }

                enemies--;
                return key;
            }
        }

        return "";
    }

    public void EndWave()
    {
        End();
    }

}

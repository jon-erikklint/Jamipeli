using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathManager : MonoBehaviour {

    private PlayerMover player;
    private Wavespawner wavespawner;
    private GameManager gameManager;

	void Start () {
        this.player = FindObjectOfType<PlayerMover>();
        this.wavespawner = FindObjectOfType<Wavespawner>();
        this.gameManager = FindObjectOfType<GameManager>();
	}
	
	public void OnEnemyDeath (Enemy died) {
        wavespawner.SpawnedDie();
        gameManager.AddPoints(died.points);
        player.GetEnemyCharge();
	}
}

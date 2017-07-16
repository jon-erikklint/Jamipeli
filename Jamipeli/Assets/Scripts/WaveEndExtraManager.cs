using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEndExtraManager : MonoBehaviour {

    public int healthAmount;
    public int slowAmount;

    private PlayerMover player;
    private WaveDefiner wave;
    private GameManager game;

    public bool extraAvailable { get { return extra; } }
    private bool extra;

	void Start () {
        player = FindObjectOfType<PlayerMover>();
        game = FindObjectOfType<GameManager>();
        wave = FindObjectOfType<WaveDefiner>();

        extra = false;
	}
	
	public void OnWaveEnd()
    {
        extra = true;
    }

    public void OnWaveStart()
    {
        if (extra)
        {
            AddPoints();
        }
        extra = false;
    }

    public void ChooseExtra(int choice)
    {
        if (!extra) return;
        if (choice == 1)
        {
            ExtraHealth();
        }
        else if(choice == 2)
        {
            ExtraGlobalSlow();
        }
        else if(choice == 0)
        {
            AddPoints();
        }

        extra = false;
    }

    private void AddPoints()
    {
        game.AddPoints(wave.waveNumber);
    }

    private void ExtraHealth()
    {
        player.health.Heal(healthAmount);
    }

    private void ExtraGlobalSlow()
    {
        player.globalSlows += slowAmount;
    }
}

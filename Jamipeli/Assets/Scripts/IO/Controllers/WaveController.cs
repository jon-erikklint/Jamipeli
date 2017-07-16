using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveController : MonoBehaviour {

    private WaveDefiner definer;

    private Text text;

    void Start()
    {
        this.definer = FindObjectOfType<WaveDefiner>();
        this.text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "Wave: " + definer.waveNumber;
    }
}

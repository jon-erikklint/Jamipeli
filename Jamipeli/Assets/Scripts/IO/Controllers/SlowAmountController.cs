using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowAmountController : MonoBehaviour {

    private PlayerMover player;

    private Text text;

    void Start()
    {
        this.player = FindObjectOfType<PlayerMover>();
        this.text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "Global slows: " + player.globalSlows;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour {

    private GameManager manager;

    private Text text;

	void Start () {
        this.manager = FindObjectOfType<GameManager>();
        this.text = GetComponent<Text>();
	}
	
	void Update () {
        text.text = "Points: " + manager.points;
	}
}

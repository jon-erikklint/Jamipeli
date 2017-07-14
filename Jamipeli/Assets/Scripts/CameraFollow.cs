using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform player;

	void Start () {
        this.player = FindObjectOfType<PlayerMover>().transform;
	}
	
	void Update () {
        Vector3 ppos = player.position;

        transform.position = new Vector3(ppos.x, ppos.y, transform.position.z);
	}
}

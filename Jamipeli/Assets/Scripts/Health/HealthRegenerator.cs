using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenerator : MonoBehaviour {

    public Health health;

    public float waitTime;
    public float regenerationSpeed;

    public bool kills { get { return health.kills; } }
    float lastHit;
    // Use this for initialization
    void Start () {
        if (health == null) {
            Debug.LogWarning("Object " + gameObject.name + ", : Health not specified!");
            this.enabled = false;
        }
        lastHit = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastHit > waitTime) {
            health.Heal(regenerationSpeed*Time.deltaTime);
        }
	}
}

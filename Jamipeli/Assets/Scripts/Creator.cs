using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour {
    public GameObject Instantiate(GameObject gameObject)
    {
        return Instantiate(gameObject);
    }

    public GameObject GameObjectWithRigidbody(string name = "New Gameobject")
    {
        GameObject gameObject = new GameObject(name);
        gameObject.AddComponent<Rigidbody2D>();
        return gameObject;
    }
}

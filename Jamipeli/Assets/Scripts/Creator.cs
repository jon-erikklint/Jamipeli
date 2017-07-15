using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour {
    public delegate void Action(Rigidbody2D rb);
    public event Action Event;

    public GameObject Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject obj = Object.Instantiate(gameObject, position, rotation);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null && Event != null)
            Event(rb);
        return obj;
    }

    public GameObject GameObjectWithRigidbody(float mass, string name = "New Gameobject")
    {
        GameObject gameObject = new GameObject(name);
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.gravityScale = 0;
        if(Event != null)
            Event(rb);
        return gameObject;
    }
}

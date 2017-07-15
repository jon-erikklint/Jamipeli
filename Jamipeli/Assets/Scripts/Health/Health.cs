using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour {

    public bool kills = true;
    Dieable dieableScript;

    private void Awake()
    {
        DoOnAwake();
    }
    private void Start()
    {
        dieableScript = gameObject.GetComponent<Dieable>();
        if (dieableScript == null)
        {
            Debug.LogWarning("No script extending \"Dieable\" interface found!");
            this.enabled = false;
        }
        else DoOnStart();
    }

    private void Update()
    {
        DoOnUpdate();
    }

    public bool Heal(float amount)
    {
        bool success = Healing(amount);
        if (kills && IsEmpty())
            dieableScript.Kill();
        return success;
    }

    public bool Damage(float amount)
    {
        bool success = Damaging(amount);
        if (kills && IsEmpty())
            dieableScript.Kill();
        return success;
    }

    public abstract float Amount();

    public virtual void DoOnAwake() { }
    public virtual void DoOnStart() { }
    public virtual void DoOnUpdate() { }

    public abstract bool Healing(float amount);
    public abstract bool Damaging(float amount);
    public abstract bool IsEmpty();

}

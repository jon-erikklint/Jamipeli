using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HasHealth {

    float MaxHealth();
    float Health();
    float Heal(float amount);
    float Damage(float amount);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ClassicalHealthInterface {
    float Healing(float amount, float max);
    float Damaging(float amount, float min);
    bool Heal(float amount);
    bool Damage(float amount);
    bool Healing(float amount);
    bool Damaging(float amount);
    float Amount();
    bool IsEmpty();
}

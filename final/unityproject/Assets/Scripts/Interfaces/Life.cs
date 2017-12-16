using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Life<T>
{
    float Heal (float amount);

    float TakeDamage (float amount);

    float GetTotalHealth ();

    float GetCurrentHealth ();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Life<T>
{
    int Heal (int amount);

    int TakeDamage (int amount);

    int GetTotalHealth ();

    int GetCurrentHealth ();
}

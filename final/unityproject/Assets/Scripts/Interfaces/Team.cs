using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Team
{
    bool IsRED ();

    bool IsBLUE ();

    GameManager.Teams GetTeam ();

    void SetTeam (GameManager.Teams team);
}

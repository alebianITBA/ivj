using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawn : MonoBehaviour
{
    private bool active;
    public Sprite activeSprite;
    public Sprite deactiveSprite;

    void Start ()
    {
        this.active = false;
    }

    void Update ()
    {
    }

    public void Activate ()
    {
        this.active = true;
        GetComponent<SpriteRenderer>().sprite = activeSprite;
    }

    public void Deactivate ()
    {
        this.active = false;
        GetComponent<SpriteRenderer>().sprite = deactiveSprite;
    }

    public void ReleaseMinions ()
    {
        if (active) {
            // TODO: add minion manager and release minions
        }
    }
}

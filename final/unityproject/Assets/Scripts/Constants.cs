using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // HEALTH
    public static float PLAYER_MAX_BASE_HEALTH = 100.0f;
    public static float BASE_MAX_BASE_HEALTH = 10000.0f;
    public static float TOWER_MAX_BASE_HEALTH = 5000.0f;
    public static float MINION_MAX_BASE_HEALTH = 50.0f;
    // SPEED
    public static float PLAYER_BASE_VELOCITY = 500.0f;
    public static float PLAYER_BASE_ROTATION_SPEED = 300.0f;
    public static float BULLET_SPEED = 300.0f;
    //MINION
    public static int MINION_AMOUNT = 15;
    public static int MINION_COOLDOWN = 100;
    // DISTANCE
    public static float CLOSE_DISTANCE = 0.72f;

    public static bool CloseEnough (GameObject obj1, GameObject obj2)
    {
        float distance = Vector2.Distance(obj1.transform.position, obj2.transform.position);
        return distance <= CLOSE_DISTANCE;
    }
}

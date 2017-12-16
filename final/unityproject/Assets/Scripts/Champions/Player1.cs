using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Champion
{
    void Start ()
    {
        this.health = Constants.PLAYER_MAX_BASE_HEALTH;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        if (this.camera != null) {
            this.camera.transform.position = new Vector3(transform.position.x, transform.position.y, -100);
            this.camera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        checkInput();
    }

    protected void checkInput ()
    {
        if (GetCurrentHealth() > 0) {
            if (Input.GetKey(KeyCode.W)) {
                applyImpulseForward();
            }
            if (Input.GetKey(KeyCode.S)) {
                applyImpulseBackwards();
            }
            if (Input.GetKey(KeyCode.A)) {
                float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * Constants.PLAYER_BASE_ROTATION_SPEED;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
            if (Input.GetKey(KeyCode.D)) {
                float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * Constants.PLAYER_BASE_ROTATION_SPEED;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
            if (Input.GetKey(KeyCode.Space)) {
                Shoot();
            }
        }
    }

    private void Shoot ()
    {
        
    }
}

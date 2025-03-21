using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed, deltaSpeed, jumpFource,gravity;

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    Vector3 movement;
    private void Update()
    {
        if (!LevelManager.instance.play) return;
        if (characterController.isGrounded)
        {
            movement = Vector3.forward * speed * Time.deltaTime;
            if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.UpArrow))
                movement.y = jumpFource; 
        }
        movement.y += gravity;

        characterController.Move(movement);

        speed += Time.deltaTime * deltaSpeed;

        NetworkManager.instance.SendPos(transform.position);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Point")
            LevelManager.instance.Point(col.transform);
        else if (col.gameObject.tag == "Enemy")
            LevelManager.instance.GameOver();
    }
}

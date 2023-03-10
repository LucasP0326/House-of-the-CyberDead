using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jump = 1f;
    public int health;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    bool hasGun;

    public GameObject mainCamera;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Gun" && !hasGun)
        {
            collision.transform.parent = mainCamera.transform;
            collision.transform.localPosition = new Vector3(0.7f, -0.33f, 1.03f);
            collision.transform.localRotation = Quaternion.Euler(0, -83, 0);

            collision.gameObject.GetComponent<ProjectileGun>().equipped = true;
            hasGun = true;
        }
        if (collision.gameObject.tag == "WinBox")
        {
            Win();
        }
    }

    public void Death(){
        Debug.Log("You died!");

              SceneManager.LoadScene("Lose", LoadSceneMode.Additive);
    }

    //Win State
    public void Win()
    {
        SceneManager.LoadScene("Win", LoadSceneMode.Additive);
    }
}
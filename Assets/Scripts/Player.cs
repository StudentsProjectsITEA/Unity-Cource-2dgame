using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameManager GameManager;
    public PowerUpManager PowerUpManager;
    public float JumpForce;
    public float Speed;
    public float RotationSpeed;
    public Transform GroundCheck;
    public LayerMask Ground;
    public bool isJumped;
    public float StopRotationRadius, isGroundedRadius;
    public AudioClip[] Clips;
    public AudioSource AudioSource;

    private Rigidbody2D rb;
    [SerializeField]
    private bool isGrounded, doubleJumped, stopRotation, landed;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        if (GameManager.IsGameOver)
            return;

        stopRotation = Physics2D.OverlapCircle(GroundCheck.position, StopRotationRadius, Ground);
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, isGroundedRadius, Ground);


        if (!stopRotation && isJumped)
        {
            StartCoroutine(RotatePlayer(RotationSpeed));
            isJumped = false;
            landed = false;
        }

        if (!stopRotation)
            landed = false;

        if (isGrounded && !isJumped)
        {
            doubleJumped = false;
            isJumped = false;
            this.transform.rotation = Quaternion.identity;
        }

        if (isGrounded)
        {
            if (!landed)
            {
                AudioSource.PlayOneShot(Clips[0]);
                landed = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = Vector2.up * JumpForce * PowerUpManager.PowerJumpForce;
                isJumped = true;
                AudioSource.PlayOneShot(Clips[1]);
            }
            else if (!doubleJumped)
            {
                rb.velocity = Vector2.up * JumpForce * PowerUpManager.PowerJumpForce;
                StartCoroutine(RotatePlayer(RotationSpeed));
                doubleJumped = true;
                AudioSource.PlayOneShot(Clips[1]);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !PowerUpManager.IsMagnetWorking)
        {
            //StartCoroutine(PowerUpManager.StartMagnet(PowerUpManager.MagnetDuration));
            PowerUpManager.StartMagnetCoroutine(PowerUpManager.MagnetDuration);
        }

        if (Input.GetKeyDown(KeyCode.F) && !PowerUpManager.IsPowerJumpWorking)
        {
            //StartCoroutine(PowerUpManager.PowerJumpStart(PowerUpManager.PowerJumpDuration));
            PowerUpManager.StartPowerJumpCoroutine(PowerUpManager.PowerJumpDuration);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collectable")
        {
            GameManager.CoinCount++;
            collision.gameObject.SetActive(false);
            GameManager.CoinsText.text = "Coins: "+GameManager.CoinCount;
        }
        if (collision.tag == "destroyer")
        {
            GameManager.GameOver();
        }
    }

    IEnumerator RotatePlayer(float rotationSpeed)
    {
        bool stopRotation = false;
        while (!stopRotation)
        {
            stopRotation = Physics2D.OverlapCircle(GroundCheck.position, StopRotationRadius, Ground);
            this.transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            yield return null;
        }

        while ((int)this.transform.eulerAngles.z % 90 > 3)
        {
            this.transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            yield return null;
        }

        this.transform.eulerAngles = new Vector3(0f, 0f, this.transform.eulerAngles.z - this.transform.eulerAngles.z % 90);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, StopRotationRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, isGroundedRadius);
    }
}

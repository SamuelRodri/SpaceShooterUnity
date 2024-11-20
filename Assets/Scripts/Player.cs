using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerHit();
    public event PlayerHit OnPlayerHit;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float hLimit;
    [SerializeField] private float vLimit;

    private ShootSystem shootSystem;

    private float lives = 100;

    private void Start()
    {
        shootSystem = GetComponent<ShootSystem>();
    }

    void Update()
    {
        Move();
        LimitMove();

        Shoot();
    }

    void Move()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector2(inputH, inputV).normalized * movementSpeed * Time.deltaTime);
    }

    void LimitMove()
    {
        float clampedH = Mathf.Clamp(transform.position.x, -hLimit, hLimit);
        float clampedV = Mathf.Clamp(transform.position.y, -vLimit, vLimit);

        transform.position = new Vector2(clampedH, clampedV);
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            shootSystem.Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("Enemy"))
        {
            lives -= 20;
            Destroy(collision.gameObject);

            OnPlayerHit?.Invoke();

            if (lives <= 0)
                Destroy(gameObject);
        }
    }
}
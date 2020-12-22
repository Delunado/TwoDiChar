using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotBase : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;

    protected Vector2 initialDirection;
    public Vector2 InitialDirection { get => initialDirection; set => initialDirection = value.normalized; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}

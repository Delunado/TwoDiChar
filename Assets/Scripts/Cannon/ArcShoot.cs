using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcShoot : ShotBase
{
    [SerializeField] float height;

    private void Start()
    {
        rb.AddForce(new Vector2(initialDirection.x * speed, initialDirection.y * speed), ForceMode2D.Impulse);
    }
}

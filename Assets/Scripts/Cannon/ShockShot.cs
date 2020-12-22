using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockShot : ShotBase
{
    private void Start()
    {
        Destroy(gameObject, 0.3f);
    }

    private void FixedUpdate()
    {
        rb.velocity = initialDirection * speed;
    }
}

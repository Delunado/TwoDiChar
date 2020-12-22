using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCollectable : MonoBehaviour
{
    [SerializeField] ShootType shootType;
    [SerializeField] AbilityType abilityType;

    public ShootType ShootType { get => shootType; set => shootType = value; }
    public AbilityType AbilityType { get => abilityType; set => abilityType = value; }

    private Vector3 initialPos;

    private SpriteRenderer sprRenderer;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sprRenderer.color = shootType.CannonColor;
        initialPos = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(initialPos.y, initialPos.y + 0.5f, Mathf.PingPong(Time.time, 0.5f)), transform.position.z);
    }
}

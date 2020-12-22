using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CannonType")]
public class ShootType : ScriptableObject
{
    [SerializeField] private Color cannonColor;
    public Color CannonColor { get => cannonColor; }

    [SerializeField] private GameObject shot;
    public GameObject Shot { get => shot; }

    [SerializeField] private FloatSO cannonDelay;
    public float CannonDelay { get => cannonDelay.value; }

}

using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBase : MonoBehaviour
{
    [SerializeField] ShootType shootType;
    [SerializeField] AbilityType abilityType;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private LayerMask cornerLayer;

    public enum AimingMode
    {
        AIMING_GROUND,
        AIMING_AIR
    }

    //Shooting
    private Timer shootTimer;
    private bool canShoot;

    //Ability
    protected Timer abilityTimer;
    protected bool canUseAbility;

    SpriteRenderer sprRenderer;

    private bool isActive;
    public bool IsActive { get => isActive; set => isActive = value; }

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        shootTimer = new Timer(shootType.CannonDelay, Timer.TimerType.TIMER_FIXED_UPDATE);
        abilityTimer = new Timer(abilityType.AbilityDelay + abilityType.AbilityDuration, Timer.TimerType.TIMER_FIXED_UPDATE);
        canShoot = true;
        canUseAbility = true;

        abilityType.InitAbility();

        sprRenderer.color = shootType.CannonColor;
    }

    public void Shoot(Vector2 aimingDirection)
    {
        if (canShoot)
        {
            Instantiate(shootType.Shot, shootPoint.transform.position, Quaternion.identity).GetComponent<ShotBase>().InitialDirection = aimingDirection;
            canShoot = false;
            shootTimer.Restart();
        }
    }

    public void UseAbility()
    {
        if (canUseAbility)
        {
            abilityType.UseAbility();
            canUseAbility = false;
            abilityTimer.Restart();
        }
    }

    /// <summary>
    /// Aim the cannon and returns the shooting direction
    /// </summary>
    /// <param name="aimingVector"></param>
    /// <param name="aimingMode"></param>
    /// <returns></returns>
    public Vector2 Aiming(Vector2 aimingVector, AimingMode aimingMode)
    {
        transform.localPosition = new Vector3((aimingVector.x == -1) ? -0.1f : 0.1f, transform.localPosition.y, transform.localPosition.z);
        Vector2 shootingVector = aimingVector;

        switch (aimingMode)
        {
            case AimingMode.AIMING_AIR:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 
                    (aimingVector.y * aimingVector.x * 45) + ((aimingVector.x == -1) ? 180 : 0));

                return shootingVector;

            case AimingMode.AIMING_GROUND:
                bool inCorner = CheckCorners(aimingVector.y, aimingVector.x);
                float limitedHeight = Mathf.Clamp(aimingVector.y, 0.0f, 1.0f);

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 
                    ((inCorner ? aimingVector.y : limitedHeight) * aimingVector.x * 45) + ((aimingVector.x == -1) ? 180 : 0));

                if (!inCorner)
                    shootingVector.y = Mathf.Clamp(shootingVector.y, 0.0f, 1.0f);

                return shootingVector;

            default:
                return shootingVector;
        }
    }

    private bool CheckCorners(float angle, float direction)
    {
        Vector3 checkStartPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        RaycastHit2D hit = Physics2D.Raycast(checkStartPoint, (direction * (Vector2.right)) + Vector2.down * 1.5f, 0.5f, cornerLayer);
        Debug.DrawRay(checkStartPoint, (direction * (Vector2.right)) + Vector2.down * 1.5f, Color.red);

        Debug.Log(!hit);

        return !hit;
    }

    public void Active(bool value)
    {
        sprRenderer.enabled = value;
        isActive = value;
    }

    private void FixedUpdate()
    {
        if (!shootTimer.Finished)
            shootTimer.Update();
        else
        {
            if (!canShoot)
            {
                canShoot = true;
            }
        }

        if (!abilityTimer.Finished)
            abilityTimer.Update();
        else
        {
            if (!canUseAbility)
            {
                canUseAbility = true;
            }
        }

        abilityType.UpdateAbility();
    }

    public void ChangeCannon(ShootType shootType, AbilityType abilityType)
    {
        this.shootType = shootType;
        this.abilityType = abilityType;

        shootTimer.Stop();
        abilityTimer.Stop();
        canShoot = true;
        canUseAbility = true;

        abilityType.InitAbility();

        sprRenderer.color = shootType.CannonColor;
    }
}

using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Vars&Methods
    private StateBase currentState;

    public void SetState(StateBase state)
    {
        //Para hacer build
        if (currentState != null)
            currentState.OnStateExit();

        Debug.Log("Estado: " + state);
        currentState = state; //Aqui cambiamos al nuevo estado

        if (currentState != null)
            currentState.OnStateEnter();
    }

    public void SetState(SimplePriorityQueue<StateBase, int> stateQueue)
    {
        if (stateQueue.TryDequeue(out StateBase state))
        {
            SetState(state);
        }
    }
    #endregion

    //Character Controller
    private CharacterController2D controller;
    public CharacterController2D Controller { get => controller; set => controller = value; }


    [SerializeField] private Stats stats;
    public Stats Stats { get => stats; }

    //Input
    private Rewired.Player input;
    private int rewiredPlayerId = 0;

    //Movement
    Vector3 velocity;
    float velocityXSmoothing;

    int lastInputDirection;

    //Jumping
    int totalJumps = 2;
    int jumpsLeft;

    //Dash
    private bool canUseDash;
    private Timer dashTimer;

    //Shooting
    CannonBase cannon;

    //SpriteRenderer
    SpriteRenderer sprite;

    private void Awake()
    {
        input = Rewired.ReInput.players.GetPlayer(rewiredPlayerId);

        controller = GetComponent<CharacterController2D>();
        cannon = GetComponentInChildren<CannonBase>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        stats.Init();

        lastInputDirection = 1;
        jumpsLeft = totalJumps;

        dashTimer = new Timer(stats.DashDelay, Timer.TimerType.TIMER_FIXED_UPDATE);
        canUseDash = true;

        SetState(new StateIdle(this));
    }

    private void Update()
    {
        currentState.Tick();
    }

    private void FixedUpdate()
    {
        if (!dashTimer.Finished)
            dashTimer.Update();
        else
        {
            if (!canUseDash)
            {
                canUseDash = true;
            }
        }

        currentState.FixedTick();
    }

    public void ResetVelocity()
    {
        velocity.x = 0;
        velocity.y = 0;
        controller.Move(velocity);
    }

    public void UpdateLastInputDirection()
    {
        if (input.GetAxis(RewiredConsts.Action.HorizontalMovement) > 0)
            lastInputDirection = 1;
        else if (input.GetAxis(RewiredConsts.Action.HorizontalMovement) < 0)
            lastInputDirection = -1;

        GetComponent<SpriteRenderer>().flipX = (lastInputDirection == 1) ? false : true;
    }

    #region Movement Methods

    /// <summary>
    /// Returns true if there is horizontal input, false otherwise.
    /// </summary>
    /// <returns></returns>
    public bool CheckHorizontalInput()
    {
        return !Mathf.Approximately(input.GetAxis(RewiredConsts.Action.HorizontalMovement), 0.0f);
    }

    public void MovementGround()
    {
        Vector2 inputMovement = new Vector2(input.GetAxis(RewiredConsts.Action.HorizontalMovement), 0);

        if (velocity.y > 20.0f)
        {
            velocity.y = 20.0f;
        }

        if (controller.collisions.below || controller.collisions.above)
        {
            velocity.y = 0;
        }

        Jump();

        float targetVelocityX = inputMovement.x * stats.HorizontalSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? stats.AccelerationTimeGround : stats.AccelerationTimeAir);

        velocity.y += stats.Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (jumpsLeft > 1)
        {
            if (input.GetButtonDown(RewiredConsts.Action.Jump))
            {
                if (jumpsLeft == totalJumps)
                    velocity.y = stats.MaxJumpVelocity;
                else
                    velocity.y = stats.MaxJumpVelocity * 0.8f;

                jumpsLeft--;
            }

            if (input.GetButtonUp(RewiredConsts.Action.Jump))
            {
                if (velocity.y > stats.MinJumpVelocity)
                    velocity.y = stats.MinJumpVelocity;
            }
        }

        if (jumpsLeft <= totalJumps - 1)
        {
            if (controller.collisions.below)
            {
                jumpsLeft = totalJumps;
            }
        }
    }

    #endregion

    #region Wall Slide & Jump
    public bool CheckWallSlide()
    {
        return ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below);
    }

    public void WallSlideMovement()
    {
        //Reset the jumps
        if (jumpsLeft != totalJumps)
            jumpsLeft = totalJumps;

        Vector2 inputMovement = new Vector2(input.GetAxis(RewiredConsts.Action.HorizontalMovement), 0);
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        //This limit the y velocity to the maximum sliding speed.
        if (velocity.y < -stats.WallSlideSpeedMax)
            velocity.y = -stats.WallSlideSpeedMax;

        if (input.GetButtonDown(RewiredConsts.Action.Jump))
        {
            if (wallDirX == inputMovement.x)
            {
                velocity.x = -wallDirX * stats.WallJumpClimb.x;
                velocity.y = stats.WallJumpClimb.y;
            }
            else if (Mathf.Sign(inputMovement.x) != wallDirX)
            {
                velocity.x = -wallDirX * stats.WallLeap.x;
                velocity.y = stats.WallLeap.y;
            }
            else
            {
                velocity.x = -wallDirX * stats.WallJumpOff.x;
                velocity.y = stats.WallJumpOff.y;
            }
        }

        if (input.GetButtonUp(RewiredConsts.Action.Jump))
        {
            if (velocity.y > stats.MinJumpVelocity)
                velocity.y = stats.MinJumpVelocity;
        }

        velocity.y += stats.Gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region Dash
    public bool CheckDash()
    {
        return (input.GetButtonDown(RewiredConsts.Action.Dash) && canUseDash);
    }

    public void DashImpulse()
    {
        velocity.x = stats.DashSpeed * lastInputDirection * Time.deltaTime;
        velocity.y = 0;

        controller.Move(velocity);
    }

    public void DashUsed()
    {
        dashTimer.Restart();
        canUseDash = false;
    }

    #endregion

    #region Shooting
    public void Aiming(CannonBase.AimingMode aimingMode)
    {
        if (!Mathf.Approximately(input.GetAxis(RewiredConsts.Action.AimingX), 0.0f) || !Mathf.Approximately(input.GetAxis(RewiredConsts.Action.AimingY), 0.0f))
        {
            if (!cannon.IsActive)
                cannon.Active(true);

            //Here we calculate the aiming vector
            float aimingDirection = Mathf.Sign(input.GetAxis(RewiredConsts.Action.AimingX));
            float aimingHeight = input.GetAxis(RewiredConsts.Action.AimingY);

            Vector2 aimingVector = new Vector2(aimingDirection, aimingHeight);

            Shoot(cannon.Aiming(aimingVector, aimingMode));

            sprite.flipX = aimingDirection != 1;
        }
        else
        {
            if (cannon.IsActive)
                cannon.Active(false);
        }
    }

    /// <summary>
    /// Checks if you have shooted, and in that case, shoot in the passed direction.
    /// </summary>
    /// <param name="aimingVector"></param>
    private void Shoot(Vector2 aimingVector)
    {
        if (input.GetButtonDown(RewiredConsts.Action.Shoot))
        {
            cannon.Shoot(aimingVector);
        }
    }

    public void SpecialAbility()
    {
        if (input.GetButton(RewiredConsts.Action.SpecialAbility))
        {
            cannon.UseAbility();
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CannonCollectable cannonCollectable = collision.GetComponent<CannonCollectable>();

        if (cannonCollectable)
        {
            cannon.ChangeCannon(cannonCollectable.ShootType, cannonCollectable.AbilityType);
            Destroy(cannonCollectable.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject
{
    //General
    private float gravity; public float Gravity { get => gravity; }

    [Header("Movement")]
    [SerializeField] private FloatSO horizontalSpeed; public float HorizontalSpeed { get => horizontalSpeed.value; }

    [SerializeField] private FloatSO accelerationTimeGround; //0.075f;
    public float AccelerationTimeGround { get => accelerationTimeGround.value; }

    [SerializeField] private FloatSO accelerationTimeAir; //0.15f;
    public float AccelerationTimeAir { get => accelerationTimeAir.value; }


    [Header("Jump")]
    private float maxJumpVelocity;
    public float MaxJumpVelocity { get => maxJumpVelocity; }

    private float minJumpVelocity;
    public float MinJumpVelocity { get => minJumpVelocity; }

    [SerializeField] private FloatSO maxJumpHeight;//3.3f;   
    public float MaxJumpHeight { get => maxJumpHeight.value; }
    
    [SerializeField] private FloatSO minJumpHeight;//0.5f;
    public float MinJumpHeight { get => minJumpHeight.value; }

    [SerializeField] private FloatSO timeToJumpApex;//0.585f;
    public float TimeToJumpApex { get => timeToJumpApex.value; }


    [Header("Wall Slide & Wall Jump")]
    [SerializeField] private FloatSO wallSlideSpeedMax;//2.8f;
    public float WallSlideSpeedMax { get => wallSlideSpeedMax.value; }

    [SerializeField] private Vector2SO wallJumpClimb;
    public Vector2 WallJumpClimb { get => wallJumpClimb.vector; }
    [SerializeField] private Vector2SO wallJumpOff;
    public Vector2 WallJumpOff { get => wallJumpOff.vector; }
    [SerializeField] private Vector2SO wallLeap;
    public Vector2 WallLeap { get => wallLeap.vector; }


    [Header("Dash")]
    [SerializeField] private IntSO dashCount;
    public int DashCount { get => dashCount.value; }

    [SerializeField] private FloatSO dashSpeed;
    public float DashSpeed { get => dashSpeed.value; }

    [SerializeField] private FloatSO dashDelay;
    public float DashDelay { get => dashDelay.value; }


    /// <summary>
    /// Initialize the gravity, jump velocity and min jump velocity.
    /// </summary>
    public void Init()
    {
        gravity = -(2 * maxJumpHeight.value) / Mathf.Pow(timeToJumpApex.value, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex.value;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight.value);
    }
}

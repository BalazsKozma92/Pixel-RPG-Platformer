using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base data")]
public class PlayerData : ScriptableObject
{
    [Header("Move state")]
    public float moveSpeed = 8f;

    [Header("Crouch state")]
    public float crouchMoveSpeed = 4f;
    public float crouchColliderHeight = .8f;
    public float standColliderHeight = 1.55f;

    [Header("Jump state")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("Air state")]
    public float coyoteTime = .2f;
    public float variableJumpHeightMultiplier = .5f;

    [Header("Wallslide state")]
    public float wallSlideVelocity = 1.5f; 

    [Header("Wallclimb state")]
    public float wallClimbVelocity = 2.5f;

    [Header("Walljump state")]
    public float wallJumpVelocity = 14f;
    public float wallJumpTime = .4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Ledge climb state")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Dash state")]
    public float dashCooldown = .5f;
    public float maxHoldTime = 4f;
    public float holdTimeScale = .25f;
    public float dashTime = .2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = .25f;
    public float distanceBetweenAfterImages = .5f;

    [Header("Check variables")]
    public float groundCheckRadius = .2f;
    public LayerMask groundMask;
    public float wallCheckDistance = .5f;
}

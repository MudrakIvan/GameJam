using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

/// <summary>
/// Simple 2D character movement processor.
/// </summary>
public class Player : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 4.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float SprintSpeed = 6.0f;
	[Tooltip("Rotation speed of the character")]
	public float RotationSpeed = 1.0f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;
	[Tooltip("Animation divider for the movement speed")]
	public float MoveSpeedAnimation = 6.0f;

	[Space(10)]
	[Tooltip("The maximum jump speed")]
	public float JumpSpeed = 1.2f;
	[Tooltip("Ramping of the jump speed")]
	public float JumpChangeRate = 0.1f;
	[Tooltip("Maximum time a jump can be held")]
	public float JumpDuration = 0.5f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = 15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.1f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;

	public float AttackDuration = 0.5f;

	public float MaxHealth = 5.0f;

	[HideInInspector]
	public bool IsAttacking => mAttackDurationDelta > 0.0f;

	private float Health;

	private float mTargetHorSpeed;
	private float mHorizontalSpeed;
	private float mTargetVerSpeed;
	private float mVerticalSpeed;
	private float mAnimationBlend;
	private float mTerminalVelocity = -53.0f;

	private float mJumpTimeoutDelta;
	private float mJumpDurationDelta;
	private float mFallTimeoutDelta;

	private float mAttackDurationDelta;

	private bool mHeadingRight;

	private bool hurt;
	private bool mHurt
	{
		get 
		{
			bool temp = hurt;
			hurt = false;
			return temp;
		}
		set
		{
			hurt = value;
		}
	}
	
	private Character2DController mController;
	private InputManager mInput;
	private Animator mAnimator;
	private BoxCollider2D mAttackCollider;
	
	public void AddHealth(float health)
	{
		Health = Health + health >= MaxHealth ? MaxHealth : Health + health;
		Debug.Log(Health);
	}

	public void RemoveHealth(float health)
	{
		Health -= health;
		mHurt = true;
        Debug.Log(Health);

        if (Health <= 0.0f){
			Destroy(gameObject);
			GameOverMenu.Instance.GameOverShow();
		}
	}

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        mController = GetComponent<Character2DController>();
        mInput = GetComponent<InputManager>();
		mAnimator = GetComponentInChildren<Animator>();
		mAttackCollider = GetComponentsInChildren<BoxCollider2D>().Skip(1).First();

        mTargetHorSpeed = 0.0f;
        mTargetVerSpeed = 0.0f;

        mJumpTimeoutDelta = JumpTimeout;
        mJumpDurationDelta = 0.0f;
        mFallTimeoutDelta = FallTimeout;
		mAttackDurationDelta = 0.0f;
		Health = MaxHealth;

        mHeadingRight = true;
		mHurt = false;
    }

    /// <summary>
    /// Update called once per frame.
    /// </summary>
    void Update()
    {
	    mTargetHorSpeed = mInput.sprint ? SprintSpeed : MoveSpeed;
	    if (mInput.move == Vector2.zero)
	    { mTargetHorSpeed = 0.0f; }
    }
    
    /// <summary>
    /// Update called at fixed intervals.
    /// </summary>
    void FixedUpdate ()
    {
		CharacterAttack();
	    MoveHorizontal();
	    JumpAndGravity();
	    AnimateCharacter();
	    
		var movement = new Vector3(
			mHorizontalSpeed * Math.Sign(mInput.move.x), 
			mVerticalSpeed, 
			0.0f
		);
		
	    mController.Move(movement * Time.fixedDeltaTime);
    }

	private void CharacterAttack()
	{
		if (mInput.attack){
			mAttackDurationDelta = AttackDuration;
			mAttackCollider.enabled = true;
			return;
		}

		mAttackDurationDelta = (mAttackDurationDelta - Time.fixedDeltaTime) > 0.0f ? mAttackDurationDelta - Time.fixedDeltaTime : 0.0f;
		mAttackCollider.enabled = IsAttacking;
	}

    /// <summary>
    /// Perform horizontal movement calculations.
    /// </summary>
    void MoveHorizontal()
    {
		var currentHorizontalSpeed = new Vector3(mController.velocity.x, 0.0f, mController.velocity.z).magnitude;

		var speedOffset = 0.1f;
		var inputMagnitude = mInput.analogMovement ? Math.Abs(mInput.move.x) : 1.0f;

		if (currentHorizontalSpeed < mTargetHorSpeed - speedOffset || 
		    currentHorizontalSpeed > mTargetHorSpeed + speedOffset)
		{
			mHorizontalSpeed = Mathf.Lerp(
				currentHorizontalSpeed, 
				mTargetHorSpeed * inputMagnitude, 
				Time.fixedDeltaTime * SpeedChangeRate
			);
			mHorizontalSpeed = Mathf.Round(mHorizontalSpeed * 1000f) / 1000f;
		}
		else
		{ mHorizontalSpeed = mTargetHorSpeed; }
    }
    
    /// <summary>
    /// Perform vertical movement calculations.
    /// </summary>
	private void JumpAndGravity()
	{
		if (mController.isGrounded)
		{
			mFallTimeoutDelta = FallTimeout;

			if (mInput.jump && mJumpTimeoutDelta <= 0.0f)
			{
				mTargetVerSpeed = Mathf.Sqrt(JumpSpeed * 2.0f * Gravity);
				mJumpTimeoutDelta = JumpTimeout;
				mJumpDurationDelta = JumpDuration; 
			}
			else
			{ mTargetVerSpeed = mVerticalSpeed; }
			
			if (mJumpTimeoutDelta >= 0.0f)
			{ mJumpTimeoutDelta -= Time.fixedDeltaTime; }
		}
		else
		{
			mTargetVerSpeed = mInput.jump && mJumpDurationDelta >= 0.0f
				? Mathf.Sqrt(JumpSpeed * 2.0f * Gravity)
				: mVerticalSpeed;
			
			if (mJumpDurationDelta >= 0.0f)
			{ mJumpDurationDelta -= Time.fixedDeltaTime; }
			
			if (mFallTimeoutDelta >= 0.0f)
			{ mFallTimeoutDelta -= Time.fixedDeltaTime; }
		}
		
		var currentVerticalSpeed = mController.velocity.y;
		
		var speedOffset = 0.1f;
		var inputMagnitude = 1.0f;

		if (currentVerticalSpeed < mTargetVerSpeed - speedOffset || 
			currentVerticalSpeed > mTargetVerSpeed + speedOffset)
		{
			mVerticalSpeed = Mathf.Lerp(
				currentVerticalSpeed, 
				mTargetVerSpeed * inputMagnitude, 
				Time.fixedDeltaTime * JumpChangeRate
			);
			mVerticalSpeed = Mathf.Round(mVerticalSpeed * 1000f) / 1000f;
		}
		else
		{ mVerticalSpeed = mTargetVerSpeed; }
		
		if (mVerticalSpeed > mTerminalVelocity)
		{ mVerticalSpeed -= Gravity * Time.fixedDeltaTime; }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.name.Contains("Flame"))
		{
			Destroy(collision.gameObject);
			gameObject.GetComponentInChildren<CandleScript>().AddIntensity(1);
		}
		else if (collision.name.Contains("Life"))
		{
			AddHealth(1.0f);
			Destroy(collision.gameObject);
		}
    }

    /// <summary>
    /// Run animation according to the current state.
    /// </summary>
    void AnimateCharacter()
    {
		var headingRight = (mHeadingRight) ? mInput.move.x >= 0.0f : mInput.move.x > 0.0f;

		// Change of direction
		if (mHeadingRight != headingRight){
			//transform.localRotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);
			transform.localScale = Vector3.Scale(new Vector3(-1.0f, 1.0f, 1.0f), transform.localScale);
		}

		mHeadingRight = headingRight;
		
	    var animator = mAnimator;
	    if (animator != null)
	    {
			var currentVerticalSpeed = mController.velocity.y;
			var currentHorizontalSpeed = new Vector3(mController.velocity.x, 0.0f, mController.velocity.z).magnitude;
			var moveSpeed = Math.Abs(mTargetHorSpeed / MoveSpeedAnimation);
			var falling = !mController.isGrounded && mFallTimeoutDelta <= 0.0f;

			animator.SetFloat("Speed", currentHorizontalSpeed);
			animator.SetFloat("MoveSpeed", moveSpeed);
			animator.SetBool("Jump", mInput.jump);
			animator.SetBool("Grounded", mController.isGrounded);
			animator.SetBool("Fall", falling);
			animator.SetBool("Crouch", mInput.crouch);
			animator.SetBool("Attack", mInput.attack);
			animator.SetBool("Hurt", mHurt);
	    }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    /// <summary>
    /// Time before the enemy will turn around
    /// </summary>
    public float OneDiractionWalkTime = 0.5f;

    [Tooltip("Move speed of the character in m/s")]
	public float Speed = 4.0f;

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

	private float mTargetHorSpeed;
	private float mHorizontalSpeed;
	private float mTargetVerSpeed;
	private float mVerticalSpeed;
	private float mAnimationBlend;
	private float mTerminalVelocity = -53.0f;
    private float mJumpTimeoutDelta;
	private float mJumpDurationDelta;
	private float mFallTimeoutDelta;

    private bool HeadingLeft = true;

    private float TimeOut = 0.0f;

    private BoxCollider2D mCharacterCollider;

    private BoxCollider2D mGroundCollider;

    private Character2DController mController;

    private Animator mAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        mController = GetComponent<Character2DController>();
		mAnimator = GetComponentInChildren<Animator>();

        mTargetHorSpeed = Speed;
        mTargetVerSpeed = 0.0f;

        mJumpTimeoutDelta = JumpTimeout;
        mJumpDurationDelta = 0.0f;
        mFallTimeoutDelta = FallTimeout;


        TimeOut = OneDiractionWalkTime / 2;
        RotateEnemy();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        TimeOut += Time.deltaTime;
        if (TimeOut >= OneDiractionWalkTime){
            TimeOut = 0.0f;
            HeadingLeft = !HeadingLeft;
			RotateEnemy();
        }

        MoveHorizontal();
	    JumpAndGravity();
	    // AnimateCharacter();
	    
		var movement = new Vector3(
			mHorizontalSpeed * ((HeadingLeft) ? -1 : 1),
			mVerticalSpeed,
			0.0f
		);
		
	    mController.Move(movement * Time.fixedDeltaTime);
    }

    private void RotateEnemy()
    {
        transform.localRotation *= Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collidedObject)
    {
        if (collidedObject.name != "Player" && collidedObject.name != "Model")
        {
            return;
        }

        var player = collidedObject.gameObject.GetComponentInParent<Character2DMovement>();

        if (player.IsAttacking && collidedObject.name == "Model")
        {
            Destroy(gameObject);
            return;
        }

        if (collidedObject.name == "Player")
        {
            Destroy(collidedObject.gameObject);
        }
    }

    /// <summary>
    /// Perform horizontal movement calculations.
    /// </summary>
    void MoveHorizontal()
    {
		var currentHorizontalSpeed = new Vector3(mController.velocity.x, 0.0f, mController.velocity.z).magnitude;

		var speedOffset = 0.1f;
		var inputMagnitude = 1.0f; // Enemy will move with constant magnitude

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

			// if (mInput.jump && mJumpTimeoutDelta <= 0.0f) // Enemy will not be able to jump
			// {
			// 	mTargetVerSpeed = Mathf.Sqrt(JumpSpeed * 2.0f * Gravity);
			// 	mJumpTimeoutDelta = JumpTimeout;
			// 	mJumpDurationDelta = JumpDuration; 
			// }
			// else
			// {  }

            mTargetVerSpeed = mVerticalSpeed;
			
			if (mJumpTimeoutDelta >= 0.0f)
			{ mJumpTimeoutDelta -= Time.fixedDeltaTime; }
		}
		else
		{
            // mInput.jump && mJumpDurationDelta >= 0.0f // Enemy will not be able to jump
			//	? Mathf.Sqrt(JumpSpeed * 2.0f * Gravity)
			//	:
			mTargetVerSpeed = mVerticalSpeed;
			
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

    /// <summary>
    /// Run animation according to the current state.
    /// </summary>
    void AnimateCharacter()
    {	
	    var animator = mAnimator;
	    if (animator != null)
	    {
			var currentVerticalSpeed = mController.velocity.y;
			var currentHorizontalSpeed = new Vector3(mController.velocity.x, 0.0f, mController.velocity.z).magnitude;
			var moveSpeed = Math.Abs(mTargetHorSpeed / MoveSpeedAnimation);
			var falling = !mController.isGrounded && mFallTimeoutDelta <= 0.0f;

            // Enemy animation do not have ani variables
			// animator.SetFloat("Speed", currentHorizontalSpeed);
			// animator.SetFloat("MoveSpeed", moveSpeed);
			// animator.SetBool("Grounded", mController.isGrounded);
			// animator.SetBool("Fall", falling);
	    }
    }
}

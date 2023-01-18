using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    /// <summary>
    /// Time before the enemy will turn around
    /// </summary>
    public float OneDiractionWalkTime = 0.5f;

    public float DyingTime = 0.25f;

    public float MaxAgroDistance = 5.0f;

    public float MaxHealth = 2.0f;

    public float ActionTimeout = 1.0f;

	public GameObject[] generatedAfterDie;

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

    private bool shouldJump;
    private float mAttackTimetOut;

    private float mIncommingDamageTimeOut;

    private bool mDying = false;

    private bool mHeadingLeft = true;

    private float TimeOut = 0.0f;

    private float mHealth;

    private BoxCollider2D mCharacterCollider;

    private BoxCollider2D mGroundCollider;

    private Character2DController mController;

    private Animator mAnimator;

    private Player mPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        mController = GetComponent<Character2DController>();
		mAnimator = GetComponentInChildren<Animator>();

        mTargetHorSpeed = Speed;
        mTargetVerSpeed = 0.0f;
        mAttackTimetOut = 0.0f;
        mIncommingDamageTimeOut = 0.0f;

        mJumpTimeoutDelta = JumpTimeout;
        mJumpDurationDelta = 0.0f;
        mFallTimeoutDelta = FallTimeout;
        mHealth = MaxHealth;
        mPlayer = GameObject.Find("Player").GetComponent<Player>();

        TimeOut = OneDiractionWalkTime / 2;
        RotateEnemy();
    }

    private void FixedUpdate()
    {
        AnimateCharacter();
        if (!mDying)
        {
            TimeOutsDecrease();
            JumpAndGravity();
            Move();
        }
    }
    private float GetPlayerDistance()
    {
        if (mPlayer == null)
            return MaxAgroDistance + 1.0f;
        return (transform.position - mPlayer.transform.position).magnitude;
    }

    private void RotateEnemy()
    {
        transform.localScale = Vector3.Scale(new Vector3(-1.0f, 1.0f, 1.0f), transform.localScale);
    }
	
	private void GenerateItems()
	{
		for (int i = 0; i < generatedAfterDie.Length; i++)
		{
            var clone = Instantiate(generatedAfterDie[i]);
            clone.transform.position = transform.position + new Vector3(0.1f * i, -0.5f, 0);
        }
	}

    private void TimeOutsDecrease()
    {
        mIncommingDamageTimeOut = mIncommingDamageTimeOut - Time.fixedDeltaTime <= 0.0f ? 0.0f : mIncommingDamageTimeOut - Time.fixedDeltaTime;
        mAttackTimetOut = mAttackTimetOut - Time.fixedDeltaTime <= 0.0f ? 0.0f : mAttackTimetOut - Time.fixedDeltaTime;
    }

    private void OnTriggerStay2D(Collider2D collidedObject)
    {
        OnTrigger(collidedObject);
    }

    private void OnTrigger(Collider2D collidedObject)
    {
        if ((collidedObject.name != "Player" && collidedObject.name != "Model") || mDying)
            return;

        var player = collidedObject.gameObject.GetComponentInParent<Player>();

        if (player == null)
            return;

        if (player.IsAttacking && collidedObject.name == "Model")
        {
            if (mIncommingDamageTimeOut <= 0.0f)
            {
                RemoveHealth(1.0f);
                mIncommingDamageTimeOut = ActionTimeout;
                shouldJump = true;
            }
            return;
        }

        if (collidedObject.name == "Player")
        {
            if (mAttackTimetOut <= 0.0f)
            {
                player.RemoveHealth(1.0f);
                mAttackTimetOut = ActionTimeout;
            }
        }
    }

    private void RemoveHealth(float health)
    {
        mHealth -= health;
        if (mHealth <= 0.0f){
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Invoke(nameof(GenerateItems), 0.5f * DyingTime);
        Destroy(gameObject, DyingTime);			
        mDying = true;
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
    
    private void Move()
    {
        if (GetPlayerDistance() > MaxAgroDistance)
            CheckMoveRotation();
        else
            RotateToPlayer();

        MoveHorizontal();
        
        var movement = new Vector3(
            mHorizontalSpeed * ((mHeadingLeft) ? -1 : 1),
            mVerticalSpeed,
            0.0f
        );
        
        mController.Move(movement * Time.fixedDeltaTime);
    }

    private void CheckMoveRotation()
    {
        TimeOut += Time.deltaTime;
        if (TimeOut >= OneDiractionWalkTime){
            TimeOut = 0.0f;
            mHeadingLeft = !mHeadingLeft;
			RotateEnemy();
        }
    }

    private void RotateToPlayer()
    {
        bool headLeft = transform.position.x > (mPlayer == null ? 0.0f : mPlayer.transform.position.x);
        headLeft = shouldJump ? !headLeft : headLeft;
        
        if (headLeft != mHeadingLeft)
            RotateEnemy();
        
        mHeadingLeft = headLeft;
    }

    /// <summary>
    /// Perform vertical movement calculations.
    /// </summary>
	private void JumpAndGravity()
	{
		if (mController.isGrounded)
		{
			mFallTimeoutDelta = FallTimeout;

			if (shouldJump && mJumpTimeoutDelta <= 0.0f) 
			{
			    mTargetVerSpeed = Mathf.Sqrt(JumpSpeed * 2.0f * Gravity);
			    mJumpTimeoutDelta = JumpTimeout;
			    mJumpDurationDelta = JumpDuration;                
            }
			else
			{ 
                mTargetVerSpeed = mVerticalSpeed;
                
            }

			
			if (mJumpTimeoutDelta >= 0.0f)
			{ mJumpTimeoutDelta -= Time.fixedDeltaTime; }
		}
        else
        {
            mTargetVerSpeed = shouldJump && mJumpDurationDelta >= 0.0f 
                ? Mathf.Sqrt(JumpSpeed * 2.0f * Gravity)
                : mVerticalSpeed;
			
			if (mJumpDurationDelta >= 0.0f)
			{ mJumpDurationDelta -= Time.fixedDeltaTime; }
            else
            {
                shouldJump = false;
            }
			
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
            animator.SetBool("Dying", mDying);
	    }
    }
}

using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


/// <summary>
/// Simple input manager for storing current inputs..
/// </summary>
public class InputManager : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	public bool crouch;
	public bool attack = false;

	[Header("Movement Settings")]
	public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM
	public void OnMove(InputValue value)
	{ MoveInput(value.Get<Vector2>()); }

	public void OnLook(InputValue value)
	{
		if(cursorInputForLook)
		{ LookInput(value.Get<Vector2>()); }
	}

	public void OnJump(InputValue value)
	{ JumpInput(value.isPressed); }

	public void OnSprint(InputValue value)
	{ SprintInput(value.isPressed); }
	
	public void OnCrouch(InputValue value)
	{ CrouchInput(value.isPressed); }

	public void OnLeftClick(InputValue value)
	{ LeftClickInput(value.isPressed); }

#else
// old input sys if we do decide to have it (most likely wont)...
#endif


	public void MoveInput(Vector2 newMoveDirection)
	{ move = newMoveDirection; } 

	public void LookInput(Vector2 newLookDirection)
	{ look = newLookDirection; }

	public void JumpInput(bool newJumpState)
	{ jump = newJumpState; }

	public void SprintInput(bool newSprintState)
	{ sprint = newSprintState; }
	
	public void CrouchInput(bool newCrouchState)
	{ crouch = newCrouchState; }
	
	public void LeftClickInput(bool newLeftClickState)
	{ attack = newLeftClickState; }

#if !UNITY_IOS || !UNITY_ANDROID

	private void OnApplicationFocus(bool hasFocus)
	{ SetCursorState(cursorLocked); }

	private void SetCursorState(bool newState)
	{ Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None; }

#endif

}

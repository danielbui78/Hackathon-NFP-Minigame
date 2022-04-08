using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class TapToRunController : MonoBehaviour
{
	public float fCurrentSpeed = 0.0f;
	public const float fMaxSpeedIncreasePerTap = 10f;
	public float fSpeedIncreasePerTap = 0.0f;
	public int speedCycleDirection = 1;
//	public float fDragForce = -0.01f;
	public float fDragCoefficient = 0.97f;
	public float fDragCoefficient_AtEndGame = 0.97f;
	public bool bIsPressed = false;
	public const float fTapRefractoryConstant = 0.350f;
	public float fTapRefractoryTimer = 0.0f;
	public bool bReadyToTap = true;
	public bool bDisableMove = false;

/*
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 2.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float SprintSpeed = 5.335f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;
*/

	[Space(10)]
	[Tooltip("The height the player can jump")]
	public float JumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.50f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	public bool Grounded = true;
	[Tooltip("Useful for rough ground")]
	public float GroundedOffset = -0.14f;
	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	public float GroundedRadius = 0.28f;
	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 70.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -30.0f;
	[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
	public float CameraAngleOverride = 0.0f;
	[Tooltip("For locking the camera position on all axis")]
	public bool LockCameraPosition = false;

	// cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;

	// player
	public float _speed;
	public float _animationBlend;
	public float _targetRotation = 0.0f;
	public float _rotationVelocity;
	public float _verticalVelocity;
	public float _terminalVelocity = 53.0f;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	// animation IDs
	public int _animIDSpeed;
	public int _animIDGrounded;
	public int _animIDJump;
	public int _animIDFreeFall;
	public int _animIDMotionSpeed;

	public PlayerInput _playerInput;
	public Animator _animator;
	public CharacterController _controller;
	public StarterAssetsInputs _input;
	public GameObject _mainCamera;

	private const float _threshold = 0.01f;

	public bool _hasAnimator;

	private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

	private void Awake()
	{
		// get a reference to our main camera
		if (_mainCamera == null)
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}

	private void Start()
	{
		_hasAnimator = TryGetComponent(out _animator);
		_controller = GetComponent<CharacterController>();
		_input = GetComponent<StarterAssetsInputs>();
		_playerInput = GetComponent<PlayerInput>();

		AssignAnimationIDs();

		// reset our timeouts on start
		_jumpTimeoutDelta = JumpTimeout;
		_fallTimeoutDelta = FallTimeout;
	}

	private void Update()
	{
		_hasAnimator = TryGetComponent(out _animator);

		if (fSpeedIncreasePerTap > fMaxSpeedIncreasePerTap)
        {
			speedCycleDirection = -1;
			fSpeedIncreasePerTap = fMaxSpeedIncreasePerTap;
		}
		else if (fSpeedIncreasePerTap < 0)
        {
			speedCycleDirection = 1;
			fSpeedIncreasePerTap = 0;
		}
		fSpeedIncreasePerTap += speedCycleDirection * fMaxSpeedIncreasePerTap * (Time.deltaTime / fTapRefractoryConstant);

		JumpAndGravity();
		GroundedCheck();
		Move();

	}

	private void FixedUpdate()
	{
		if (fCurrentSpeed > 0)
        {
//			fCurrentSpeed += fDragForce;
//			fCurrentSpeed *= fDragCoefficient;
			fCurrentSpeed *= 0.985f;
		}
	}

	private void LateUpdate()
	{
		CameraRotation();
	}

	private void AssignAnimationIDs()
	{
		_animIDSpeed = Animator.StringToHash("Speed");
		_animIDGrounded = Animator.StringToHash("Grounded");
		_animIDJump = Animator.StringToHash("Jump");
		_animIDFreeFall = Animator.StringToHash("FreeFall");
		_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
	}

	private void GroundedCheck()
	{
		// set sphere position, with offset
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

		// update animator if using character
		if (_hasAnimator)
		{
			_animator.SetBool(_animIDGrounded, Grounded);
		}
	}

	private void CameraRotation()
	{
		// if there is an input and camera position is not fixed
		if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
		{
			//Don't multiply mouse input by Time.deltaTime;
			float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

			_cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
			_cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
		}

		// clamp our rotations so our values are limited 360 degrees
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		// Cinemachine will follow this target
		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
	}

	void Move()
	{
		if (bDisableMove)
			return;

		// Detect Tapping
		bool bWasTapped = false;

		if (fTapRefractoryTimer > 0.0f)
        {
			fTapRefractoryTimer -= Time.deltaTime;
			bReadyToTap = false;
        }
		else
        {
			bReadyToTap = true;
        }
/*
		if (_input.move.y == 0.0f)
        {
			bIsPressed = false;
        }
		else if (_input.move.y > 0.0f)
        {
			if (bIsPressed == false && bReadyToTap == true)
            {
				bWasTapped = true;
				fTapRefractoryTimer = fTapRefractoryConstant;
            }
			bIsPressed = true;
        }
*/

		// NOTE: Activate Button must be set to "Value" type in the Unity Input System Package in order to be correctly reset to false
		bIsPressed = _input.activateButton;
		if (bIsPressed)
        {
			speedCycleDirection = -1;
        }

		if (bIsPressed && bReadyToTap)
        {
			bWasTapped = true;
			fTapRefractoryTimer = fTapRefractoryConstant;
		}

		// set target speed based on move speed, sprint speed and if sprint is pressed
		// gradually increase run speed be cumulatively adding increases from taps of _input.sprint
		if (bWasTapped)
		{
/*
			if (fCurrentSpeed <= 0.0f)
            {
				fCurrentSpeed += fSpeedIncreasePerTap * 3;
			}
			else
            {
				fCurrentSpeed += fSpeedIncreasePerTap;
			}
*/
			fCurrentSpeed += fSpeedIncreasePerTap;

		}

		float targetSpeed = fCurrentSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
//		if (_input.move == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
//		float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
		float currentHorizontalSpeed = 1.0f;

		float speedOffset = 0.1f;
		float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

		//// accelerate or decelerate to target speed
		//if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		//{
		//	// creates curved result rather than a linear one giving a more organic speed change
		//	// note T in Lerp is clamped, so we don't need to clamp our speed
		//	_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

		//	// round speed to 3 decimal places
		//	_speed = Mathf.Round(_speed * 1000f) / 1000f;
		//}
		//else
		//{
		//	_speed = targetSpeed;
		//}
//		_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
		_animationBlend = targetSpeed;

		// For Racing Game, always use the forward vector
		Vector3 inputDirection = Vector3.forward;

        // normalise input direction
        //Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        //// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        //// if there is a move input rotate player when the player is moving
        //if (_input.move != Vector2.zero)
        //{
        //    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        //    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

        //    // rotate to face input direction relative to camera position
        //    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        //}

        Vector3 targetDirection = Vector3.forward;

		// move the player
		Vector3 MoveParameter = targetDirection.normalized * (fCurrentSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
		_controller.Move(MoveParameter);

		// update animator if using character
		if (_hasAnimator)
		{
			_animator.SetFloat(_animIDSpeed, _animationBlend);
			_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
		}

	}

	private void JumpAndGravity()
	{
		if (Grounded)
		{
			// reset the fall timeout timer
			_fallTimeoutDelta = FallTimeout;

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDJump, false);
				_animator.SetBool(_animIDFreeFall, false);
			}

			// stop our velocity dropping infinitely when grounded
			if (_verticalVelocity < 0.0f)
			{
				_verticalVelocity = -2f;
			}

			// Jump
			if (_input.jump && _jumpTimeoutDelta <= 0.0f)
			{
				// the square root of H * -2 * G = how much velocity needed to reach desired height
				_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, true);
				}
			}

			// jump timeout
			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{
			// reset the jump timeout timer
			_jumpTimeoutDelta = JumpTimeout;

			// fall timeout
			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}
			else
			{
				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDFreeFall, true);
				}
			}

			// if we are not grounded, do not jump
			_input.jump = false;
		}

		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (_verticalVelocity < _terminalVelocity)
		{
			_verticalVelocity += Gravity * Time.deltaTime;
		}
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

	private void OnDrawGizmosSelected()
	{
		Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		if (Grounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
	}
}

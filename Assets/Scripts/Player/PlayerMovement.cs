using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public bool CanMove = true;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 7f;

    [Header("Jump and Gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float coyoteTime = 0.2f; // Duration for coyote time
    private float _coyoteTimeCounter;
    [SerializeField] private int maxJumpCount = 1; // Number of jumps allowed
    private int _jumpCount;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f; // Distance of the dash
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash
    [SerializeField] private float dashCooldown = 1f; // Cooldown between dashes
    private float _dashTime;
    private bool _isDashing;

    [Header("CACHE")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform childGFX;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioSource footStepAudioSource;
    [SerializeField] private List<AudioClip> footStepClips = new List<AudioClip>();
    private int currentFootStop;

    private bool _isLanding = true;
    private CharacterController _controller;
    public Vector2 _moveInput { get; private set; }
    private Vector3 _velocity;
    private bool _isGrounded;

    // Invoke on speed
    public delegate void OnSpeedChange(float speed);
    public static event OnSpeedChange onSpeedChange;

    public event System.EventHandler OnJumpEvent;
    public event System.EventHandler OnDashEvent;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        _jumpCount = maxJumpCount;
    }

    private void Update()
    {
        if (!CanMove)
        {
            onSpeedChange?.Invoke(0);
            return;
        }

        if (_isDashing)
        {
            HandleDash();
            return;
        }

        CheckGroundStatus();
        CalculateMovement();
        CalculateRotation();
        CalculateGravity();
        UpdateCoyoteTime();
        HandleFlip();
    }

    public void FootStepEVENT()
    {
        if (_isGrounded)
        {
            currentFootStop = (currentFootStop + 1) % footStepClips.Count;
            footStepAudioSource.PlayOneShot(footStepClips[currentFootStop]);
        }
    }

    private void CheckGroundStatus()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded)
        {
            if (_velocity.y < 0)
                _velocity.y = -2f; // Small value to keep the player grounded

            // Reset jump count when grounded
            if (_isLanding)
                _jumpCount = maxJumpCount;
        }
    }

    private void CalculateMovement()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        move = cameraTransform.TransformDirection(move);
        move.y = 0;

        // Normalize movement
        if (move.magnitude > 1f) move.Normalize();

        // Set Movement
        _controller.Move(move * speed * Time.deltaTime);

        // Update Speed Delegate
        float currentSpeed = move.magnitude * speed;
        onSpeedChange?.Invoke(currentSpeed);
    }

    private void CalculateRotation()
    {
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        move = cameraTransform.TransformDirection(move);
        move.y = 0;

        // Rotate towards move direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * 100);
        }
    }

    private void CalculateGravity()
    {
        if (!_isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void UpdateCoyoteTime()
    {
        if (_isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else if (_isLanding || _jumpCount <= 0)
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleDash()
    {
        if (Time.time >= _dashTime)
        {
            _isDashing = false; // End dash when time expires
            return;
        }

        // Move player in dash direction
        Vector3 dashDirection = cameraTransform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)).normalized;
        dashDirection.y = 0;

        _controller.Move(dashDirection * (dashDistance / dashDuration) * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _coyoteTimeCounter > 0)
        {
            if (_jumpCount > 0)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _jumpCount--;
                _isLanding = false;
                Invoke("SetIsLanding", 0.3f);
                OnJumpEvent?.Invoke(this, System.EventArgs.Empty);
            }
            else
                _coyoteTimeCounter = 0; // Reset coyote time counter
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time >= _dashTime + dashCooldown)
            {
                _isDashing = true;
                _dashTime = Time.time + dashDuration; // Set end time for the dash
                SoundManager.PlayAudioClip(dashClip);
                OnDashEvent?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }

    private void SetIsLanding()
    {
        _isLanding = true;
    }

    private void HandleFlip()
    {
        // Flip the player based on movement direction
        if (_moveInput.x != 0)
        {
            Vector3 scale = childGFX.localScale;
            scale.x = Mathf.Sign(_moveInput.x) * Mathf.Abs(scale.x);
            childGFX.localScale = scale;
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}

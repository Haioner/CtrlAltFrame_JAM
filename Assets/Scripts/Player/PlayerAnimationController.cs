using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float tiltAmount = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float animationSmoothTime = 0.1f;

    [SerializeField] private Animator anim;

    private PlayerMovement _playerMovement;
    private Quaternion _initialRotation;
    private Vector3 _targetTilt;
    private float _currentSpeed;
    private float _speedVelocity;

    private void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        if (_playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on the same GameObject.");
            return;
        }

        _initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (_playerMovement.CanMove)
        {
            UpdateRotation();
            UpdateTilt();
            MovementAnimation();
        }
    }

    private void MovementAnimation()
    {
        float targetSpeed = _playerMovement._moveInput.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedVelocity, animationSmoothTime);
        anim.SetFloat("Speed", _currentSpeed);
    }

    private void UpdateRotation()
    {
        Vector3 moveInput = new Vector3(_playerMovement._moveInput.x, 0, _playerMovement._moveInput.y);
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation * targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateTilt()
    {
        Vector3 moveInput = new Vector3(_playerMovement._moveInput.x, 0, _playerMovement._moveInput.y);
        if (moveInput != Vector3.zero)
        {
            float tiltX = moveInput.z * tiltAmount;
            float tiltZ = -moveInput.x * tiltAmount;
            _targetTilt = new Vector3(tiltX, 0, tiltZ);
        }
        else
        {
            _targetTilt = Vector3.zero;
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, _initialRotation * Quaternion.Euler(_targetTilt), tiltSpeed * Time.deltaTime);
    }
}

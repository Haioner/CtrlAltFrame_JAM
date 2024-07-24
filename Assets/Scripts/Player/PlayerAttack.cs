using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Transform attackPivot;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float attackCooldown = 0.5f; // Cooldown duration in seconds
    public bool IsFiring { get; private set; }

    [Header("CACHE")]
    [SerializeField] private LayerMask damageableLayerMask;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private AudioClip attackClip;

    private PlayerManager playerManager;
    private Camera _camera;
    private float _nextAttackTime = 0f;
    private Vector3 _targetPoint;
    private float currentAudioPitch = 1f;
    private float currentAudioVolume = 1f;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        _camera = Camera.main;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsFiring = true;
        }
        else if (context.canceled)
        {
            IsFiring = false;
        }
    }

    private void Update()
    {
        if (playerManager.CurrentEnergy <= 0) 
        {
            DisableLineRenderer();
            currentAudioPitch = 1;
            currentAudioVolume = 1;
            return;
        }

        if (IsFiring)
        {
            UpdateLine(_targetPoint);
            if (Time.time >= _nextAttackTime)
            {
                Attack();
                _nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            DisableLineRenderer();
            currentAudioPitch = 1;
            currentAudioVolume = 1;
        }
    }

    private void Attack()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, damageableLayerMask))
        {
            _targetPoint = hit.point;
            Ray attackRay = new Ray(attackPivot.position, _targetPoint - attackPivot.position);
            if (Physics.Raycast(attackRay, out hit, Vector3.Distance(attackPivot.position, _targetPoint), damageableLayerMask))
            {
                _targetPoint = hit.point;
                float offsetDistance = -0.5f;
                _targetPoint = hit.point - (hit.normal * offsetDistance);

                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(damage);
                }
            }
        }
        else
        {
            _targetPoint = ray.GetPoint(100f);
        }

        currentAudioPitch -= Time.deltaTime * 0.5f;
        currentAudioPitch = Mathf.Clamp(currentAudioPitch, 0.7f, 1);
        currentAudioVolume -= Time.deltaTime * 0.1f;
        currentAudioVolume = Mathf.Clamp(currentAudioVolume, 0.1f, 1f);
        SoundManager.PlayAudioClipVolumeAndPitch(attackClip, currentAudioVolume, currentAudioPitch);

        SpawnHitParticle();
    }

    private void UpdateLine(Vector3 targetPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, attackPivot.position);
        lineRenderer.SetPosition(1, targetPoint);
    }

    private void DisableLineRenderer()
    {
        lineRenderer.enabled = false;
    }

    private void SpawnHitParticle()
    {
        Instantiate(hitParticle, _targetPoint, Quaternion.identity);
    }
}

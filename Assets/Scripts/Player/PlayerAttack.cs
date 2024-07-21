using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Transform attackPivot;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float shockTime = 0.1f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("CACHE")]
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask damageableLayerMask;
    [SerializeField] private AudioClip attackClip;

    private Camera _camera;
    private float _nextAttackTime = 0f;
    private bool _isFiring = false;

    private void Start()
    {
        _camera = Camera.main;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isFiring = true;
        }
        else if (context.canceled)
        {
            _isFiring = false;
        }
    }

    private void Update()
    {
        if (_isFiring && Time.time >= _nextAttackTime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        _nextAttackTime = Time.time + attackCooldown;

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, damageableLayerMask))
        {
            targetPoint = hit.point;
            Ray attackRay = new Ray(attackPivot.position, targetPoint - attackPivot.position);
            if (Physics.Raycast(attackRay, out hit, Vector3.Distance(attackPivot.position, targetPoint), damageableLayerMask))
            {
                targetPoint = hit.point;

                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(damage);
                }
            }
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        SoundManager.PlayAudioClip(attackClip);
        CreateLine(targetPoint);
        SpawnHitParticle(targetPoint);
        CinemachineShake.instance.ShakeCamera(1, shockTime);
    }

    private void CreateLine(Vector3 targetPoint)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, attackPivot.position);
        lineRenderer.SetPosition(1, targetPoint);
        Invoke("DisableLineRenderer", shockTime);
    }

    private void SpawnHitParticle(Vector3 targetPoint)
    {
        Instantiate(hitParticle, targetPoint, Quaternion.identity);
    }

    private void DisableLineRenderer()
    {
        lineRenderer.enabled = false;
    }
}

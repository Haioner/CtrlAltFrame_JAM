using UnityEngine.Events;
using UnityEngine;

public class MemoryFlowerController : MonoBehaviour, IDamageable
{
    [Header("Flower")]
    [SerializeField] private FlowerSO flowerSO;
    [SerializeField] private SpriteRenderer flowerRenderer;
    [SerializeField] private SpriteRenderer orbRenderer;
    [SerializeField] private ParticleSystem fullyParticle;
    [SerializeField] private AudioClip fullyClip;

    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [Range(0f, 4f)][SerializeField] private float currentEnergy;

    [Space]
    [SerializeField] private UnityEvent OnDamage;
    [SerializeField] private UnityEvent OnFullyEvent;

    private bool isFully;

    private void Start()
    {
        UpdateFlowerEmission();
        UpdateFlowerSprite();
    }

    public void Damage(float damage)
    {
        currentEnergy += damage;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        OnDamage?.Invoke();

        UpdateFlowerEmission();
        UpdateFlowerSprite();
        CheckFullOfEnergy();
    }

    private void UpdateFlowerEmission()
    {
        orbRenderer.material.SetColor("_EmissionColor", flowerSO.colorList[GetEnergyIndex(flowerSO.colorList.Count)]);
    }

    private void UpdateFlowerSprite()
    {
        flowerRenderer.sprite = flowerSO.flowerSprites[GetEnergyIndex(flowerSO.flowerSprites.Count)];
    }

    private void CheckFullOfEnergy()
    {
        if (currentEnergy >= maxEnergy && !isFully)
        {
            SoundManager.PlayAudioClip(fullyClip);
            isFully = true;
            OnFullyEvent?.Invoke();
            Instantiate(fullyParticle, transform);
        }
    }

    private int GetEnergyIndex(int listCount)
    {
        int index = Mathf.FloorToInt((currentEnergy / maxEnergy) * (listCount - 1));
        return Mathf.Clamp(index, 0, listCount - 1);
    }
}

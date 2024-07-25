using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteRenderersList
{
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    public void ChangeMaterialsColor(Color newColor)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.SetColor("_EmissionColor", newColor);
        }
    }
}

public class PlayerManager : MonoBehaviour
{
    [Header("Electric")]
    [ColorUsage(true, true)][SerializeField] private Color playerColor;
    [SerializeField] private SpriteRenderersList playerRenderers;
    [SerializeField] private LineRenderer shockLineRenderer;
    [SerializeField] private Light playerLight;

    [Header("Energy")]
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyChangeRate = 10f;
    public float CurrentEnergy { get;private set; }

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    private PlayerAnimationController playerAnimation;

    public static Vector3 checkPointPos;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponentInChildren<PlayerAnimationController>();
        SpawnCheckPoint();
        CurrentEnergy = maxEnergy;
    }

    private void Update()
    {
        CalculatePlayerEnergy();
        CalculatePlayerColor();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
            transform.position = new Vector3(9.63f, 0, -23.38f);
#endif
    }

    public void ClearCheckpoint()
    {
        checkPointPos = new Vector3(9.63f, 0, -23.38f);
    }

    private void SpawnCheckPoint()
    {
        if (checkPointPos != Vector3.zero)
            transform.position = checkPointPos;
    }

    public void SetPlayerControl(bool controlState)
    {
        playerAttack.enabled = controlState;
        playerMovement.enabled = controlState;
        playerAnimation.ResetAnimation();
        playerAnimation.enabled = controlState;
    }

    public float GetMaxEnergy() { return maxEnergy; }

    private void CalculatePlayerEnergy()
    {
        if (playerAttack.IsFiring)
        {
            CurrentEnergy = Mathf.Max(CurrentEnergy - energyChangeRate * Time.deltaTime, 0);
        }
        else
        {
            CurrentEnergy = Mathf.Min(CurrentEnergy + energyChangeRate * Time.deltaTime * 2, maxEnergy);
        }
    }

    private void CalculatePlayerColor()
    {
        float emissionIntensity = Mathf.Lerp(0, 5, CurrentEnergy / (maxEnergy * 5));
        Color emissionColor = playerColor * emissionIntensity;
        playerRenderers.ChangeMaterialsColor(emissionColor);

        shockLineRenderer.material.SetColor("_EmissionColor", emissionColor);

        playerLight.intensity = emissionIntensity;
    }
}

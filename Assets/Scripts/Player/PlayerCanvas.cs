using UnityEngine.UI;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Slider energySlider;

    private void Start()
    {
        energySlider.maxValue = playerManager.GetMaxEnergy();
    }

    private void Update()
    {
        energySlider.value = playerManager.CurrentEnergy;
    }
}

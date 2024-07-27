using System.Collections;
using UnityEngine;
using Fluxy;

public class FluidController : MonoBehaviour
{
    [SerializeField] private FluxyTarget target;
    [SerializeField] private Vector2 rateStep = new Vector2(4,100);
    [SerializeField] private float speedRate = 80f;
    [SerializeField] private bool canStartRate = true;
    private float currentRateStep;

    private void Awake()
    {
        currentRateStep = rateStep.y;

        if (canStartRate)
            StartCoroutine(ReduceRateStepOverTime());
    }

    private void RateStep()
    {
        target.rateOverSteps = Mathf.RoundToInt(currentRateStep);
    }

    public void StartRate()
    {
        StartCoroutine(ReduceRateStepOverTime());
    }

    private IEnumerator ReduceRateStepOverTime()
    {
        while (currentRateStep > rateStep.x)
        {
            currentRateStep -= speedRate * Time.deltaTime;
            RateStep();
            yield return null;
        }

        // Ensure the final rate step is set to rateStep.x
        currentRateStep = rateStep.x;
        RateStep();
    }
}

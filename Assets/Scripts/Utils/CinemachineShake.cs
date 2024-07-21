using Unity.Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get; private set; }
    private CinemachineBasicMultiChannelPerlin m_MultiChannelPerlin;
    private float startingIntensity;
    private float shakeTimerTotal;
    private float shakeTimer;

    private void Awake()
    {
        instance = this;
        m_MultiChannelPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        m_MultiChannelPerlin.AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                m_MultiChannelPerlin.AmplitudeGain =
                Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }
}

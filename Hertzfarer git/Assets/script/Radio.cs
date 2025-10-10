using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField]
    private Transform radioTower;

    [SerializeField, Header("Distance Settings")]
    private float minDistance = 5f;

    [SerializeField]
    private float maxDistance = 30f;

    [SerializeField, Header("Static Settings")]
    private AudioClip staticNoiseClip;

    [SerializeField, Range(0f, 1f)]
    private float maxStaticVolume = 0.5f;

    [Header("Low-Pass Filter")]
    [SerializeField]
    private bool useLowPassFilter = true;
    [SerializeField]
    private float minCutoffFreq = 5000f;  // Near
    [SerializeField]
    private float maxCutoffFreq = 500f;   // Far

    private AudioSource source;
    private AudioSource staticSource;
    private AudioLowPassFilter lowPass;

    void Start()
    {
        // Main source
        source = GetComponent<AudioSource>();

        // Create a second AudioSource for static noise
        GameObject staticObj = new GameObject("StaticNoise");
        staticObj.transform.SetParent(transform);
        staticObj.transform.localPosition = Vector3.zero;

        staticSource = staticObj.AddComponent<AudioSource>();
        staticSource.clip = staticNoiseClip;
        staticSource.loop = true;
        staticSource.volume = 0f; // start silent
        staticSource.spatialBlend = 1f; // 3D sound
        staticSource.rolloffMode = AudioRolloffMode.Linear;
        staticSource.minDistance = minDistance;
        staticSource.maxDistance = maxDistance;
        staticSource.Play();

        // Optional low-pass filter for muffling
        if (useLowPassFilter)
        {
            lowPass = gameObject.AddComponent<AudioLowPassFilter>();
            lowPass.cutoffFrequency = minCutoffFreq;
        }
    }

    void Update()
    {
        if (radioTower == null) return;

        float distance = Vector3.Distance(transform.position, radioTower.position);
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        t = Mathf.SmoothStep(0f, 1f, t);

        // Increase static volume with distance
        staticSource.volume = Mathf.Lerp(0f, maxStaticVolume, t);

        // Lower main audio volume slightly as distance increases
        source.volume = Mathf.Lerp(1f, 0.6f, t);
        source.volume *= Random.Range(0.9f, 1f - t * 0.3f);
        // Apply low-pass filter for muffling
        if (useLowPassFilter && lowPass != null)
        {
            lowPass.cutoffFrequency = Mathf.Lerp(minCutoffFreq, maxCutoffFreq, t);
        }
    }
}

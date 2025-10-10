using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField]
    private Transform radioTower;

    [SerializeField, Header("Distance Settings")]
    [Tooltip("Minimum distance where there is no distortion.")]
    private float minDistance = 5f;

    [SerializeField, Tooltip("Maximum distance where distortion is strongest.")]
    private float maxDistance = 30f;

    [SerializeField, Header("Distortion Settings"), Tooltip("Distortion amount at maximum distance (0 to 1)."), Range(0f, 1f)]
    private float maxDistortion = 0.8f;

    private AudioDistortionFilter distortionFilter;

    void Start()
    {
        distortionFilter = GetComponent<AudioDistortionFilter>();

        if (radioTower == null)
        {
            Debug.LogWarning("No target assigned to DistanceDistortion on " + gameObject.name);
        }
    }

    void Update()
    {
        if (radioTower == null) return;

        float distance = Vector3.Distance(transform.position, radioTower.position);

        // Normalize distance (0 = minDistance, 1 = maxDistance)
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

        // Smooth interpolation for more natural feel
        t = Mathf.SmoothStep(0f, 1f, t);

        // Apply distortion level
        distortionFilter.distortionLevel = Mathf.Lerp(0f, maxDistortion, t);
    }
}

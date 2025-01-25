using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(Rigidbody))]
public class Soft_Physics : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    public float waveAmplitude = 1f;
    public float waveFrequency = 1f;
    public float waveSpeed = 1f;
    public float smoothing = 5f;
    public float damping = 0.95f;
    public float stiffness = 5f;
    public float waveSpread = 5f;
    public float gravityInfluence = 1f;
    public float maxWaveForce = 1f;
    public float collisionCooldown = 0.1f;

    private float currentCollisionForce;
    private Vector3 currentCollisionDirection;
    private Vector3 collisionPoint;
    private float waveTime;
    private float lastCollisionTime;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic(); // Optimization: Indicate mesh will change frequently
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    private void Update()
    {
        if (currentCollisionForce > 0.01f)
        {
            waveTime += Time.deltaTime;
            float deltaTimeSmoothing = Time.deltaTime * smoothing;

            // Precompute repetitive calculations
            float waveBaseValue = waveAmplitude
                * Mathf.Clamp(currentCollisionForce, 0, maxWaveForce);

            for (int i = 0; i < originalVertices.Length; i++)
            {
                Vector3 originalVertex = originalVertices[i];
                Vector3 worldVertex = transform.TransformPoint(originalVertex);

                // Use squared magnitude to avoid square root
                float sqrDistanceFromImpact = Vector3.SqrMagnitude(worldVertex - collisionPoint);
                float distanceFromImpact = Mathf.Sqrt(sqrDistanceFromImpact);
                float verticalPosition = worldVertex.y - transform.position.y;

                float gravityFactor = Mathf.Clamp01(verticalPosition);
                float wave = Mathf.Sin(waveTime * waveSpeed - distanceFromImpact * waveFrequency)
                    * waveBaseValue
                    * Mathf.Exp(-distanceFromImpact / waveSpread)
                    * (1f + gravityFactor * gravityInfluence);

                displacedVertices[i] = Vector3.Lerp(
                    displacedVertices[i],
                    originalVertex + currentCollisionDirection * wave,
                    deltaTimeSmoothing
                );
            }

            // Only update mesh if deformation is significant
            mesh.vertices = displacedVertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            currentCollisionForce = Mathf.Max(0, currentCollisionForce - Time.deltaTime);
        }
        else
        {
            waveTime = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - lastCollisionTime >= collisionCooldown)
        {
            currentCollisionForce = Mathf.Clamp(collision.relativeVelocity.magnitude / stiffness, 0, maxWaveForce);
            currentCollisionDirection = collision.relativeVelocity.normalized;
            collisionPoint = collision.contacts[0].point;
            waveTime = 0;
            lastCollisionTime = Time.time;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentCollisionForce = Mathf.Clamp(collision.relativeVelocity.magnitude / stiffness, 0, maxWaveForce);
        currentCollisionDirection = collision.relativeVelocity.normalized;
        collisionPoint = collision.contacts[0].point;
        waveTime = 0;
        lastCollisionTime = Time.time;
    }
}
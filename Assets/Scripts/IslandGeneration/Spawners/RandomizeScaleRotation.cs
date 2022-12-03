using UnityEngine;

public class RandomizeScaleRotation : MonoBehaviour
{
    [Header("Uniform Scaling Properties")]
    [SerializeField] private bool scaleUniformly = true;

    [Space]
    [SerializeField] private float uniformScaleMin = .1f;
    [SerializeField] private float uniformScaleMax = 1f;

    [Header("Non-Uniform Scaling Properties")]
    [SerializeField] private float xScaleMin = .1f;
    [SerializeField] private float xScaleMax = 3f;
    [SerializeField] private float yScaleMin = .1f;
    [SerializeField] private float yScaleMax = 3f;
    [SerializeField] private float zScaleMin = .1f;
    [SerializeField] private float zScaleMax = 3f;


    public Quaternion RandomizeYRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);

    }

    public Vector3 RandomizeObjectScale(GameObject objectToSpawn)
    {
        Vector3 randomizedScale = Vector3.one;

        if (scaleUniformly)
        {
            float uniformScale = Random.Range(uniformScaleMin, uniformScaleMax);
            randomizedScale = new Vector3(uniformScale, uniformScale, uniformScale);
        }
        else
        {
            randomizedScale = new Vector3(Random.Range(xScaleMin, xScaleMax), Random.Range(yScaleMin, yScaleMax), Random.Range(zScaleMin, zScaleMax));
        }

        float objectCurrentScale = objectToSpawn.transform.localScale.x;

        return randomizedScale * objectCurrentScale;
    }
}

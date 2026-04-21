using UnityEngine;
using UnityEngine.Serialization;

public class ItemAnim : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float frequency = 1f;
    public Vector3 rotationSpeed = new Vector3(0f, 30f, 0f);
    
    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.localPosition;
    }

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime);

        float time = Time.time;
        float newY = amplitude * Mathf.Sin(time * frequency * Mathf.PI * 2f);
        transform.localPosition = _startPos + new Vector3(0f, newY, 0f);
    }
}

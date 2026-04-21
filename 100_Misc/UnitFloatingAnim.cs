using UnityEngine;

namespace _01_Script._100_Misc
{
    public class UnitFloatingAnim : MonoBehaviour
    {
        public float floatSpeed = 1f;
        public float floatHeight = 0.2f;
        private Vector3 startPosition;
        private float phaseOffset;

        void Start()
        {
            startPosition = transform.localPosition;
            phaseOffset = Random.Range(0f, Mathf.PI * 2f);
        }

        void Update()
        {
            float newY = Mathf.Cos(Time.time * floatSpeed + phaseOffset) * floatHeight;
            transform.localPosition = startPosition + new Vector3(0f, newY, 0f);
        }
    }
}

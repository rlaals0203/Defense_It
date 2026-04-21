using UnityEngine;

namespace _01_Script._100_Misc
{
    [CreateAssetMenu(fileName = "DestinationFinderSO", menuName = "SO/DestinationFinderSO")]
    public class DestinationFinderSO : ScriptableObject
    {
        public Transform Destination { get; private set; } 

        public void SetTarget(Transform target)
        {
            Destination = target;
        }
    }
}

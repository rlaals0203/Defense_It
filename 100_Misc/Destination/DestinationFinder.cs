using MinLibrary.Dependencies;
using UnityEngine;

namespace _01_Script._100_Misc.Destination
{
    [Provide]
    public class DestinationFinder : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private Transform destination;
        [SerializeField] private DestinationFinderSO destinationFinder;

        private void Start()
        {
            destinationFinder.SetTarget(destination);
        }

        public Transform SetDestination() => destination;
    }
}

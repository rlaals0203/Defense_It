using UnityEngine;

namespace _01_Script._00_Core.ETC
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public abstract void StopFeedback();
    }
}
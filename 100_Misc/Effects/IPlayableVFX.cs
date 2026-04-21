using UnityEngine;

namespace _01_Script._100_Misc.Effects
{
    public interface IPlayableVFX
    {
        public string VFXName { get; }
        public void PlayVFX(Vector3 position,Quaternion rotation);
        public void StopVFX();
    }
}
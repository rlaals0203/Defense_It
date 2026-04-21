using System.Collections.Generic;
using System.Linq;
using _01_Script._100_Misc.Effects;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._01_Entities
{
    public class EntityVFX : MonoBehaviour, IEntityComponent
    {
        private Dictionary<string, IPlayableVFX> _playableDictionary;
        
        public void Initialize(Entity entity)
        {
            _playableDictionary = new Dictionary<string, IPlayableVFX>();
            GetComponentsInChildren<IPlayableVFX>().ToList()
                .ForEach(playable => _playableDictionary.Add(playable.VFXName, playable));
        }

        public void PlayVFX(string vfxName, Vector3 position, Quaternion rotation)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist");
            vfx.PlayVFX(position, rotation);
        }

        public void StopVFX(string vfxName)
        {
            IPlayableVFX vfx = _playableDictionary.GetValueOrDefault(vfxName);
            Debug.Assert(vfx != default(IPlayableVFX), $"{vfxName} is not exist");
            vfx.StopVFX();
        }
    }
}
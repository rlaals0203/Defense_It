using System.Linq;
using UnityEngine;

namespace _01_Script._100_Misc
{
    public class InvisibleGround : MonoBehaviour
    {
        // StartCommand is called once before the first execution of UpdateCommand after the MonoBehaviour is created
        void Awake()
        {
            GetComponentsInChildren<MeshRenderer>().ToList().ForEach(m =>
            {
                m.enabled = false;
            });
        }
    }
}

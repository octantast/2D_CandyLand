using UnityEngine;

namespace rIAEugth.vseioAW.Game
{
    public class Appps:MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
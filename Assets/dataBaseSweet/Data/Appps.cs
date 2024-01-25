using UnityEngine;

namespace dataBaseSweet.Data
{
    public class Appps:MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
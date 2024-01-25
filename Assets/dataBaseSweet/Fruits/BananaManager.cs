using UnityEngine;

namespace BananaWorld.Fruits
{
    public class BananaManager : MonoBehaviour
    {
        public void SetupBanana()
        {
            UniWebView.SetAllowInlinePlay(true);

            var audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (var audioSource in audioSources)
            {
                audioSource.Stop();
            }

            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Sound
{
    sealed public class UISoundEnabler : MonoBehaviour
    {
        public void Start()
        {
            foreach (var button in GetComponentsInChildren<Button>(true))
            {
                button.onClick.AddListener(PlayUISound);
            }

            foreach (var toggle in GetComponentsInChildren<Toggle>(true))
            {
                toggle.onValueChanged.AddListener(b => PlayUISound());
            }
        }

        public void PlayUISound()
        {
            PlaySoundEvent.Dispatch(SoundType.UIClick);
        }
    }
}

using UnityEngine;
using TakiExtensions.Audio;
using UI;

namespace TakiExtensions.Audio
{
    public class SoundButtonController : MonoBehaviour
    {
        [SerializeField] private HoverButton[] buttons;
        [SerializeField] private SeKind[] seKinds;

        private void Start()
        {
            if (buttons.Length != seKinds.Length)
            {
                Debug.LogError("Buttons and SeKinds length mismatch!");
                return;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => PlaySe(index));
            }
        }

        private void PlaySe(int index)
        {
            if (index >= 0 && index < seKinds.Length)
            {
                SeSingleton.Instance.Play(seKinds[index]);
            }
        }
    }
}

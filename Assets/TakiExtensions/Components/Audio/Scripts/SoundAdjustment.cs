using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TakiExtensions.TakiExtension.SoundAdjustment
{
    public class PointerUpHandler : MonoBehaviour, IEndDragHandler
    {
        public event UnityEngine.Events.UnityAction OnPointerUpEvent;

        public void OnEndDrag(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke();
        }
    }

    public class SoundAdjustment : MonoBehaviour
    {
        [SerializeField] AudioMixer mixer;
        [SerializeField] string mixerParameter_BGMVolume = "BGMVolume";
        [SerializeField] string mixerParameter_SEVolume = "SEVolume";

        [SerializeField] Slider BGMSlider;
        [SerializeField] Slider SESlider;
        [SerializeField] AudioSource SETestAudio;
        [SerializeField] GameObject ButtonObject;

        private void Awake()
        {
            if (BGMSlider != null)
            {
                BGMSlider.onValueChanged.AddListener(BGMChanged);
            }

            if (SESlider != null)
            {
                SESlider.onValueChanged.AddListener(SEChanged);

                // SEスライダーから手を離した時に音を鳴らす
                AddPointerUpListener(() =>
                {
                    SETestAudio?.Play();
                });
            }
        }

        private void AddPointerUpListener(UnityEngine.Events.UnityAction action)
        {
            if (ButtonObject == null) return;

            PointerUpHandler handler = ButtonObject.GetComponent<PointerUpHandler>();
            if (handler == null)
            {
                handler = ButtonObject.AddComponent<PointerUpHandler>();
            }

            handler.OnPointerUpEvent += action;
        }


        void BGMChanged(float slider)
        {
            if (mixer == null) return;
            mixer.SetFloat(mixerParameter_BGMVolume, Liner2dB(slider));
        }

        void SEChanged(float slider)
        {
            if (mixer == null) return;
            mixer.SetFloat(mixerParameter_SEVolume, Liner2dB(slider));
        }


        float Liner2dB(float liner)
        {
            if (liner < 0.001f)
            {
                liner = 0.001f;
            }
            return 20 * Mathf.Log10(liner);
        }


    }

}
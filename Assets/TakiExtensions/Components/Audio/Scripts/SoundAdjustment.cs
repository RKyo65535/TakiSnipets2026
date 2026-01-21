using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ThisGame.Component.Audio
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
        public const string mixerParameter_BGMVolume = "BGMVolume";
        public const string mixerParameter_SEVolume = "SEVolume";

        [SerializeField] Slider BGMSlider;
        [SerializeField] Slider SESlider;
        [SerializeField] AudioSource SETestAudio;
        [SerializeField] GameObject ButtonObject;

        private const float DEFAULT_VOLUME = 0.8f;

        private void Awake()
        {
            // 起動時に保存値を読み込んでスライダーとAudioMixerに適用
            LoadVolume();

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

        private void OnDestroy()
        {
            // 安全のため破棄時にも保存
            SaveVolume();
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

        // PlayerPrefs から読み込み、スライダーとMixerに反映
        private void LoadVolume()
        {
            float bgmVolume = PlayerPrefs.GetFloat(mixerParameter_BGMVolume, DEFAULT_VOLUME);
            float seVolume = PlayerPrefs.GetFloat(mixerParameter_SEVolume, DEFAULT_VOLUME);

            if (BGMSlider != null)
            {
                // スライダーの値を直接設定しても、ここではMixerに反映する
                BGMSlider.value = bgmVolume;
                if (mixer != null)
                {
                    mixer.SetFloat(mixerParameter_BGMVolume, Liner2dB(bgmVolume));
                }
            }

            if (SESlider != null)
            {
                SESlider.value = seVolume;
                if (mixer != null)
                {
                    mixer.SetFloat(mixerParameter_SEVolume, Liner2dB(seVolume));
                }
            }
        }

        // 現在のスライダー値をPlayerPrefsに保存
        private void SaveVolume()
        {
            if (BGMSlider != null)
            {
                PlayerPrefs.SetFloat(mixerParameter_BGMVolume, BGMSlider.value);
            }

            if (SESlider != null)
            {
                PlayerPrefs.SetFloat(mixerParameter_SEVolume, SESlider.value);
            }

            PlayerPrefs.Save();
        }

        void BGMChanged(float slider)
        {
            if (mixer == null) return;
            mixer.SetFloat(mixerParameter_BGMVolume, Liner2dB(slider));

            // 値が変わったら即時保存
            PlayerPrefs.SetFloat(mixerParameter_BGMVolume, slider);
            PlayerPrefs.Save();
        }

        void SEChanged(float slider)
        {
            if (mixer == null) return;
            mixer.SetFloat(mixerParameter_SEVolume, Liner2dB(slider));

            // 値が変わったら即時保存
            PlayerPrefs.SetFloat(mixerParameter_SEVolume, slider);
            PlayerPrefs.Save();
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
using System.Collections.Generic;
using UnityEngine;

namespace TakiExtensions.Audio
{
    public class BgmSingleton : MonoBehaviourSingleton<BgmSingleton>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<BgmInformation> bgms;
        private int currentBgmIndex = 0;

        private void Start()
        {
            if (bgms == null || bgms.Count == 0) return;
            PlayNextBgm();
        }

        private void Update()
        {
            if (audioSource == null) return;
            if (bgms == null || bgms.Count == 0) return;

            if (audioSource.isPlaying)
            {
                // ループ処理
                var currentBgm = bgms[currentBgmIndex];
                if (currentBgm.loopEndPoint > 0 && audioSource.timeSamples >= currentBgm.loopEndPoint)
                {
                    audioSource.timeSamples = currentBgm.loopStartPoint;
                }
            }
            else
            {
                PlayNextBgm();
            }
        }

        private void PlayNextBgm()
        {
            if (bgms.Count == 1)
            {
                currentBgmIndex = 0;
            }
            else
            {
                // 現在再生中のもの以外からランダムに選択
                int nextIndex = (currentBgmIndex + UnityEngine.Random.Range(1, bgms.Count)) % bgms.Count;
                currentBgmIndex = nextIndex;
            }

            var audioInfo = bgms[currentBgmIndex];
            audioSource.clip = audioInfo.clip;
            audioSource.loop = audioInfo.loopEndPoint <= 0; // ループポイントが設定されていない場合は通常のループ
            audioSource.Play();
        }

        protected override void LateAwake()
        {
        }
    }
}
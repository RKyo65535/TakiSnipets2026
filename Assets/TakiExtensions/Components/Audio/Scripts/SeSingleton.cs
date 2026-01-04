using System.Collections.Generic;
using UnityEngine;

namespace TakiExtensions.Audio
{
    public class SeSingleton : MonoBehaviourSingleton<SeSingleton>
    {
        [SerializeField] private AudioSource audioSourcePrefab;
        private readonly Dictionary<AudioClip, List<AudioSource>> audioSourceMap = new Dictionary<AudioClip, List<AudioSource>>();
        [SerializeField] private SeList seList;
        private const int MaxConcurrentSounds = 3;

        public void Play(SeKind kind)
        {
            var seInfo = seList.GetSoundClip(kind);
            if (seInfo != null && seInfo.clip != null)
            {
                Play(seInfo.clip);
            }
        }

        public void Play(AudioClip clip)
        {
            if (clip == null) return;

            AudioSource source = GetDedicatedSource(clip);
            source.clip = clip;
            source.Play();
        }

        private AudioSource GetDedicatedSource(AudioClip clip)
        {
            if (!audioSourceMap.TryGetValue(clip, out var sources))
            {
                sources = new List<AudioSource>();
                audioSourceMap.Add(clip, sources);
            }

            // 再生中でないソースを探す
            foreach (var source in sources)
            {
                if (!source.isPlaying) return source;
            }

            // 制限数未満なら新規作成
            if (sources.Count < MaxConcurrentSounds)
            {
                var newSource = Instantiate(audioSourcePrefab, transform);
                newSource.name = $"AudioSource_{clip.name}_{sources.Count}";
                sources.Add(newSource);
                return newSource;
            }

            // 全て再生中の場合、最も再生が進んでいる（残り時間が短い）ものを探す
            // ここでは単純に、リストの先頭（最初に作ったもの）を再利用するか、
            // もしくは再生時間が一番長いものを探す。
            AudioSource oldestSource = sources[0];
            float maxTime = -1f;
            foreach (var source in sources)
            {
                if (source.time > maxTime)
                {
                    maxTime = source.time;
                    oldestSource = source;
                }
            }

            return oldestSource;
        }

        protected override void LateAwake()
        {
        }
    }
}
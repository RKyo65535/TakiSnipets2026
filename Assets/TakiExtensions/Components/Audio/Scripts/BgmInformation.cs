using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TakiExtensions.Audio
{
    [Serializable]
    public class BgmInformation
    {
        public AudioClip clip;
        public int loopStartPoint;
        public int loopEndPoint;
    }
}
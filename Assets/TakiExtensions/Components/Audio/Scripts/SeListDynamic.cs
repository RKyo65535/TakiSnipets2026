// This file is auto-generated
using UnityEngine;

namespace TakiExtensions.Audio
{
    public partial class SeList
    {
        public SeInformation GetSoundClip(SeKind kind)
        {
            switch (kind)
            {
                case SeKind.tools_01:
                    return ses[0];
                case SeKind.tools_02:
                    return ses[1];
                case SeKind.tools_03:
                    return ses[2];
                case SeKind.tools_04:
                    return ses[3];
                case SeKind.tools_05:
                    return ses[4];
                default:
                    return default;
            }
        }
    }

    public enum SeKind
    {
        tools_01,
        tools_02,
        tools_03,
        tools_04,
        tools_05,
    }
}

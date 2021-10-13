using System;

namespace Divit.RhythmRunner
{
    [Serializable]
    public class LevelAudioData
    {
        public string AudioClipName;
        public int AudioClipLength;
        public float AudioClipBpm;
        public float BeatDelayTime;
    }
}
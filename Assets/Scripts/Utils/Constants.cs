using System.Collections.Generic;

namespace Divit.RhythmRunner
{
    public static class Constants
    {
        public static HashSet<int> LEVELS = new HashSet<int>
        {
            1,
            2,
            3,
        };

        public static Dictionary<int, LevelAudioData> LEVEL_AUDIO_DATA = new Dictionary<int, LevelAudioData>
        {
            { 1, new LevelAudioData {
                AudioClipName = "Never Stop",
                AudioClipLength = 282,
                AudioClipBpm = 86, 
                BeatDelayTime = 9.6f
            }},
            { 2, new LevelAudioData { 
                AudioClipName = "Game Over", 
                AudioClipLength = 238,
                AudioClipBpm = 172, 
                BeatDelayTime = 10.5f
            }},
            { 3, new LevelAudioData {
                AudioClipName = "Groove",
                AudioClipLength = 8,
                AudioClipBpm = 112.8f,
                BeatDelayTime = 0
            }}
        };

        public static Dictionary<TutorialPartType, string> TUTORIAL_DATA = new Dictionary<TutorialPartType, string>
        {
            { TutorialPartType.PAUSE, "PAUSE THE GAME BY PRESSING THE MENU BUTTON ON YOUR CONTROLLER"},
            { TutorialPartType.COIN, "COLLECT COINS WITH THE CORRECT CONTROLLER TO EARN POINTS"},
            { TutorialPartType.OBSTACLE, "AVOID OBSTACLES TO EARN POINTS"},
            { TutorialPartType.OBSTACLE_COLLISION, "IF YOU GET HIT BY AN OBSTACLE YOU WILL LOSE HEALTH"},
            { TutorialPartType.DESTRUCTIBLE_OBSTACLE, "YOU CAN DESTROY SOME OBSTACLES BY HITTING THEM WITH THE CORRECT CONTROLLER"}
        };

        public static int COIN_POINTS = 100;
        public static int DEFAULT_POINTS = 100;
        public static int OBSTACLE_POINTS = 250;
        public static int DESTRUCTIBLE_OBSTACLE_POINTS = 500;
        public static int LEVEL_CLEARED_POINTS = 1000;

        public const string CONTROLLER_LEFT_TAG = "Left Controller";
        public const string CONTROLLER_RIGHT_TAG = "Right Controller";
        public const string OBSTACLE_COLLISION_TAG = "Player";

        public const float MOVEABLE_OBJECT_SPEED = 7.5f;

        public const float POSITIVE_HAPTICS_FEEDBACK_AMPLITUDE = 1;
        public const float POSITIVE_HAPTICS_FEEDBACK_DURATION = 0.5f;

        public const float NEGATIVE_HAPTICS_FEEDBACK_AMPLITUDE = 0.5f;
        public const float NEGATIVE_HAPTICS_FEEDBACK_DURATION = 1;

        public static readonly List<string> DEATH_ZONE_COLLISION_TAGS = new List<string> 
        { 
            "Coin", 
            "Obstacle" 
        };

        public static string USE_CHECK_KEY = "WasUsed";
    }
}
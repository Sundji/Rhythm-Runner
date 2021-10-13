using System;

namespace Divit.RhythmRunner
{
    public class HapticImpulseSender
    {
        public static Action<VRControllerType, float, float> OnHapticImpulse;

        public void SendHapticImpulse(VRControllerType type, float amplitude, float duration)
        {
            OnHapticImpulse?.Invoke(type, amplitude, duration);
        }
    }
}
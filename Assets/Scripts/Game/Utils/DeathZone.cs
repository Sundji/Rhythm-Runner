using UnityEngine;

namespace Divit.RhythmRunner
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (Constants.DEATH_ZONE_COLLISION_TAGS.Contains(other.tag))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
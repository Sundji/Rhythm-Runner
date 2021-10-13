using UnityEngine;

public class ParticleSystemDeath : MonoBehaviour
{
    private void Awake()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
    }
}

using System;
using UnityEngine;

namespace Divit.RhythmRunner
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int _maximumHealth = 100;

        public static Action OnDeath;
        public static Action OnHealthChanged;

        public int Health
        {
            get;
            private set;
        }

        public int MaximumHealth
        {
            get
            {
                return _maximumHealth;
            }
        }

        public void ResetPlayerHealth()
        {
            Health = _maximumHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            OnHealthChanged?.Invoke();
            if (Health <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        private void Awake()
        {
            Health = _maximumHealth;
        }
    }
}
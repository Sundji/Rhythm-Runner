using System;
using System.Collections;
using UnityEngine;

namespace Divit.RhythmRunner
{
    [RequireComponent(typeof(PlayerHealth))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerScore))]
    public class Player : MonoBehaviour
    {
        public static Action<int> OnPlayerDeath;

        private bool _isPlaying = true;

        private PlayerHealth _playerHealth;
        private PlayerMovement _playerMovement;
        private PlayerScore _playerScore;

        public int GetHealth()
        {
            return _playerHealth.Health;
        }

        public int GetMaximumHealth()
        {
            return _playerHealth.MaximumHealth;
        }

        public int GetScore()
        {
            return _playerScore.Score;
        }

        public void LevelCleared()
        {
            _isPlaying = false;
            _playerScore.AddToScore(Constants.LEVEL_CLEARED_POINTS);
        }

        public void ResetPlayer()
        {
            _playerHealth.ResetPlayerHealth();
            _playerMovement.ResetPlayerMovement();
            _playerScore.ResetPlayerScore();
            _isPlaying = true;
        }

        private void Awake()
        {
            _playerHealth = GetComponent<PlayerHealth>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerScore = GetComponent<PlayerScore>();

            BasePoints.OnPointsCollected += OnCollect;
            HapticImpulseSender.OnHapticImpulse += OnHapticImpulse;
            Obstacle.OnPlayerHit += OnHit;
            PlayerHealth.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            BasePoints.OnPointsCollected -= OnCollect;
            HapticImpulseSender.OnHapticImpulse -= OnHapticImpulse;
            Obstacle.OnPlayerHit -= OnHit;
            PlayerHealth.OnDeath -= OnDeath;
        }

        private void OnCollect(int points)
        {
            if (_isPlaying)
            {
                _playerScore.AddToScore(points);
            }
        }

        private void OnHapticImpulse(VRControllerType vrControllerType, float amplitude, float duration)
        {
            if (_isPlaying)
            {
                _playerMovement.SendHapticImpulse(vrControllerType, amplitude, duration);
            }
        }

        private void OnHit(int damageAmount)
        {
            if (_isPlaying)
            {
                _playerHealth.TakeDamage(damageAmount);
            }
        }

        private void OnDeath()
        {
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForEndOfFrame();
            _isPlaying = false;
            OnPlayerDeath?.Invoke(_playerScore.Score);
        }
    }
}
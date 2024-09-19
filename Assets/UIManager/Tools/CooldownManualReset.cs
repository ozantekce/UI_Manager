using UnityEngine;

namespace UIManager
{
    /// <summary>
    /// Manages cooldown periods that require manual resets, useful for actions or events 
    /// where automatic reset behavior is not desired.
    /// </summary>
    public class CooldownManualReset
    {
        /// <summary>
        /// The duration of the cooldown in seconds.
        /// </summary>
        private float _cooldownDuration;

        /// <summary>
        /// The time when the cooldown was last activated or reset.
        /// </summary>
        private float _lastActivationTime;

        /// <summary>
        /// Flag to check if it's the first time the cooldown completion is being checked.
        /// This allows the first check to always return true.
        /// </summary>
        private bool _isFirstCheck = true;

        /// <summary>
        /// Constructor that initializes the cooldown with a specific duration and sets the last activation time to the current time.
        /// </summary>
        /// <param name="cooldown">The duration of the cooldown in seconds.</param>
        public CooldownManualReset(float cooldown)
        {
            _cooldownDuration = cooldown;
            _lastActivationTime = Time.time;
        }

        /// <summary>
        /// Updates the cooldown duration to a new value.
        /// </summary>
        /// <param name="cooldown">The new duration of the cooldown in seconds.</param>
        public void SetCooldown(float cooldown)
        {
            _cooldownDuration = cooldown;
        }

        /// <summary>
        /// Checks if the cooldown has completed since the last reset.
        /// The first check after initialization or reset will always return true.
        /// </summary>
        /// <returns>True if the cooldown period is complete, otherwise false.</returns>
        public bool HasCompleted()
        {
            // On the first check, ignore the cooldown duration and return true.
            if (_isFirstCheck)
            {
                _isFirstCheck = false;
                return true;
            }

            // Check if the elapsed time since the last activation exceeds the cooldown duration.
            return ElapsedTime() >= _cooldownDuration;
        }

        /// <summary>
        /// Resets the cooldown timer, updating the last activation time to the current time
        /// and resetting the first check flag.
        /// </summary>
        public void ResetTimer()
        {
            _isFirstCheck = false;
            _lastActivationTime = Time.time;
        }

        /// <summary>
        /// Calculates the time elapsed since the last reset or activation of the cooldown.
        /// </summary>
        /// <returns>The elapsed time in seconds since the last activation.</returns>
        private float ElapsedTime() => Time.time - _lastActivationTime;

    }

}

using UnityEngine;

namespace UIManager
{
    /// <summary>
    /// Manages dynamically specified cooldown durations for actions or abilities.
    /// This allows different cooldowns to be managed using the same class instance.
    /// </summary>
    public class CooldownDynamic
    {
        /// <summary>
        /// Tracks the last time the cooldown was reset.
        /// </summary>
        private float _lastResetTime;

        /// <summary>
        /// Constructor for CooldownDynamic. It initializes and resets the cooldown timer upon object creation.
        /// </summary>
        public CooldownDynamic()
        {
            Reset();
        }

        /// <summary>
        /// Determines if the cooldown has expired based on the specified duration.
        /// If the cooldown is expired, it automatically resets the cooldown timer.
        /// </summary>
        /// <param name="cooldown">The cooldown duration in seconds.</param>
        /// <returns>True if the cooldown has expired, allowing the action to be performed; otherwise, false.</returns>
        public bool Ready(float cooldown)
        {
            // Check if the elapsed time since the last reset is greater than or equal to the specified cooldown duration.
            if (HasCompleted(cooldown))
            {
                Reset();  // Reset the cooldown timer if the condition is met.
                return true;  // Cooldown has expired.
            }
            return false;  // Cooldown has not yet expired.
        }

        /// <summary>
        /// Determines if the cooldown is still active.
        /// </summary>
        /// <param name="cooldown">The cooldown duration in seconds.</param>
        /// <returns>True if the cooldown is still active; otherwise, false.</returns>
        public bool NotReady(float cooldown) => !Ready(cooldown);

        /// <summary>
        /// Checks if the cooldown has completed without resetting the cooldown timer.
        /// </summary>
        /// <param name="cooldown">The cooldown duration in seconds.</param>
        /// <returns>True if the elapsed time since the last reset is greater than or equal to the cooldown duration; otherwise, false.</returns>
        public bool HasCompleted(float cooldown) => ElapsedTime() >= cooldown;

        /// <summary>
        /// Resets the cooldown timer by updating the last reset time to the current time.
        /// </summary>
        public void Reset()
        {
            _lastResetTime = Time.time;  // Set the last reset time to the current time.
        }

        /// <summary>
        /// Calculates the time elapsed since the last reset of the cooldown timer.
        /// </summary>
        /// <returns>The elapsed time in seconds since the last cooldown reset.</returns>
        private float ElapsedTime() => Time.time - _lastResetTime;
    }
}

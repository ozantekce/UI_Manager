using UnityEngine;

namespace UIManager
{
    /// <summary>
    /// The Cooldown class is designed to manage cooldown operations for actions or abilities.
    /// </summary>
    public class Cooldown
    {
        /// <summary>
        /// Private field to store the duration of the cooldown.
        /// </summary>
        private float _cooldownDuration;

        /// <summary>
        /// Private field to store the last time the cooldown was activated.
        /// </summary>
        private float _lastActivationTime;

        /// <summary>
        /// Constructor for the Cooldown class with a parameter to set the cooldown duration.
        /// Initializes the cooldown and resets its timer.
        /// </summary>
        /// <param name="cooldownDuration">The duration for which the cooldown will last.</param>
        public Cooldown(float cooldownDuration)
        {
            _cooldownDuration = cooldownDuration;  // Initialize the cooldown duration.
            Reset();  // Reset the cooldown timer upon creation.
        }

        /// <summary>
        /// Method to set or update the cooldown duration.
        /// </summary>
        /// <param name="cooldownDuration">The new duration to set for the cooldown.</param>
        public void SetCooldown(float cooldownDuration)
        {
            _cooldownDuration = cooldownDuration;
        }

        /// <summary>
        /// Method to check if the cooldown has completed and if the action is ready to be performed again.
        /// </summary>
        /// <returns>True if the cooldown is complete, otherwise false.</returns>
        public bool IsReady()
        {
            // Check if the current time minus the last activation time is greater than or equal to the cooldown duration.
            if (HasCompleted())
            {
                Reset();  // Reset the cooldown.
                return true;  // Return true indicating the cooldown is complete.
            }
            return false;  // Return false indicating the cooldown is not yet complete.
        }

        /// <summary>
        /// Method to check if the cooldown is still active (i.e., the action is not ready).
        /// </summary>
        /// <returns>True if the cooldown is not ready, otherwise false.</returns>
        public bool IsNotReady() => !IsReady();  // Returns the opposite of IsReady().

        /// <summary>
        /// Method to directly check if the cooldown period has completed without resetting the cooldown.
        /// </summary>
        /// <returns>True if the elapsed time since last activation is greater than or equal to the cooldown duration, otherwise false.</returns>
        public bool HasCompleted() => ElapsedTime >= _cooldownDuration;

        /// <summary>
        /// Method to reset the cooldown, setting the last activation time to the current time.
        /// </summary>
        public void Reset()
        {
            _lastActivationTime = Time.time;  // Update the last activation time to the current time.
        }

        /// <summary>
        /// Property to get the elapsed time since the last activation of the cooldown.
        /// </summary>
        /// <value>The time elapsed since the last activation time.</value>
        public float ElapsedTime => Time.time - _lastActivationTime;
    }
}

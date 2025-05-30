using UnityEngine;
using Seyren.Universe;

namespace Seyren.System.Knockup
{
    /// <summary>
    /// Implements a sinusoidal motion for knockup effects, creating a smooth rise and fall
    /// </summary>
    public class SinusoidalKnockupMotion : IKnockupMotion
    {
        public void Apply(ITime time, KnockupData data)
        {
            // Store previous height for delta calculation
            data.prevHeight = data.currentHeight;
            
            data.elapsed += time.DeltaTime;

            if (data.elapsed >= data.duration)
            {
                // Reset height and mark as inactive if knockup duration is complete
                data.currentHeight = 0;
                data.active = false;
                return;
            }

            // Calculate height using sine wave for smooth up and down motion
            // sin(0) = 0, sin(π/2) = 1, sin(π) = 0
            float t = data.elapsed / data.duration;
            float newHeight = data.maxHeight * Mathf.Sin(t * Mathf.PI);
            
            // Update current height
            data.currentHeight = newHeight;
            
            // Calculate height difference for smooth movement
            float heightDifference = data.currentHeight - data.prevHeight;
            
            // Apply the height change to the target
            Vector3 currentPosition = data.target.Location;
            data.target.Move(new Vector3(currentPosition.x, currentPosition.y + heightDifference, currentPosition.z));
        }
    }
}
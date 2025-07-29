using System;
using System.Collections.Generic;

namespace Seyren.Payment
{
    /// <summary>
    /// Interface for managing game resources
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// Get the current amount of a resource
        /// </summary>
        /// <param name="resourceId">The resource identifier</param>
        /// <returns>The current amount of the resource</returns>
        int GetResourceAmount(string resourceId);

        /// <summary>
        /// Check if there is enough of a resource available
        /// </summary>
        /// <param name="resourceId">The resource identifier</param>
        /// <param name="amount">The amount to check</param>
        /// <returns>True if there is enough of the resource, false otherwise</returns>
        bool HasResource(string resourceId, int amount);

        /// <summary>
        /// Add resources to the manager
        /// </summary>
        /// <param name="resourceId">The resource identifier</param>
        /// <param name="amount">The amount to add</param>
        void AddResource(string resourceId, int amount);

        /// <summary>
        /// Remove resources from the manager
        /// </summary>
        /// <param name="resourceId">The resource identifier</param>
        /// <param name="amount">The amount to remove</param>
        /// <returns>True if the resource was successfully removed, false if there wasn't enough</returns>
        bool RemoveResource(string resourceId, int amount);

        /// <summary>
        /// Event triggered when a resource amount changes
        /// </summary>
        event Action<string, int> OnResourceChanged;
    }

    /// <summary>
    /// Default implementation of the resource manager
    /// </summary>
    public class DefaultResourceManager : IResourceManager
    {
        private Dictionary<string, int> resources = new Dictionary<string, int>();
        public event Action<string, int> OnResourceChanged;

        public int GetResourceAmount(string resourceId)
        {
            if (resources.TryGetValue(resourceId, out int value))
            {
                return value;
            }
            return 0;
        }

        public bool HasResource(string resourceId, int amount)
        {
            return GetResourceAmount(resourceId) >= amount;
        }

        public void AddResource(string resourceId, int amount)
        {
            if (!resources.ContainsKey(resourceId))
            {
                resources[resourceId] = amount;
            }
            else
            {
                resources[resourceId] += amount;
            }

            OnResourceChanged?.Invoke(resourceId, resources[resourceId]);
        }

        public bool RemoveResource(string resourceId, int amount)
        {
            if (!HasResource(resourceId, amount))
            {
                return false;
            }

            resources[resourceId] -= amount;
            OnResourceChanged?.Invoke(resourceId, resources[resourceId]);
            return true;
        }
    }
}
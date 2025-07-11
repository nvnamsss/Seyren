using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Payment
{
    /// <summary>
    /// Interface for any object that has a cost associated with it
    /// </summary>
    public interface ICostProvider
    {
        /// <summary>
        /// Get the cost associated with this object
        /// </summary>
        /// <returns>The cost specification</returns>
        ICost GetCost();
    }

    /// <summary>
    /// Interface for cost specifications
    /// </summary>
    public interface ICost
    {
        /// <summary>
        /// Check if this cost can be satisfied with the given resource manager
        /// </summary>
        /// <param name="resourceManager">The resource manager to check against</param>
        /// <returns>True if the cost can be paid, false otherwise</returns>
        bool CanSatisfy(IResourceManager resourceManager);

        /// <summary>
        /// Apply the cost to the resource manager (deduct resources)
        /// </summary>
        /// <param name="resourceManager">The resource manager to apply the cost to</param>
        /// <returns>True if the cost was successfully applied, false otherwise</returns>
        bool Apply(IResourceManager resourceManager);

        /// <summary>
        /// Refund the cost to the resource manager (add resources back)
        /// </summary>
        /// <param name="resourceManager">The resource manager to refund to</param>
        void Refund(IResourceManager resourceManager);

        /// <summary>
        /// Get a human-readable description of this cost
        /// </summary>
        /// <returns>A formatted description of the cost</returns>
        string GetDescription();
    }

    /// <summary>
    /// Simple implementation of a cost consisting of a single resource
    /// </summary>
    [Serializable]
    public class SimpleCost : ICost
    {
        [SerializeField] private string resourceId;
        [SerializeField] private int amount;

        public string ResourceId => resourceId;
        public int Amount => amount;

        private Queue<string> transactionHistory = new Queue<string>();

        public SimpleCost(string resourceId, int amount)
        {
            this.resourceId = resourceId;
            this.amount = amount;
        }

        public bool CanSatisfy(IResourceManager resourceManager)
        {
            return resourceManager.HasResource(resourceId, amount);
        }

        public bool Apply(IResourceManager resourceManager)
        {
            // create a transaction record
            string transaction = $"{DateTime.Now}: Deducted {amount} of {resourceId}";
            transactionHistory.Enqueue(transaction);
            return resourceManager.RemoveResource(resourceId, amount);
        }

        public void Refund(IResourceManager resourceManager)
        {
            if (transactionHistory.Count == 0)
            {
                Debug.LogWarning("No transaction to refund.");
                return;
            }
            // revert the transaction
            string transaction = transactionHistory.Dequeue();
            Debug.Log($"Refunding transaction: {transaction}");
            resourceManager.AddResource(resourceId, amount);
        }

        public string GetDescription()
        {
            return $"{amount} {resourceId}";
        }
    }

}
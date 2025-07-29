namespace Seyren.Payment
{
    /// <summary>
    /// Interface for payment processors that handle resource transactions
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Process a payment of specified resources
        /// </summary>
        /// <param name="cost">The cost specification</param>
        /// <returns>True if the payment was successful, false otherwise</returns>
        bool ProcessPayment(ICost cost);

        /// <summary>
        /// Check if a payment can be made based on current resource availability
        /// </summary>
        /// <param name="cost">The cost specification</param>
        /// <returns>True if the payment can be processed, false otherwise</returns>
        bool CanProcessPayment(ICost cost);

        /// <summary>
        /// Refund a previously processed payment
        /// </summary>
        /// <param name="cost">The cost to refund</param>
        void RefundPayment(ICost cost);
    }

    /// <summary>
    /// Standard payment processor implementation
    /// </summary>
    public class StandardPaymentProcessor : IPaymentProcessor
    {
        private IResourceManager resourceManager;

        public StandardPaymentProcessor(IResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        public bool ProcessPayment(ICost cost)
        {
            return cost.Apply(resourceManager);
        }

        public bool CanProcessPayment(ICost cost)
        {
            return cost.CanSatisfy(resourceManager);
        }

        public void RefundPayment(ICost cost)
        {
            cost.Refund(resourceManager);
        }
    }

    /// <summary>
    /// Payment utility to simplify working with purchasable objects
    /// </summary>
    public static class PaymentUtility
    {
        /// <summary>
        /// Try to purchase an object with a cost
        /// </summary>
        /// <param name="costProvider">The object to purchase</param>
        /// <param name="paymentProcessor">The payment processor</param>
        /// <returns>True if the purchase was successful, false otherwise</returns>
        public static bool TryPurchase(ICostProvider costProvider, IPaymentProcessor paymentProcessor)
        {
            ICost cost = costProvider.GetCost();
            return paymentProcessor.ProcessPayment(cost);
        }

        /// <summary>
        /// Check if an object can be purchased
        /// </summary>
        /// <param name="costProvider">The object to check</param>
        /// <param name="paymentProcessor">The payment processor</param>
        /// <returns>True if the object can be purchased, false otherwise</returns>
        public static bool CanPurchase(ICostProvider costProvider, IPaymentProcessor paymentProcessor)
        {
            ICost cost = costProvider.GetCost();
            return paymentProcessor.CanProcessPayment(cost);
        }
    }
}
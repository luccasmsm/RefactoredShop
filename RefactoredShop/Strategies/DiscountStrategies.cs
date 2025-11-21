using RefactoredShop.Interfaces;

namespace RefactoredShop.Strategies
{
    public class NoDiscountStrategy : IDiscountStrategy
    {
        public double CalculateDiscount(double orderTotal) => 0;
    }

    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly double _percentage;
        public PercentageDiscountStrategy(double percentage)
        {
            _percentage = percentage;
        }

        public double CalculateDiscount(double orderTotal)
        {
            return orderTotal * (_percentage / 100);
        }
    }

    public class FixedValueDiscountStrategy : IDiscountStrategy
    {
        private readonly double _value;
        public FixedValueDiscountStrategy(double value)
        {
            _value = value;
        }

        public double CalculateDiscount(double orderTotal) => _value;
    }

    public static class DiscountFactory
    {
        public static IDiscountStrategy Create(double value, bool isPercentage)
        {
            if (value <= 0) return new NoDiscountStrategy();
            if (isPercentage) return new PercentageDiscountStrategy(value);
            return new FixedValueDiscountStrategy(value);
        }
    }
}
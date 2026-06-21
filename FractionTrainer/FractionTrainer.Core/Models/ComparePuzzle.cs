namespace FractionTrainer.Core.Models
{
    public class ComparePuzzle
    {
        public Fraction FractionA { get; }
        public Fraction FractionB { get; }
        public Fraction LargerFraction { get; }

        public ComparePuzzle(Fraction a, Fraction b)
        {
            FractionA = a;
            FractionB = b;
            LargerFraction = a.ToDouble() >= b.ToDouble() ? a : b;
        }

        public bool IsCorrect(Fraction answer)
        {
            return answer.IsEquivalentTo(LargerFraction);
        }
    }
}

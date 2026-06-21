namespace FractionTrainer.Core.Models
{
    public class CompleteToWholePuzzle
    {
        public Fraction GivenFraction { get; }
        public Fraction NeededFraction { get; }

        public CompleteToWholePuzzle(Fraction given)
        {
            GivenFraction = given;
            NeededFraction = new Fraction(
                given.Denominator - given.Numerator,
                given.Denominator);
        }

        public bool IsCorrect(int numeratorInput)
        {
            return numeratorInput == NeededFraction.Numerator;
        }
    }
}

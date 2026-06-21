using FractionTrainer.Core.Enums;

namespace FractionTrainer.Core.Models
{
    public class FindPairsPuzzle
    {
        public List<PairCard> Cards { get; }
        public FractionPair CorrectPair { get; }

        public FindPairsPuzzle(List<PairCard> cards, FractionPair correctPair)
        {
            Cards = cards;
            CorrectPair = correctPair;
        }

        public bool IsMatch(int indexA, int indexB)
        {
            if (indexA < 0 || indexA >= Cards.Count ||
                indexB < 0 || indexB >= Cards.Count || indexA == indexB)
                return false;

            return Cards[indexA].Fraction.IsEquivalentTo(Cards[indexB].Fraction);
        }
    }

    public class PairCard
    {
        public Fraction Fraction { get; }
        public ShapeType Shape { get; }
        public int TotalSectors { get; }
        public int HighlightedSectors { get; }

        public PairCard(Fraction fraction, ShapeType shape)
        {
            Fraction = fraction;
            Shape = shape;
            TotalSectors = fraction.Denominator;
            HighlightedSectors = fraction.Numerator;
        }
    }

    public class FractionPair
    {
        public Fraction FractionA { get; }
        public Fraction FractionB { get; }

        public FractionPair(Fraction a, Fraction b)
        {
            FractionA = a;
            FractionB = b;
        }
    }
}

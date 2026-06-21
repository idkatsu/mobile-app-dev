using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Interfaces;
using FractionTrainer.Core.Models;

namespace FractionTrainer.Core.Services
{
    public class PuzzleGenerator : IPuzzleGenerator
    {
        private readonly Random _random = new Random();

        private int[] GetDenominators(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Easy => new[] { 2, 3, 4 },
                DifficultyLevel.Medium => new[] { 2, 3, 4, 5, 6 },
                DifficultyLevel.Hard => new[] { 2, 3, 4, 5, 6, 8, 10 },
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
            };
        }

        private Fraction GenerateRandomFraction(DifficultyLevel difficulty)
        {
            int[] denominators = GetDenominators(difficulty);
            int denominator = denominators[_random.Next(denominators.Length)];
            int numerator = _random.Next(1, denominator);
            return new Fraction(numerator, denominator);
        }

        public FractionPuzzle Generate(DifficultyLevel difficulty)
        {
            return GenerateWithType(difficulty, PuzzleType.BuildFraction);
        }

        public FractionPuzzle GenerateWithType(DifficultyLevel difficulty, PuzzleType type)
        {
            Fraction fraction = GenerateRandomFraction(difficulty);
            ShapeType shape = (ShapeType)_random.Next(Enum.GetValues(typeof(ShapeType)).Length);
            var puzzle = new FractionPuzzle(fraction, type) { Shape = shape };
            return puzzle;
        }

        public ComparePuzzle GenerateCompare(DifficultyLevel difficulty)
        {
            Fraction a = GenerateRandomFraction(difficulty);
            Fraction b;
            do
            {
                b = GenerateRandomFraction(difficulty);
            }
            while (a.IsEquivalentTo(b));

            return new ComparePuzzle(a, b);
        }

        public CompleteToWholePuzzle GenerateCompleteToWhole(DifficultyLevel difficulty)
        {
            Fraction fraction = GenerateRandomFraction(difficulty);
            return new CompleteToWholePuzzle(fraction);
        }

        public EquivalentPuzzle GenerateEquivalent(DifficultyLevel difficulty)
        {
            int[] denominators = GetDenominators(difficulty);
            int targetDenom = denominators[_random.Next(denominators.Length)];
            int targetNum = _random.Next(1, targetDenom);

            Fraction target = new Fraction(targetNum, targetDenom);
            Fraction simplified = target.Simplify();

            List<Fraction> options = new List<Fraction>();
            options.Add(simplified);

            int attempts = 0;
            while (options.Count < 4 && attempts < 50)
            {
                attempts++;
                int optDenom = denominators[_random.Next(denominators.Length)];
                int optNum = _random.Next(1, optDenom);
                Fraction opt = new Fraction(optNum, optDenom);

                bool isDuplicate = options.Any(o => o.IsEquivalentTo(opt));
                bool isSame = opt.IsEquivalentTo(target);

                if (!isDuplicate && !isSame)
                {
                    options.Add(opt);
                }
            }

            while (options.Count < 4)
            {
                int fallbackDenom = denominators[_random.Next(denominators.Length)];
                int fallbackNum = _random.Next(1, fallbackDenom);
                Fraction fallback = new Fraction(fallbackNum, fallbackDenom);
                if (!options.Any(o => o.IsEquivalentTo(fallback)))
                {
                    options.Add(fallback);
                }
            }

            options = options.OrderBy(_ => _random.Next()).ToList();

            return new EquivalentPuzzle(target, simplified, options);
        }

        public FindPairsPuzzle GenerateFindPairs(DifficultyLevel difficulty)
        {
            int[] denominators = GetDenominators(difficulty);
            ShapeType[] shapes = Enum.GetValues<ShapeType>();

            Fraction baseFraction = GenerateRandomFraction(difficulty);
            Fraction equivalent = FindEquivalentFraction(baseFraction, denominators);

            List<PairCard> cards = new List<PairCard>();

            ShapeType shapeA = shapes[_random.Next(shapes.Length)];
            ShapeType shapeB;
            do
            {
                shapeB = shapes[_random.Next(shapes.Length)];
            }
            while (shapeB == shapeA);

            cards.Add(new PairCard(baseFraction, shapeA));
            cards.Add(new PairCard(equivalent, shapeB));

            int distractorsNeeded = 4;
            int attempts = 0;
            while (cards.Count < 2 + distractorsNeeded && attempts < 100)
            {
                attempts++;
                Fraction distractor = GenerateRandomFraction(difficulty);

                bool isDuplicate = cards.Any(c => c.Fraction.IsEquivalentTo(distractor));
                if (!isDuplicate)
                {
                    ShapeType distractorShape = shapes[_random.Next(shapes.Length)];
                    cards.Add(new PairCard(distractor, distractorShape));
                }
            }

            cards = cards.OrderBy(_ => _random.Next()).ToList();

            int indexA = cards.FindIndex(c => c.Fraction.IsEquivalentTo(baseFraction));
            int indexB = cards.FindIndex(c => c.Fraction.IsEquivalentTo(equivalent) && c != cards[indexA]);

            return new FindPairsPuzzle(cards, new FractionPair(baseFraction, equivalent));
        }

        private Fraction FindEquivalentFraction(Fraction original, int[] denominators)
        {
            for (int attempt = 0; attempt < 50; attempt++)
            {
                int mult = _random.Next(2, 4);
                int newNum = original.Numerator * mult;
                int newDen = original.Denominator * mult;

                if (denominators.Contains(newDen) && newNum < newDen)
                {
                    return new Fraction(newNum, newDen);
                }
            }

            int[] safeDenoms = denominators.Where(d => d > original.Denominator).ToArray();
            if (safeDenoms.Length > 0)
            {
                int newDen = safeDenoms[_random.Next(safeDenoms.Length)];
                int newNum = (int)Math.Round(original.ToDouble() * newDen);
                if (newNum > 0 && newNum < newDen)
                {
                    return new Fraction(newNum, newDen);
                }
            }

            return new Fraction(original.Numerator * 2, original.Denominator * 2);
        }
    }
}

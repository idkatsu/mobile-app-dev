using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Models;
using FractionTrainer.Core.Services;
using Xunit;

namespace FractionTrainer.Tests
{
    public class PuzzleGeneratorTests
    {
        private readonly PuzzleGenerator _generator = new PuzzleGenerator();

        [Fact]
        public void Generate_Easy_GeneratesDenominatorsInRange2To4()
        {
            int[] expected = { 2, 3, 4 };
            for (int i = 0; i < 100; i++)
            {
                FractionPuzzle puzzle = _generator.Generate(DifficultyLevel.Easy);
                Assert.Contains(puzzle.TargetFraction.Denominator, expected);
            }
        }

        [Fact]
        public void Generate_Medium_GeneratesDenominatorsInRange2To6()
        {
            int[] expected = { 2, 3, 4, 5, 6 };
            for (int i = 0; i < 100; i++)
            {
                FractionPuzzle puzzle = _generator.Generate(DifficultyLevel.Medium);
                Assert.Contains(puzzle.TargetFraction.Denominator, expected);
            }
        }

        [Fact]
        public void Generate_Hard_GeneratesDenominatorsInRange2To10()
        {
            int[] expected = { 2, 3, 4, 5, 6, 8, 10 };
            for (int i = 0; i < 100; i++)
            {
                FractionPuzzle puzzle = _generator.Generate(DifficultyLevel.Hard);
                Assert.Contains(puzzle.TargetFraction.Denominator, expected);
            }
        }

        [Fact]
        public void Generate_AlwaysGeneratesProperFraction()
        {
            DifficultyLevel[] levels = { DifficultyLevel.Easy, DifficultyLevel.Medium, DifficultyLevel.Hard };

            foreach (DifficultyLevel level in levels)
            {
                for (int i = 0; i < 100; i++)
                {
                    FractionPuzzle puzzle = _generator.Generate(level);
                    Assert.True(
                        puzzle.TargetFraction.Numerator < puzzle.TargetFraction.Denominator,
                        $"Числитель ({puzzle.TargetFraction.Numerator}) должен быть меньше " +
                        $"знаменателя ({puzzle.TargetFraction.Denominator}) на уровне {level}.");
                }
            }
        }

        [Fact]
        public void GenerateCompare_AlwaysGeneratesDifferentFractions()
        {
            for (int i = 0; i < 100; i++)
            {
                ComparePuzzle puzzle = _generator.GenerateCompare(DifficultyLevel.Easy);
                Assert.False(puzzle.FractionA.IsEquivalentTo(puzzle.FractionB));
            }
        }

        [Fact]
        public void GenerateCompleteToWhole_CorrectAnswer()
        {
            for (int i = 0; i < 100; i++)
            {
                CompleteToWholePuzzle puzzle = _generator.GenerateCompleteToWhole(DifficultyLevel.Easy);
                int needed = puzzle.GivenFraction.Denominator - puzzle.GivenFraction.Numerator;
                Assert.Equal(needed, puzzle.NeededFraction.Numerator);
            }
        }

        [Fact]
        public void GenerateEquivalent_HasCorrectAnswer()
        {
            for (int i = 0; i < 100; i++)
            {
                EquivalentPuzzle puzzle = _generator.GenerateEquivalent(DifficultyLevel.Easy);
                Assert.True(puzzle.CorrectAnswer.IsEquivalentTo(puzzle.TargetFraction));
                Assert.Equal(4, puzzle.Options.Count);
            }
        }
    }
}

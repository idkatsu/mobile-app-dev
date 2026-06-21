using FractionTrainer.Core.Models;
using Xunit;

namespace FractionTrainer.Tests
{
    /// <summary>
    /// Тесты для класса FractionPuzzle.
    /// </summary>
    public class FractionPuzzleTests
    {
        /// <summary>
        /// Проверяет, что ToggleSector добавляет индекс, если его нет.
        /// </summary>
        [Fact]
        public void ToggleSector_IndexNotPresent_AddsIndex()
        {
            Fraction target = new Fraction(2, 4);
            FractionPuzzle puzzle = new FractionPuzzle(target);

            puzzle.ToggleSector(0);

            Assert.Contains(0, puzzle.SelectedSectors);
        }

        /// <summary>
        /// Проверяет, что ToggleSector убирает индекс, если он уже есть.
        /// </summary>
        [Fact]
        public void ToggleSector_IndexPresent_RemovesIndex()
        {
            Fraction target = new Fraction(2, 4);
            FractionPuzzle puzzle = new FractionPuzzle(target);

            puzzle.ToggleSector(0);
            puzzle.ToggleSector(0);

            Assert.DoesNotContain(0, puzzle.SelectedSectors);
        }

        /// <summary>
        /// Проверяет, что IsSolved == true при правильном количестве секторов.
        /// </summary>
        [Fact]
        public void IsSolved_CorrectSelection_ReturnsTrue()
        {
            Fraction target = new Fraction(2, 4);
            FractionPuzzle puzzle = new FractionPuzzle(target);

            puzzle.ToggleSector(0);
            puzzle.ToggleSector(1);

            Assert.True(puzzle.IsSolved);
        }

        /// <summary>
        /// Проверяет, что IsSolved == false при неправильном количестве секторов.
        /// </summary>
        [Fact]
        public void IsSolved_IncorrectSelection_ReturnsFalse()
        {
            Fraction target = new Fraction(2, 4);
            FractionPuzzle puzzle = new FractionPuzzle(target);

            puzzle.ToggleSector(0);

            Assert.False(puzzle.IsSolved);
        }

        /// <summary>
        /// Проверяет, что Reset() очищает SelectedSectors.
        /// </summary>
        [Fact]
        public void Reset_ClearsSelectedSectors()
        {
            Fraction target = new Fraction(2, 4);
            FractionPuzzle puzzle = new FractionPuzzle(target);

            puzzle.ToggleSector(0);
            puzzle.ToggleSector(1);
            puzzle.Reset();

            Assert.Empty(puzzle.SelectedSectors);
        }

        /// <summary>
        /// Проверяет, что GetCurrentFraction() возвращает корректную дробь.
        /// </summary>
        [Fact]
        public void GetCurrentFraction_ReturnsCorrectFraction()
        {
            Fraction target = new Fraction(2, 4);
            FractionPuzzle puzzle = new FractionPuzzle(target);

            puzzle.ToggleSector(0);
            puzzle.ToggleSector(1);

            Fraction current = puzzle.GetCurrentFraction();

            Assert.Equal(2, current.Numerator);
            Assert.Equal(4, current.Denominator);
        }
    }
}

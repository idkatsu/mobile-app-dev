using FractionTrainer.Core.Models;
using Xunit;

namespace FractionTrainer.Tests
{
    /// <summary>
    /// Тесты для класса Fraction.
    /// </summary>
    public class FractionTests
    {
        /// <summary>
        /// Проверяет форматирование дроби в строку.
        /// </summary>
        [Fact]
        public void ToString_ReturnsCorrectFormat()
        {
            Fraction fraction = new Fraction(3, 4);
            Assert.Equal("3/4", fraction.ToString());
        }

        /// <summary>
        /// Проверяет эквивалентность дробей через перекрёстное умножение.
        /// </summary>
        [Fact]
        public void IsEquivalentTo_SameValue_ReturnsTrue()
        {
            Fraction fraction1 = new Fraction(1, 2);
            Fraction fraction2 = new Fraction(2, 4);

            Assert.True(fraction1.IsEquivalentTo(fraction2));
        }

        /// <summary>
        /// Проверяет сокращение дроби 2/4 до 1/2.
        /// </summary>
        [Fact]
        public void Simplify_TwoFourths_ReturnsOneHalf()
        {
            Fraction fraction = new Fraction(2, 4);
            Fraction simplified = fraction.Simplify();

            Assert.Equal(1, simplified.Numerator);
            Assert.Equal(2, simplified.Denominator);
        }

        /// <summary>
        /// Проверяет сокращение дроби 6/9 до 2/3.
        /// </summary>
        [Fact]
        public void Simplify_SixNinths_ReturnsTwoThirds()
        {
            Fraction fraction = new Fraction(6, 9);
            Fraction simplified = fraction.Simplify();

            Assert.Equal(2, simplified.Numerator);
            Assert.Equal(3, simplified.Denominator);
        }

        /// <summary>
        /// Проверяет, что дробь 1/1 остаётся 1/1 после сокращения.
        /// </summary>
        [Fact]
        public void Simplify_OneOne_ReturnsOneOne()
        {
            Fraction fraction = new Fraction(1, 1);
            Fraction simplified = fraction.Simplify();

            Assert.Equal(1, simplified.Numerator);
            Assert.Equal(1, simplified.Denominator);
        }

        /// <summary>
        /// Проверяет, что при знаменателе 0 выбрасывается ArgumentException.
        /// </summary>
        [Fact]
        public void Constructor_ZeroDenominator_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Fraction(1, 0));
        }

        /// <summary>
        /// Проверяет преобразование дроби в вещественное число.
        /// </summary>
        [Fact]
        public void ToDouble_ReturnsCorrectValue()
        {
            Fraction fraction = new Fraction(1, 4);
            Assert.Equal(0.25, fraction.ToDouble(), 5);
        }
    }
}

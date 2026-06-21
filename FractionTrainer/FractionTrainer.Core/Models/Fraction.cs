namespace FractionTrainer.Core.Models
{
    /// <summary>
    /// Математическая дробь с целочисленными числителем и знаменателем.
    /// </summary>
    public class Fraction : IEquatable<Fraction>
    {
        /// <summary>Числитель дроби.</summary>
        public int Numerator { get; }

        /// <summary>Знаменатель дроби (не может быть нулём).</summary>
        public int Denominator { get; }

        /// <summary>
        /// Создаёт дробь с заданными числителем и знаменателем.
        /// </summary>
        /// <param name="numerator">Числитель.</param>
        /// <param name="denominator">Знаменатель (не должен быть 0).</param>
        /// <exception cref="ArgumentException">Выбрасывается, если знаменатель равен 0.</exception>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Знаменатель не может быть равен нулю.", nameof(denominator));
            }

            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Проверяет эквивалентность двух дробей перекрёстным умножением.
        /// </summary>
        /// <param name="other">Другая дробь для сравнения.</param>
        /// <returns>true, если дроби эквивалентны; иначе false.</returns>
        public bool IsEquivalentTo(Fraction other)
        {
            if (other is null)
            {
                return false;
            }

            return Numerator * other.Denominator == Denominator * other.Numerator;
        }

        /// <summary>
        /// Возвращает сокращённую версию дроби.
        /// </summary>
        /// <returns>Новая дробь, сокращённая до наименьших значений.</returns>
        public Fraction Simplify()
        {
            int greatestCommonDivisor = CalculateGreatestCommonDivisor(
                Math.Abs(Numerator),
                Math.Abs(Denominator));

            int simplifiedNumerator = Numerator / greatestCommonDivisor;
            int simplifiedDenominator = Denominator / greatestCommonDivisor;

            if (simplifiedDenominator < 0)
            {
                simplifiedNumerator = -simplifiedNumerator;
                simplifiedDenominator = -simplifiedDenominator;
            }

            return new Fraction(simplifiedNumerator, simplifiedDenominator);
        }

        /// <summary>
        /// Преобразует дробь в вещественное число.
        /// </summary>
        /// <returns>Вещественное значение дроби.</returns>
        public double ToDouble()
        {
            return (double)Numerator / Denominator;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as Fraction);
        }

        /// <summary>
        /// Проверяет равенство двух дробей (с учётом сокращения).
        /// </summary>
        /// <param name="other">Другая дробь.</param>
        /// <returns>true, если дроби равны; иначе false.</returns>
        public bool Equals(Fraction? other)
        {
            if (other is null)
            {
                return false;
            }

            return IsEquivalentTo(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            Fraction simplified = Simplify();
            return HashCode.Combine(simplified.Numerator, simplified.Denominator);
        }

        /// <summary>
        /// Вычисляет наибольший общий делитель двух чисел (алгоритм Евклида).
        /// </summary>
        private static int CalculateGreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }
}

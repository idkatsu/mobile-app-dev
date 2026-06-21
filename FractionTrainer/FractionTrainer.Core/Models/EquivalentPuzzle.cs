namespace FractionTrainer.Core.Models
{
    public class EquivalentPuzzle
    {
        public Fraction TargetFraction { get; }
        public Fraction CorrectAnswer { get; }
        public List<Fraction> Options { get; }

        public EquivalentPuzzle(Fraction target, Fraction correct, List<Fraction> options)
        {
            TargetFraction = target;
            CorrectAnswer = correct;
            Options = options;
        }

        public bool IsCorrect(Fraction answer)
        {
            return answer.IsEquivalentTo(CorrectAnswer);
        }
    }
}

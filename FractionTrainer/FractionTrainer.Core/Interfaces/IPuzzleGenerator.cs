using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Models;

namespace FractionTrainer.Core.Interfaces
{
    public interface IPuzzleGenerator
    {
        FractionPuzzle Generate(DifficultyLevel difficulty);
        FractionPuzzle GenerateWithType(DifficultyLevel difficulty, PuzzleType type);
        ComparePuzzle GenerateCompare(DifficultyLevel difficulty);
        CompleteToWholePuzzle GenerateCompleteToWhole(DifficultyLevel difficulty);
        EquivalentPuzzle GenerateEquivalent(DifficultyLevel difficulty);
        FindPairsPuzzle GenerateFindPairs(DifficultyLevel difficulty);
    }
}

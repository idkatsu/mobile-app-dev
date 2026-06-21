using FractionTrainer.Core.Enums;
using FractionTrainer.Core.Events;

namespace FractionTrainer.Core.Models
{
    public class FractionPuzzle
    {
        private readonly HashSet<int> _selectedSectors = new HashSet<int>();

        public Fraction TargetFraction { get; }
        public int TotalSectors { get; }
        public PuzzleType PuzzleType { get; }
        public ShapeType Shape { get; set; } = ShapeType.Circle;

        public HashSet<int> SelectedSectors
        {
            get { return new HashSet<int>(_selectedSectors); }
        }

        public bool IsSolved
        {
            get
            {
                return _selectedSectors.Count == TargetFraction.Numerator
                    && TotalSectors == TargetFraction.Denominator;
            }
        }

        public event EventHandler<SectorToggledEventArgs>? SectorToggled;
        public event EventHandler<PuzzleSolvedEventArgs>? PuzzleSolved;

        public FractionPuzzle(Fraction targetFraction, PuzzleType puzzleType = PuzzleType.BuildFraction)
        {
            TargetFraction = targetFraction ?? throw new ArgumentNullException(nameof(targetFraction));
            TotalSectors = targetFraction.Denominator;
            PuzzleType = puzzleType;
        }

        public void ToggleSector(int sectorIndex)
        {
            if (sectorIndex < 0 || sectorIndex >= TotalSectors)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(sectorIndex),
                    $"Индекс сектора должен быть в диапазоне 0..{TotalSectors - 1}.");
            }

            bool isSelected;

            if (_selectedSectors.Contains(sectorIndex))
            {
                _selectedSectors.Remove(sectorIndex);
                isSelected = false;
            }
            else
            {
                _selectedSectors.Add(sectorIndex);
                isSelected = true;
            }

            SectorToggled?.Invoke(this, new SectorToggledEventArgs(sectorIndex, isSelected));

            if (IsSolved)
            {
                PuzzleSolved?.Invoke(this, new PuzzleSolvedEventArgs(this, TimeSpan.Zero));
            }
        }

        public void Reset()
        {
            _selectedSectors.Clear();
        }

        public Fraction GetCurrentFraction()
        {
            return new Fraction(_selectedSectors.Count, TotalSectors);
        }
    }
}

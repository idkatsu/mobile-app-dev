namespace FractionTrainer.Core.Events
{
    /// <summary>
    /// Аргументы события переключения сектора круга.
    /// </summary>
    public class SectorToggledEventArgs : EventArgs
    {
        /// <summary>Индекс переключённого сектора.</summary>
        public int SectorIndex { get; }

        /// <summary>Выбран ли сектор после переключения.</summary>
        public bool IsSelected { get; }

        /// <summary>
        /// Создаёт экземпляр SectorToggledEventArgs.
        /// </summary>
        /// <param name="sectorIndex">Индекс сектора.</param>
        /// <param name="isSelected">Состояние выбора.</param>
        public SectorToggledEventArgs(int sectorIndex, bool isSelected)
        {
            SectorIndex = sectorIndex;
            IsSelected = isSelected;
        }
    }
}

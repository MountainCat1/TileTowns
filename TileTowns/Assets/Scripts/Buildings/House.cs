using UnityEngine;
using Zenject;

namespace Buildings
{
    public class House : Building
    {
        [field: SerializeField] private int HousingProvided { get; set; }
        
        public override int WorkSlots => 0;
        public override int Housing => HousingProvided;
        public override int ImmigrationChange => Housing / 2;

        [TextArea]
        [field: SerializeField] private string description =
            $@"House provides some immigration and $1 housing.";

        public override string GetDescription()
        {
            return description.Replace("$1", HousingProvided.ToString());
        }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.ImmigrationChange = ImmigrationChange;
        }
    }
}
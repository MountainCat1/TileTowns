namespace Buildings
{
    public class Farm : Building
    {
        public override int WorkSlots => 3;
        public override int Housing => 2;

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = CalculateIncome();
            mutation.ImmigrationChange = 5 * tileData.WorkersAssigned;
        }

        private float CalculateIncome()
        {
            // TODO add farm income logic here pls 🐲
            return 5f;
        }
    }
}
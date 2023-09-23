namespace Buildings.Extensions
{
    public static class BuildingExtensions
    {
        public static TooltipData GetTooltipData(this Building building)
        {
            return new TooltipData()
            {
                Title = building.name,
                Content = building.GetDescription()
            };
        }
    }
}
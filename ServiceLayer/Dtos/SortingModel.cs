using static Shared.Enums.CommonEnum;

namespace ServiceLayer.Dtos
{
    public class SortingModel
    {
        public string SortingExpression { get; set; }
        public SortDirectionEnum SortingDirection { get; set; }
    }
}

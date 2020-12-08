namespace ServiceLayer.Dtos
{
    public class PagingModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SortingModel SortingModel { get; set; }
    }
}

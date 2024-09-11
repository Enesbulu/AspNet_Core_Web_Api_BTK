namespace Entities.DataTransferObjects
{
    //public record BookDtoForUpdate
    //{
    //    public int Id { get; init; }
    //    public string Title { get; set; }
    //    public decimal Price { get; set; }
    //}
    public record BookDtoForUpdate(int Id, String Title, decimal Price);
}

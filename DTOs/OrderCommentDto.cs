namespace WorkshopManager.DTOs
{
    public class OrderCommentDto
    {
        public int Id { get; set; }
        public int ServiceOrderId { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
    }
}

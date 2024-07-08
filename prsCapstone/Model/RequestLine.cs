namespace prsCapstone.Model
{
    public class RequestLine
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        public virtual Request? Requests { get; set; }
        public virtual Product? Products { get; set; }
    }
}

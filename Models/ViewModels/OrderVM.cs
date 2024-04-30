namespace BooksSpring2024_sec02.Models.ViewModels
{
    public class OrderVM
    {

        public Order Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }



    }
}

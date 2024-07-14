namespace CUAHANG_TAPHOA.Models.ViewModel
{
	public class CartItemViewModel
	{
		public List<CartModel> CartItems { get; set; }
		// Danh sách tên CartItems nhưng sử dụng CartModel
		public decimal GrandTotal { get; set; }
	}
}

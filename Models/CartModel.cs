using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace CUAHANG_TAPHOA.Models
{
	public class CartModel
	{
		public string Image { get; set; }
		public long ProductId { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public decimal Total 
			
		{
			
			get {  return Quantity * Price; }
		}
		public CartModel()
		{

		}
		public CartModel(ProductModel product)
		{
			ProductId = product.Id;
			ProductName = product.Name;
			//Quantity = Quantity += 1;
			Price = product.Price;
			Image = product.Image;
		}

	}
}

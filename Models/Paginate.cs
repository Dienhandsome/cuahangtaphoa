namespace CUAHANG_TAPHOA.Models
{
	public class Paginate
	{
		public int TotalItems { get; private set; }
		public int PageSize { get; private set; }
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int StartPage { get; private set; }
		public int EndPage { get; private set; }
		public Paginate() { }

		public Paginate(int totalItems, int page, int pageSize = 10) // set 10 items trên trang
		{
			// làm tròn tổng items/10 items trên 1 trang vd: 14 item/10 = làm tròn thì 3 trang => 2 trang 10 trang cuối 4 items
			int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

			int currentPage = page; // trang hiện tại = 1


			int startPage = currentPage;
			int endPage = currentPage + 4;
			if (startPage <= 0)
			{
				endPage = endPage - (startPage - 1);
				startPage = 1;
			}
			if (endPage > totalPages)
			{
				endPage = totalPages;
				if (endPage > 10)
				{
					startPage = endPage - 9;
				}
			}
			TotalItems = totalItems;
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalPages = totalPages;
			StartPage = startPage;
			EndPage = endPage;
		}
	}
}

namespace AspWebApiGlebTest.Models.Domain
{
	public class PageInfo
	{
		public int TotalCount { get; set; }
		public int ItemsPerPage { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }

		public bool HasPreviousPage { get; set; }
		public bool HasNextPage { get; set; }

		public PageInfo(int totalCount, int itemsPerPage, int currentPage)
		{
			TotalCount = totalCount;
			ItemsPerPage = itemsPerPage;
			CurrentPage = currentPage;
			TotalPages = (int)Math.Ceiling(TotalCount / (double)ItemsPerPage);

			HasPreviousPage = CurrentPage > 1;
			HasNextPage = CurrentPage < TotalPages;
		}
	}
}

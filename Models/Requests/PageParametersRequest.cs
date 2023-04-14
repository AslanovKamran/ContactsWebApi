using System.ComponentModel.DataAnnotations;

namespace AspWebApiGlebTest.Models.Requests
{
	public class PageParametersRequest
	{
		[Required]
		[Range(1,50,ErrorMessage ="Items per page may vary from 0 to 50")] 
		public int ItemsPerPage { get; set; }

		[Required]
		[Range(1, Int32.MaxValue, ErrorMessage ="Current Page must be greater than 0")]
		public int CurrentPage { get; set; }
	}
}

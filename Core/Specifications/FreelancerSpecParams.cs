

namespace Core.Specifications
{
    public class FreelancerSpecParams
    {
        // Search by username or email
        public string? Search { get; set; }

        // Filter by skill name
        public string? Skill { get; set; }

        // Filter by hobby name
        public string? Hobby { get; set; }



        // Pagination
        //private const int MaxPageSize = 50;
        //public int PageIndex { get; set; } = 1;

        //private int _pageSize = 10;
        //public int PageSize
        //{
        //    get => _pageSize;
        //    set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        //}
    }
}



namespace Vezeta.VezetaApi.Helpers
{
    public class Pagination<T>
    {         
        public  int PageNo { get; set; }
        public  int PageSize { get; set; }
        public  int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageNumber, int pageSize,int count, IReadOnlyList<T> data)
        {
            PageNo = pageNumber;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }
    }
}

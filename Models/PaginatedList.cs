namespace api.Models
{
    public class PaginatedList<T>: List<T>
    {
        public int _PageIndex{set;get;}
        public int _TotalPages{set;get;}
        public PaginatedList(List<T>items,int count,int pageIndex,int pageSize)
        {
            _PageIndex = pageIndex;
            _TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            AddRange(items);
        }
        public static PaginatedList<T> Create(IQueryable<T> source,int pageIndex,int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items,count,pageIndex,pageSize);
        }

    }
}
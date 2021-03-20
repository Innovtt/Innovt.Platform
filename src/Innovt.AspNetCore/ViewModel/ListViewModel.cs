namespace Innovt.AspNetCore.ViewModel
{
    public abstract class ListViewModel<T> : ViewModelBase where T : class
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public string Term { get; set; }

        protected ListViewModel()
        {
        }

        protected ListViewModel(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
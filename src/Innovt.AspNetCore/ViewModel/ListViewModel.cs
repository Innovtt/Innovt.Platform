// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.AspNetCore.ViewModel
{
    public abstract class ListViewModel<T> : ViewModelBase where T : class
    {
        protected ListViewModel()
        {
        }

        protected ListViewModel(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public string Term { get; set; }
    }
}
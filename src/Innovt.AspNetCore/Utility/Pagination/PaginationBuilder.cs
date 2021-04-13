// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-04-08
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Core.Collections;

namespace Innovt.AspNetCore.Utility.Pagination
{
    public class PaginationBuilder<T> where T : class
    {
        private readonly string formId;

        public PaginationBuilder(PagedCollection<T> collection, string formId)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
            this.formId = formId;
        }

        public PagedCollection<T> Collection { get; set; }


        public virtual string BuildHeader()
        {
            return @"<div class=""portlet-body text-center""><ul class=""pagination pagination-large"">";
        }

        public virtual string BuildPagerScript()
        {
            return @"<script>var Pager=function(){return{goToPreviousPage:function(formId){var form=$('" + formId +
                   "');" +
                   "var pageHidden=form.find('#Page');var pageVal=parseInt(pageHidden.val());" +
                   "pageVal-=1;if(pageVal<0) pageVal=0;pageHidden.val(pageVal);form.submit()},goToPage:function(index,formId){$('#Page').val(index);$('" +
                   formId + "').submit()},goToNextPage:function(formId){var form=$('" + formId +
                   "');var pageHidden=form.find('#Page');var pageVal=parseInt(pageHidden.val());pageVal+=1;pageHidden.val(pageVal);form.submit()}}}();</script>";
        }

        public virtual string BuildFooter()
        {
            return "</ul></div>";
        }

        public virtual string BuildPrevious(string previousText = "Anterior")
        {
            return
                $@"<li><a title=""{previousText}"" href=""javascript:Pager.goToPreviousPage('{formId}');"">«</a></li>";
        }


        public virtual string BuildNext(string nextText = "Próximo")
        {
            return
                $@"<li class=""next""><a title=""{nextText}"" href=""javascript:Pager.goToNextPage('{formId}');"">»</a></li>";
        }

        public virtual string BuildItem(int page, bool isCurrent)
        {
            return
                $@"<li {(isCurrent ? "class=\"active\"" : "")}><a href=""javascript:Pager.goToPage({page},'{formId}')"">{page + 1}</a></li>";
        }
    }
}
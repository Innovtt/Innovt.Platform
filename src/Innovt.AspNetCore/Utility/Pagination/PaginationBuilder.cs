// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using Innovt.Core.Collections;

namespace Innovt.AspNetCore.Utility.Pagination;

/// <summary>
///     Builder class for generating pagination HTML elements.
/// </summary>
/// <typeparam name="T">Type of items in the collection.</typeparam>
public class PaginationBuilder<T> where T : class
{
    private readonly string formId;

    /// <summary>
    ///     Initializes a new instance of the PaginationBuilder class.
    /// </summary>
    /// <param name="collection">The paged collection of items.</param>
    /// <param name="formId">The ID of the HTML form associated with the pagination.</param>
    public PaginationBuilder(PagedCollection<T> collection, string formId)
    {
        Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        this.formId = formId;
    }

    /// <summary>
    ///     Gets or sets the paged collection of items.
    /// </summary>
    public PagedCollection<T> Collection { get; set; }

    /// <summary>
    ///     Builds the header HTML for pagination.
    /// </summary>
    /// <returns>The HTML for the pagination header.</returns>
    public virtual string BuildHeader()
    {
        return @"<div class=""portlet-body text-center""><ul class=""pagination pagination-large"">";
    }

    /// <summary>
    ///     Builds the pager script HTML for pagination functionality.
    /// </summary>
    /// <returns>The HTML for the pagination script.</returns>
    public virtual string BuildPagerScript()
    {
        return @"<script>var Pager=function(){return{goToPreviousPage:function(formId){var form=$('" + formId +
               "');" +
               "var pageHidden=form.find('#Page');var pageVal=parseInt(pageHidden.val());" +
               "pageVal-=1;if(pageVal<0) pageVal=0;pageHidden.val(pageVal);form.submit()},goToPage:function(index,formId){$('#Page').val(index);$('" +
               formId + "').submit()},goToNextPage:function(formId){var form=$('" + formId +
               "');var pageHidden=form.find('#Page');var pageVal=parseInt(pageHidden.val());pageVal+=1;pageHidden.val(pageVal);form.submit()}}}();</script>";
    }

    /// <summary>
    ///     Builds the footer HTML for pagination.
    /// </summary>
    /// <returns>The HTML for the pagination footer.</returns>
    public virtual string BuildFooter()
    {
        return "</ul></div>";
    }

    /// <summary>
    ///     Builds the HTML for the "Previous" pagination button.
    /// </summary>
    /// <param name="previousText">The text to display for the "Previous" button (default is "Anterior").</param>
    /// <returns>The HTML for the "Previous" button.</returns>
    public virtual string BuildPrevious(string previousText = "Anterior")
    {
        return
            $@"<li><a title=""{previousText}"" href=""javascript:Pager.goToPreviousPage('{formId}');"">«</a></li>";
    }

    /// <summary>
    ///     Builds the HTML for the "Next" pagination button.
    /// </summary>
    /// <param name="nextText">The text to display for the "Next" button (default is "Próximo").</param>
    /// <returns>The HTML for the "Next" button.</returns>
    public virtual string BuildNext(string nextText = "Próximo")
    {
        return
            $@"<li class=""next""><a title=""{nextText}"" href=""javascript:Pager.goToNextPage('{formId}');"">»</a></li>";
    }

    /// <summary>
    ///     Builds the HTML for a pagination item.
    /// </summary>
    /// <param name="page">The page number associated with the item.</param>
    /// <param name="isCurrent">Indicates whether the item represents the current page.</param>
    /// <returns>The HTML for the pagination item.</returns>
    public virtual string BuildItem(int page, bool isCurrent)
    {
        return
            $@"<li {(isCurrent ? "class=\"active\"" : "")}><a href=""javascript:Pager.goToPage({page},'{formId}')"">{page + 1}</a></li>";
    }
}
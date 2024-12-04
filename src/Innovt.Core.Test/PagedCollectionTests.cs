using Innovt.Core.Collections;
using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class PagedCollectionTests
{
    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2", "Item3" };
        var page = "1";
        var pageSize = 2;

        // Act
        var pagedCollection = new PagedCollection<string>(items, page, pageSize) { TotalRecords = 3 };

        // Assert
        Assert.That(pagedCollection.Items, Is.EqualTo(items));
        Assert.That(pagedCollection.Page, Is.EqualTo(page));
        Assert.That(pagedCollection.PageSize, Is.EqualTo(pageSize));
        Assert.That(pagedCollection.TotalRecords, Is.EqualTo(3));
    }

    [Test]
    public void PageCount_ShouldReturnCorrectNumberOfPages()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2", "Item3" };

        var pageSize = 8;

        var pagedCollection = new PagedCollection<string>(items, "1", pageSize)
        {
            TotalRecords = 13
        };

        // Act
        var pageCount = pagedCollection.PageCount;

        // Assert
        Assert.That(pageCount, Is.EqualTo(2));
    }

    [Test]
    public void PageCount_ShouldReturnZero_WhenNotItems()
    {
        // Arrange
        var pagedCollection = new PagedCollection<string>(new List<string>(), "1", 1)
        {
            TotalRecords = 0
        };

        // Act
        var pageCount = pagedCollection.PageCount;

        // Assert
        Assert.That(pageCount, Is.EqualTo(0));
    }

    [Test]
    public void HasNext_ShouldReturnTrue_WhenThereAreMorePages()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };

        var pageSize = 2;

        var pagedCollection = new PagedCollection<string>(items, "0", pageSize)
        {
            TotalRecords = 5
        };

        // Act
        var hasNext = pagedCollection.HasNext();

        // Assert
        Assert.That(hasNext, Is.True);
    }

    [Test]
    public void HasNext_ShouldReturnFalse_WhenNoMorePages()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };

        var pageSize = 2;

        var pagedCollection = new PagedCollection<string>(items, "1", pageSize)
        {
            TotalRecords = 3
        };

        // Act
        var hasNext = pagedCollection.HasNext();

        // Assert
        Assert.That(hasNext, Is.False);
    }

    [Test]
    public void HasPrevious_ShouldReturnTrue_WhenThereIsAPreviousPage()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };

        var pageSize = 2;

        var pagedCollection = new PagedCollection<string>(items, "2", pageSize)
        {
            TotalRecords = 5
        };

        // Act
        var hasPrevious = pagedCollection.HasPrevious();

        // Assert
        Assert.That(hasPrevious, Is.True);
    }

    [Test]
    public void HasPrevious_ShouldReturnFalse_WhenOnFirstPage()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };

        var pageSize = 2;

        var pagedCollection = new PagedCollection<string>(items, "0", pageSize)
        {
            TotalRecords = 5
        };

        // Act
        var hasPrevious = pagedCollection.HasPrevious();

        // Assert
        Assert.That(hasPrevious, Is.False);
    }

    [Test]
    public void IsNumberPagination_ShouldReturnTrue_WhenPageIsNumeric()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };

        var pagedCollection = new PagedCollection<string>(items, "1", 2);

        // Act
        var isNumberPagination = pagedCollection.IsNumberPagination;

        // Assert
        Assert.That(isNumberPagination, Is.True);
    }

    [Test]
    public void IsNumberPagination_ShouldReturnFalse_WhenPageIsNotNumeric()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };

        var pagedCollection = new PagedCollection<string>(items, "NotANumber", 2);

        // Act
        var isNumberPagination = pagedCollection.IsNumberPagination;

        // Assert
        Assert.That(isNumberPagination, Is.False);
    }
}

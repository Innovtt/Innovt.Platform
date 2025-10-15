using Innovt.Core.Attributes;

namespace Innovt.AspNetCore.Application.Tests.ViewModels;

[ModelExcludeFilter("ExternalId", "UserId")]
public class AddUserViewModel
{
    public string? UserId { get; set; }

    public string? FirstName { get; set; }

    public string? ExternalId { get; set; }
}
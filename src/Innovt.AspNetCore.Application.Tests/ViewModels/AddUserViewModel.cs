using System.Text.Json.Serialization;

namespace Innovt.AspNetCore.Application.Tests.ViewModels;

public class AddUserViewModel
{
    [JsonIgnore] public string UserId { get; set; }

    public string FirstName { get; set; }

    [JsonIgnore] public string ExternalId { get; set; }
}
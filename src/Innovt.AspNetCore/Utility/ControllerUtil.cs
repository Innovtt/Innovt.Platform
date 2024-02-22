// Innovt Company
// Author: Michel Borges
// Project: Innovt.AspNetCore

using System.ComponentModel;
using System.Reflection;
using Innovt.AspNetCore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Innovt.AspNetCore.Utility;

/// <summary>
///     Utility class for reading and processing MVC controllers and actions.
/// </summary>
public static class ControllerUtil
{
    /// <summary>
    ///     Reads and retrieves information about MVC controllers and their actions.
    /// </summary>
    /// <param name="actionDescriptorProvider">The provider for action descriptors.</param>
    /// <returns>A list of MVC controller view models with associated actions.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the actionDescriptorProvider is null.</exception>
    public static IList<MvcControllerViewModel> ReadControllers(
        IActionDescriptorCollectionProvider actionDescriptorProvider)
    {
        ArgumentNullException.ThrowIfNull(actionDescriptorProvider);


        var controllers = new List<MvcControllerViewModel>();

        //from web this code
        var items = actionDescriptorProvider
            .ActionDescriptors.Items
            .Where(descriptor => descriptor.GetType() == typeof(ControllerActionDescriptor))
            .Select(descriptor => (ControllerActionDescriptor)descriptor)
            .GroupBy(descriptor => descriptor.ControllerTypeInfo.FullName)
            .ToList();

        foreach (var actionDescriptors in items)
        {
            if (!actionDescriptors.Any())
                continue;

            var actionDescriptor = actionDescriptors.First();

            if (!actionDescriptor.MethodInfo.IsPublic)
                continue;

            var controllerTypeInfo = actionDescriptor.ControllerTypeInfo;

            var currentController = new MvcControllerViewModel
            {
                Area = controllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue,
                DisplayName = controllerTypeInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                Name = actionDescriptor.ControllerName
            };

            var actions = new List<MvcActionViewModel>();

            foreach (var descriptor in actionDescriptors.GroupBy
                         (a => a.ActionName).Select(g => g.First()))
            {
                var methodInfo = descriptor.MethodInfo;
                actions.Add(new MvcActionViewModel
                {
                    Controller = currentController.Name,
                    Name = descriptor.ActionName,
                    DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName,
                    Method = methodInfo.GetCustomAttribute<HttpMethodAttribute>()?.Name ?? "GET"
                });
            }

            currentController.AddActions(actions);
            controllers.Add(currentController);
        }

        return controllers;
    }
}
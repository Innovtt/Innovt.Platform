// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-20
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Innovt.Contrib.Authorization.Platform.Application.Commands;
using Innovt.Core.Cqrs.Commands;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Innovt.Contrib.Authorization.AspNetCore.ViewModels
{
    public class InitViewModel : ICommand
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Domain { get; set; }


        public List<AddPermissionCommand> Permissions { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        private void ReadPermissions(IActionDescriptorCollectionProvider actionDescriptorProvider)
        {
            //from web this code
            var items = actionDescriptorProvider
                       .ActionDescriptors.Items
                       .Where(descriptor => descriptor.GetType() == typeof(ControllerActionDescriptor))
                       .Select(descriptor => (ControllerActionDescriptor)descriptor)
                       .GroupBy(descriptor => descriptor.ControllerTypeInfo.FullName)
                       .ToList();


            Permissions ??= new List<AddPermissionCommand>();

            foreach (var actionDescriptors in items)
            {
                if (!actionDescriptors.Any())
                    continue;

                var actionDescriptor = actionDescriptors.First();

                if (!actionDescriptor.MethodInfo.IsPublic)
                    continue;

                var controllerTypeInfo = actionDescriptor.ControllerTypeInfo;

                foreach (var descriptor in actionDescriptors.GroupBy
                                            (a => a.ActionName).Select(g => g.First()))
                {
                    var methodInfo = descriptor.MethodInfo;

                    var method = methodInfo.GetCustomAttribute<HttpMethodAttribute>()?.Name ?? "GET";

                    Permissions.Add(new AddPermissionCommand
                    {
                        Domain = Domain,
                        Name = actionDescriptor.ControllerName,
                        Resource = $"{actionDescriptor.ControllerName  }/{method}"
                    });


                    //Controller = currentController.Name,
                    //Name = descriptor.ActionName,

                    //Method = methodInfo.GetCustomAttribute<HttpMethodAttribute>()?.Name ?? "GET",
                }

                //currentController.Actions = actions;
                //controllers.Add(currentController);
            }
        }


        public InitCommand ToCommand(IActionDescriptorCollectionProvider actionDescriptorProvider)
        {
            var command = new InitCommand()
            {
                Email = Email,
                Password = Password,
                ConfirmPassword = ConfirmPassword,
                Domain = Domain
            };

            if (command.Permissions is null)
            {
                ReadPermissions(actionDescriptorProvider);
            }

            command.Permissions = Permissions;

            return command;
        }

    }
}
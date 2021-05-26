// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Authorization.Platform
// Solution: Innovt.Platform
// Date: 2021-05-20
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Security;

namespace Innovt.Contrib.Authorization.Platform.Domain
{
    public class InitService
    {
        private readonly IAuthorizationRepository repository;

        public InitService(IAuthorizationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
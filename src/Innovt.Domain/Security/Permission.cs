// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class Permission : ValueObject<Guid>
    {
        public Permission()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        ///     Can be the area in your Controller
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        ///     The custom name that you need to show to your customer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     * - mean that you want to authorize the full path/domain
        ///     Controller/* mean that you can authorize all actions
        ///     Controller/Action mean that you want to authorize only this action
        /// </summary>
        public string Resource { get; set; }
    }
}
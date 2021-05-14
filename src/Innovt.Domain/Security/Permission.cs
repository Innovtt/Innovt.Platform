// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Domain
// Solution: Innovt.Platform
// Date: 2021-05-03
// Contact: michel@innovt.com.br or michelmob@gmail.com

using System;
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class Permission : ValueObject<Guid>
    {
        /// <summary>
        ///     Can be the area in your Controller
        /// </summary>
        public virtual string Domain { get; set; }

        /// <summary>
        ///     The custom name that you need to show to your customer.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///  * - mean that you want to authorize the full path/domain
        ///  Controller/* mean that you can authorize all actions
        ///  Controller/Action mean that you want to authorize only this action
        /// </summary>
        public virtual string Resource { get; set; }
    }
}
using Innovt.Domain.Core.Model;

namespace Innovt.Domain.Security
{
    public class Permission: ValueObject
    {
        /// <summary>
        /// Can be the area in your Controller
        /// </summary>
        public virtual string Domain { get; set; }

        /// <summary>
        /// The custom name that you need to show to your customer.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// * - mean that you want to authoriza the full path/domain
        /// Controller/* mean that you can althorize all actions
        /// Controller/Action mean that you want to authorize only this action
        /// </summary>
        public virtual string Resource { get; set; }
    }
}

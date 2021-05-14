using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Innovt.Authorization.Platform.Application.Commands;

namespace Innovt.Authorization.Platform.Application
{
    public class SecurityAppService : BaseAppService, ISecurityAppService
    {
        private readonly ISecurityRepository repository;

        public SecurityAppService(IUnitOfWork unitWork,
           ILogger logger, ISecurityRepository securityRepository) : base(unitWork, logger)
        {
            this.repository = securityRepository ?? throw new ArgumentNullException(nameof(securityRepository));
        }

        public async Task AddPermission(AddPermissionCommand command)
        {
            Check.NotNull(command, nameof(command));

            var permission = new Permission()
            {
                Domain = command.Domain,
                Name = command.Name,
                Resource = command.Resource
            };

            await repository.AddPermission(permission);

            await UnitOfWork.CommitAsync();
        }

        public async Task AddPolicie(AddPolicyCommand command)
        {
            Check.NotNull(command, nameof(command));

            var policy = new Policy()
            {
                //Domain = command.,
                //Name = command.Name,
                //Resource = command.Resource
            };

            await repository.AddPolicy(policy);

            await UnitOfWork.CommitAsync();
        }

        public async Task<IList<PermissionDTO>> FindPermissionBy(string domain = null, string resource = null, string name = null)
        {
            var record = await repository.GetPermissionsBy();

            return record.ProjectAs();
        }

        public Task<IList<PermissionDTO>> GetPermissionsBy(string domain = null, string resource = null, string name = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<PolicyDTO>> GetPolicies(string name = null, string description = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<PermissionDTO>> GetUserPermissions(string userId, string domain = null, string resource = null)
        {
            throw new NotImplementedException();
        }

        public async Task RemovePermission(RemovePermissionCommand command)
        {
            Check.NotNull(command, nameof(command));

            var permission = new Permission()
            {
                Id = command.Id
            };

            await repository.RemovePermission(permission);
        }

        public Task RemovePolicy(RemovePolicyCommand policy)
        {
            throw new NotImplementedException();
        }
    }
}

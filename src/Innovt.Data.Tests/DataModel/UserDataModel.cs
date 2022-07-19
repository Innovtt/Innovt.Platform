using Innovt.Data.DataModels;
using Innovt.Data.Tests.Model;

namespace Innovt.Data.Tests.DataModel
{
    public class UserDataModel:BaseDataModel<User,UserDataModel>
    {
        private int id;
        private string name;
        private string lastName;
        private string address;

        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public override User ParseToDomain(UserDataModel dataModel)
        {
            return new User()
            {
                Name = dataModel.Name,
                LastName = dataModel.LastName,
                Address = dataModel.Address,
                Id = dataModel.Id
            };

        }

        public override UserDataModel ParseToDataModel(User domainModel)
        {
            return new UserDataModel()
            {
                Name = domainModel.Name,
                LastName = domainModel.LastName,
                Address = domainModel.Address,
                Id = domainModel.Id
            };
        }
    }
}
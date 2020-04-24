using System.Collections.Generic;

namespace Innovt.AspNetCore.ViewModel
{
    public class MvcControllerViewModel:ViewModelBase
    {
        public string Area { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public List<MvcActionViewModel> Actions { get; set; }
    }
}

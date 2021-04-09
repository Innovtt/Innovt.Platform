using System;
using System.Collections.Generic;


namespace Innovt.AspNetCore.ViewModel
{
    public class MvcControllerViewModel : ViewModelBase
    {
        public string Area { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        private List<MvcActionViewModel> Actions { get; set; }


        public void AddActions(IList<MvcActionViewModel> actions)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            
            this.Actions ??= new List<MvcActionViewModel>();

            this.Actions.AddRange(actions);
        }
    }
}
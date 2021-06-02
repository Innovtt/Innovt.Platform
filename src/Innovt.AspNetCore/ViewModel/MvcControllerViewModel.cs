// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

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

            Actions ??= new List<MvcActionViewModel>();

            Actions.AddRange(actions);
        }
    }
}
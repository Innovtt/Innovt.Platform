// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.AspNetCore.ViewModel;

public class MvcActionViewModel : ViewModelBase
{
    public string DisplayName { get; set; }

    public string Name { get; set; }

    public string Controller { get; set; }

    public string Method { get; set; }
}
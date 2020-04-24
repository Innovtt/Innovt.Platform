namespace Innovt.AspNetCore.ViewModel
{
    public class MvcActionViewModel: ViewModelBase
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string Controller { get; set; }

        public string Method { get; set; }
    }
}

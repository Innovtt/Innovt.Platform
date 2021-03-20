namespace Innovt.AspNetCore.ViewModel
{
    public abstract class CreateOrEditViewModel : ViewModelBase
    {
        public const string CreateAction = "Create";

        public const string EditAction = "Edit";

        public string Action { get; set; }

        public int Id { get; set; }

        public bool IsCreate => Action == CreateAction;

        public bool IsEdit => Action == EditAction;

        protected CreateOrEditViewModel()
        {
            Action = CreateAction;
        }
    }
}
// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.AspNetCore
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

namespace Innovt.AspNetCore.ViewModel;

public abstract class CreateOrEditViewModel : ViewModelBase
{
    public const string CreateAction = "Create";

    public const string EditAction = "Edit";

    protected CreateOrEditViewModel()
    {
        Action = CreateAction;
    }

    public string Action { get; set; }

    public int Id { get; set; }

    public bool IsCreate => Action == CreateAction;

    public bool IsEdit => Action == EditAction;
}
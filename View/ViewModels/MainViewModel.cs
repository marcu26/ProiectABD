using DevExpress.Mvvm.CodeGenerators;

namespace View.ViewModels
{
    [GenerateViewModel]
    public partial class MainViewModel
    {
        [GenerateProperty]
        string _Status;
        [GenerateProperty]
        string _UserName;

        [GenerateCommand]
        void Login() => Status = "User: " + UserName;
        bool CanLogin() => !string.IsNullOrEmpty(UserName);
    }
}

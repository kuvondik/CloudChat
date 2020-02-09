using System.Threading.Tasks;

namespace CloudChat
{
    public interface IUIManager
    {
        Task ShowMessage(MessageBoxDialogViewModel viewModel);
    }
}

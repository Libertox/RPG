using System.Threading.Tasks;

namespace UI
{
    public interface IView : IInputListener
    {
        public void Initialize();
        public void Open();
        public Task OpenAsync();
        public void Close();
        public Task CloseAsync();
    }
}
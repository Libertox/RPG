using UnityEngine;
using System.Threading.Tasks;

namespace UI
{
    public abstract class UIViewBase : UIElement<UIViewBase>, IInputListener
    {
        [field: SerializeField] public UIViewSO ViewID { get; private set; }

        [field: SerializeField] public bool OpenOnStart { get; private set; }

        public virtual void Initialize()
        {

        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual Task OpenAsync()
        {
            Open();
            return Task.CompletedTask;
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
        public virtual Task CloseAsync()
        {
            Close();
            return Task.CompletedTask;
        }

        public abstract void SubscribeToInputEvents();
        public abstract void UnsubscribeToInputEvents();
       
    }
}

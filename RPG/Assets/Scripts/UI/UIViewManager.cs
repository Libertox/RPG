using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.HUD;
using UnityEngine;

namespace UI
{
    public class UIViewManager : MonoBehaviour
    {
        private Dictionary<Type, IView> _views;

        private readonly Stack<IView> _viewsStack = new();

        private bool _isOpeningView;

        private void Awake()
        {
            GatherViews();

            _= TryOpenView<GameHUD>();
        }

        public async Task TryOpenView<T>(bool closeOpenedViewFirst = false) where T : IView
        {
            if (_isOpeningView) return;

            _views.TryGetValue(typeof(T), out var view);

            if (view == null) return;

            _isOpeningView = true;

            IView peekView = TryPeekView();
            peekView?.UnsubscribeToInputEvents();

            if (closeOpenedViewFirst)
                await peekView?.CloseAsync();

            await view.OpenAsync();

            view.SubscribeToInputEvents();

            if (!closeOpenedViewFirst)
                peekView?.Close();
   
            _viewsStack.Push(view);

            _isOpeningView = false;


            Debug.Log("Open View: " + view.GetType().Name);
            Debug.Log("Close View: " + peekView.GetType().Name);
        }

        private IView TryPeekView()
        {
            if (_viewsStack.Count == 0) return default;

            return _viewsStack.Peek();
        }

        public async Task OpenPreviousView()
        {
            if (_isOpeningView) return;

            _isOpeningView = true;

            IView view = _viewsStack.Pop();
            IView peekView = TryPeekView();

            view.UnsubscribeToInputEvents();

            peekView?.Open();
            peekView?.SubscribeToInputEvents();

            await view.CloseAsync();

            _isOpeningView = false;

            Debug.Log("Close View: " + view.GetType().Name);
            Debug.Log("Open View: " + peekView.GetType().Name);
        }

        public void ClearStack()
        {
            foreach (var view in _viewsStack)
            {
                view.UnsubscribeToInputEvents();
                view.Close();
            }

            _viewsStack.Clear();
        }

        private void GatherViews()
        {
            _views = new();

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out IView view))
                {
                    _views.Add(view.GetType(), view);
                    view.Initialize();
                }
            }
        }

        public void RegisterView(IView view)
        {
            if (!_views.ContainsKey(view.GetType()))
            {
                view.Initialize();
                _views.Add(view.GetType(), view);
            }

        }

        public void UnregisterView(IView view)
        {
            if (_views.ContainsKey(view.GetType()))
                _views.Remove(view.GetType());
        }


    }
}

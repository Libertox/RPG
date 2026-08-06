using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.HUD;
using UnityEngine;

namespace UI
{
    public class UIViewManager : MonoBehaviour
    {
        private Dictionary<UIViewSO, UIViewBase> _views;

        private readonly Stack<UIViewBase> _viewsStack = new();

        private bool _isOpeningView;

        private void Awake()
        {
            GatherViews();
        }

        public async Task TryOpenView(UIViewSO menuID, bool closeOpenedViewFirst = false)
        {
            if (_isOpeningView) return;

            _views.TryGetValue(menuID, out var view);

            if (view == null) return;

            _isOpeningView = true;

            UIViewBase peekView = TryPeekView();
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

        private UIViewBase TryPeekView()
        {
            if (_viewsStack.Count == 0) return default;

            return _viewsStack.Peek();
        }

        public async Task OpenPreviousView()
        {
            if (_isOpeningView) return;

            _isOpeningView = true;

            UIViewBase view = _viewsStack.Pop();
            UIViewBase peekView = TryPeekView();

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
                if (child.TryGetComponent(out UIViewBase view))
                {
                    _views.Add(view.ViewID, view);
                    view.Initialize();

                    if (view.OpenOnStart)
                    {
                        _= TryOpenView(view.ViewID);
                    }
                }
            }
        }

        public void RegisterView(UIViewBase view)
        {
            if (!_views.ContainsKey(view.ViewID))
            {
                view.Initialize();
                _views.Add(view.ViewID, view);
            }

        }

        public void UnregisterView(UIViewBase view)
        {
            if (_views.ContainsKey(view.ViewID))
                _views.Remove(view.ViewID);
        }


    }
}

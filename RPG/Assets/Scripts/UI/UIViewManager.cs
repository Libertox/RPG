using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.HUD;
using UnityEngine;

namespace UI
{
    public class UIViewManager : MonoBehaviour
    {
        private Dictionary<UIViewSO, UIViewBase> views;

        private readonly Stack<UIViewBase> viewsStack = new();

        private bool isOpeningView;

        private void Awake()
        {
            GatherViews();
        }

        public async Task TryOpenView(UIViewSO menuID, bool closeOpenedViewFirst = false)
        {
            if (isOpeningView) return;

            views.TryGetValue(menuID, out var view);

            if (view == null) return;

            isOpeningView = true;

            UIViewBase peekView = TryPeekView();
            peekView?.UnsubscribeToInputEvents();

            if (closeOpenedViewFirst)
                await peekView?.CloseAsync();

            await view.OpenAsync();

            view.SubscribeToInputEvents();

            if (!closeOpenedViewFirst)
                peekView?.Close();
   
            viewsStack.Push(view);

            isOpeningView = false;


            Debug.Log("Open View: " + view.GetType().Name);
            Debug.Log("Close View: " + peekView.GetType().Name);
        }

        private UIViewBase TryPeekView()
        {
            if (viewsStack.Count == 0) return default;

            return viewsStack.Peek();
        }

        public async Task OpenPreviousView()
        {
            if (isOpeningView) return;

            isOpeningView = true;

            UIViewBase view = viewsStack.Pop();
            UIViewBase peekView = TryPeekView();

            view.UnsubscribeToInputEvents();

            peekView?.Open();
            peekView?.SubscribeToInputEvents();

            await view.CloseAsync();

            isOpeningView = false;

            Debug.Log("Close View: " + view.GetType().Name);
            Debug.Log("Open View: " + peekView.GetType().Name);
        }

        public void ClearStack()
        {
            foreach (var view in viewsStack)
            {
                view.UnsubscribeToInputEvents();
                view.Close();
            }

            viewsStack.Clear();
        }

        private void GatherViews()
        {
            views = new();

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out UIViewBase view))
                {
                    views.Add(view.ViewID, view);
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
            if (!views.ContainsKey(view.ViewID))
            {
                view.Initialize();
                views.Add(view.ViewID, view);
            }

        }

        public void UnregisterView(UIViewBase view)
        {
            if (views.ContainsKey(view.ViewID))
                views.Remove(view.ViewID);
        }


    }
}

using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownPopupController
    {
        private readonly DropdownPopupView _view = new();
        private readonly List<DropdownItemController> _itemControllers = new();

        public event EventHandler<DropdownItemSelectedEventArgs> ItemSelected;

        public VisualElement Root => _view.Root;

        public int ItemCount => _itemControllers.Count;

        public bool TryInitialize()
        {
            return _view.TryLoadFromTemplate();
        }

        public void PopulateItems(IReadOnlyList<VersionInfo> versions, VersionInfo selectedVersion)
        {
            foreach (var controller in _itemControllers)
            {
                controller.Clicked -= OnItemClicked;
            }
            _itemControllers.Clear();
            _view.ClearItems();

            foreach (var version in versions)
            {
                var controller = CreateItemController(version);
                if (controller == null) continue;

                controller.SetSelected(version == selectedVersion);
                _view.AddItem(controller.Root);
                _itemControllers.Add(controller);
            }
        }

        public void SetNoScrollbar(bool noScrollbar)
        {
            _view.SetNoScrollbar(noScrollbar);
        }

        private DropdownItemController CreateItemController(VersionInfo version)
        {
            var controller = new DropdownItemController(version);

            if (!controller.TryInitialize())
            {
                return null;
            }

            controller.Clicked += OnItemClicked;

            return controller;
        }

        private void OnItemClicked(object sender, DropdownItemSelectedEventArgs args)
        {
            ItemSelected?.Invoke(this, args);
        }
    }
}

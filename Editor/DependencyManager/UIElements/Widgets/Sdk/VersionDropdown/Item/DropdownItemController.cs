using System;
using System.Linq;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class DropdownItemController
    {
        private const string DefaultRecommendedTooltip = "Recommended";
        private const string DefaultNewTooltip = "New";
        private const string DefaultDeprecatedTooltip = "Deprecated";
        private const string DefaultUnstableTooltip = "Unstable";

        private readonly DropdownItemView _view = new();

        public event EventHandler<DropdownItemSelectedEventArgs> Clicked;

        private VersionInfo Model { get; }

        public VisualElement Root => _view.Root;

        public DropdownItemController(VersionInfo version)
        {
            Model = version;
        }

        public bool TryInitialize()
        {
            if (!_view.TryLoadFromTemplate())
            {
                return false;
            }

            ApplyModel();
            _view.Clicked += OnViewClicked;
            return true;
        }

        public void SetSelected(bool isSelected)
        {
            _view.SetSelected(isSelected);
        }

        private void ApplyModel()
        {
            if (Model == null)
            {
                _view.SetVersionText("none");
                _view.SetTooltip(null);
                _view.HideAllBadges();
                return;
            }

            _view.SetVersionText(Model.Name);
            _view.SetTooltip(Model.Message);
            _view.HideAllBadges();

            if (Model.IsRecommended) _view.SetBadgeRecommended(true, GetBadgeMessage(BadgeType.Recommended) ?? DefaultRecommendedTooltip);
            if (Model.IsNew) _view.SetBadgeNew(true, GetBadgeMessage(BadgeType.New) ?? DefaultNewTooltip);
            if (Model.IsDeprecated) _view.SetBadgeDeprecated(true, GetBadgeMessage(BadgeType.Deprecated) ?? DefaultDeprecatedTooltip);
            if (Model.IsUnstable) _view.SetBadgeUnstable(true, GetBadgeMessage(BadgeType.Unstable) ?? DefaultUnstableTooltip);
        }

        private string GetBadgeMessage(BadgeType type)
        {
            return Model?.Badges?.FirstOrDefault(b => b.Type == type)?.Message;
        }

        private void OnViewClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, new DropdownItemSelectedEventArgs(Model));
        }
    }
}

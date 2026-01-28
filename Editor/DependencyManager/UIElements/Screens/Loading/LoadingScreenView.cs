using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class LoadingScreenView : IDisposable
    {
        private const string TemplatePath = "Appodeal/DependencyManager/Screens/LoadingScreen";

        private const float RotationSpeed = 8f;
        private const int FrameIntervalMs = 16;

        private VisualElement _spinner;
        private IVisualElementScheduledItem _rotationSchedule;

        public VisualElement Root { get; private set; }

        public void Dispose()
        {
            _rotationSchedule?.Pause();
            _rotationSchedule = null;

            if (Root != null)
            {
                Root.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
                Root.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
            }
        }

        public bool TryLoadFromTemplate()
        {
            var template = Resources.Load<VisualTreeAsset>(TemplatePath);
            if (template == null)
            {
                LogHelper.LogError($"[LoadingScreenView] Failed to load template from Resources/{TemplatePath}");
                return false;
            }

            Root = template.Instantiate();

            _spinner = Root.Q<VisualElement>(DmConstants.Uxml.LoadingScreen.Spinner);
            if (_spinner == null)
            {
                LogHelper.LogError("[LoadingScreenView] Failed to find Spinner element in template");
                return false;
            }

            Root.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            Root.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            return true;
        }

        private void RotateSpinner()
        {
            float currentAngle = _spinner.style.rotate.value.angle.value;
            _spinner.style.rotate = new Rotate((currentAngle + RotationSpeed) % 360f);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            _rotationSchedule = _spinner.schedule.Execute(RotateSpinner).Every(FrameIntervalMs);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            _rotationSchedule?.Pause();
            _rotationSchedule = null;
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.UIElements;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    internal interface ISdkCardController : IDisposable
    {
        Sdk AndroidModel { get; }
        Sdk IosModel { get; }

        VersionInfo SelectedAndroidVersion { get; }
        VersionInfo SelectedIosVersion { get; }

        VisualElement Root { get; }

        string SdkId { get; }
        string SdkName { get; set; }
        bool IsSelected { get; }
        bool HasAnyVersions { get; }

        event EventHandler<SelectionChangedEventArgs> SelectionChanged;
        event EventHandler<VersionChangedEventArgs> AndroidVersionChanged;
        event EventHandler<VersionChangedEventArgs> IosVersionChanged;

        bool TryInitialize();
        void SetSelectedWithoutNotify(bool value);
    }
}

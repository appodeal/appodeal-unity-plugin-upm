using UnityEditor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class PostUpdateBootstrap
    {
        private static bool _subscribed;

        [InitializeOnLoadMethod]
        private static void Init() => SubscribeIfNeeded();

        private static void SubscribeIfNeeded()
        {
            if (!SessionState.GetBool(DmConstants.Plugin.PostUpdatePendingKey, false)) return;
            if (_subscribed) return;

            _subscribed = true;
            EditorApplication.update += Tick;
        }

        private static void Unsubscribe()
        {
            if (!_subscribed) return;

            _subscribed = false;
            EditorApplication.update -= Tick;
        }

        private static void Tick()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;

            if (!SessionState.GetBool(DmConstants.Plugin.PostUpdatePendingKey, false))
            {
                Unsubscribe();
                return;
            }

            DmAssetPostprocessor.RunPostUpdateFlow();
            if (!SessionState.GetBool(DmConstants.Plugin.PostUpdatePendingKey, false)) Unsubscribe();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
#if APPODEAL_DEV
    [CreateAssetMenu(menuName = "Appodeal/SDK Info (Scriptable Object)", fileName = "New SDK Info")]
#endif
    internal class SdkInfoScriptableObject : ScriptableObject
    {
        private static SdksInfoContainerScriptableObject _sdksInfoSo;

        [SerializeField] private string id;
        [SerializeField] private string description;
        [SerializeField] private Texture2D texture;

        public string Description => description;
        public Texture2D Texture => texture;

        private string Id => id;

        private static List<SdkInfoScriptableObject> SdksInfo
        {
            get
            {
                if (_sdksInfoSo) return _sdksInfoSo.SdksInfo;
                _sdksInfoSo = AssetDatabase.LoadAssetAtPath<SdksInfoContainerScriptableObject>(DmConstants.SdkCatalog.SdksInfoPath);
                return _sdksInfoSo?.SdksInfo ?? new List<SdkInfoScriptableObject>();
            }
        }

        public static Outcome<SdkInfoScriptableObject> TryGetById(string sdkId)
        {
            var defaultSdkInfo = SdksInfo.FirstOrDefault(sdkInfo => sdkInfo.Id == DmConstants.SdkCatalog.UnlistedSdkId);
            var requestedSdkInfo = SdksInfo.FirstOrDefault(sdkInfo => sdkInfo.Id == sdkId);
            if (defaultSdkInfo == null && requestedSdkInfo == null) return Failure.Create("ItemNotFound", $"{nameof(TryGetById)} failed to find requested info");
            return requestedSdkInfo == null ? defaultSdkInfo : requestedSdkInfo;
        }
    }
}

// ReSharper disable CheckNamespace

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

        public string Description { get => description; }
        public Texture2D Texture { get => texture; }

        private string Id { get => id; }

        private static List<SdkInfoScriptableObject> SdksInfo
        {
            get
            {
                if (_sdksInfoSo) return _sdksInfoSo.SdksInfo;
                _sdksInfoSo = AssetDatabase.LoadAssetAtPath<SdksInfoContainerScriptableObject>(DmConstants.SdksInfoSoPath);
                return _sdksInfoSo?.SdksInfo ?? new List<SdkInfoScriptableObject>();
            }
        }

        public static Request<SdkInfoScriptableObject> Get(string sdkId)
        {
            var defaultSdkInfo = SdksInfo.FirstOrDefault(sdkInfo => sdkInfo.Id == DmConstants.UnlistedSdkInfoId);
            var requestedSdkInfo = SdksInfo.FirstOrDefault(sdkInfo => sdkInfo.Id == sdkId);
            if (defaultSdkInfo == null && requestedSdkInfo == null) return Error.Create("ItemNotFound", $"{nameof(Get)} failed to find requested info");
            return requestedSdkInfo == null ? defaultSdkInfo : requestedSdkInfo;
        }
    }
}

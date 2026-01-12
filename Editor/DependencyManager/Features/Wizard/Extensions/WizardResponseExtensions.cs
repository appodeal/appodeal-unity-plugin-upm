using System.Collections.Generic;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class WizardResponseExtensions
    {
        public static List<(string name, Sdk android, Sdk ios)> GroupSdksByName(this WizardResponse data)
        {
            if (data == null) return new List<(string, Sdk, Sdk)>();

            var sdkMap = new Dictionary<string, (Sdk android, Sdk ios)>();

            if (data.Android != null)
            {
                foreach (var sdk in data.Android)
                {
                    if (!sdkMap.ContainsKey(sdk.Name)) sdkMap[sdk.Name] = (null, null);
                    var current = sdkMap[sdk.Name];
                    sdkMap[sdk.Name] = (sdk, current.ios);
                }
            }

            if (data.Ios != null)
            {
                foreach (var sdk in data.Ios)
                {
                    if (!sdkMap.ContainsKey(sdk.Name)) sdkMap[sdk.Name] = (null, null);
                    var current = sdkMap[sdk.Name];
                    sdkMap[sdk.Name] = (current.android, sdk);
                }
            }

            var result = new List<(string, Sdk, Sdk)>();
            foreach (var kvp in sdkMap)
            {
                result.Add((kvp.Key, kvp.Value.android, kvp.Value.ios));
            }
            return result;
        }
    }
}

using System;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class ProblematicSdkDtoExtensions
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static ProblematicSdk ToDomain(this ProblematicSdkDto dto)
        {
            if (dto == null) return null;

            if (String.IsNullOrWhiteSpace(dto.sdk_name))
            {
                Log.Warning("Problematic SDK with empty name received from validation endpoint. Skipping");
                return null;
            }

            string message = String.IsNullOrWhiteSpace(dto.message) ? "No details provided" : dto.message;

            return new ProblematicSdk
            {
                SdkName = dto.sdk_name,
                Message = message
            };
        }
    }
}

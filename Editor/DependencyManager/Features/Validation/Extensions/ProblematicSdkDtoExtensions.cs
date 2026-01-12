using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class ProblematicSdkDtoExtensions
    {
        public static ProblematicSdk ToDomain(this ProblematicSdkDto dto)
        {
            if (dto == null) return null;

            if (String.IsNullOrWhiteSpace(dto.sdk_name))
            {
                LogHelper.LogWarning("Problematic SDK with empty name received from validation endpoint. Skipping");
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

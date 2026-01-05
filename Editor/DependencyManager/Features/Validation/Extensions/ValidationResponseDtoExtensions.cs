using System.Collections.Generic;
using System.Linq;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class ValidationResponseDtoExtensions
    {
        public static ValidationResponse ToDomain(this ValidationResponseDto dto)
        {
            if (dto == null) return null;

            var problematicSdks = new List<ProblematicSdk>();
            if (dto.problematic_sdk_versions != null)
            {
                problematicSdks.AddRange(dto.problematic_sdk_versions
                    .Select(sdkDto => sdkDto.ToDomain())
                    .Where(sdk => sdk != null));
            }

            return new ValidationResponse
            {
                ProblematicSdks = problematicSdks
            };
        }
    }
}

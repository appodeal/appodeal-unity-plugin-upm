using System.Collections.Generic;
using System.Linq;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class WizardResponseDtoExtensions
    {
        public static WizardResponse ToDomain(this WizardResponseDto dto)
        {
            if (dto == null) return null;

            var android = new List<Sdk>();
            if (dto.android != null)
            {
                android.AddRange(dto.android.Select(sdkDto => sdkDto.ToDomain()).Where(sdk => sdk != null));
            }

            var ios = new List<Sdk>();
            if (dto.ios != null)
            {
                ios.AddRange(dto.ios.Select(sdkDto => sdkDto.ToDomain()).Where(sdk => sdk != null));
            }

            return new WizardResponse
            {
                Android = android,
                Ios = ios
            };
        }
    }
}

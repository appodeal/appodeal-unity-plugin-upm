using System;
using System.Collections.Generic;
using System.Linq;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class SdkDtoExtensions
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static Sdk ToDomain(this SdkDto dto)
        {
            if (dto == null) return null;

            if (String.IsNullOrWhiteSpace(dto.name))
            {
                Log.Error("SDK name is null or empty. Skipping SDK");
                return null;
            }

            if (!Enum.IsDefined(typeof(Category), dto.category))
            {
                Log.Error($"Invalid category value '{dto.category}' for SDK '{dto.name}'. Skipping SDK");
                return null;
            }

            if (!Enum.IsDefined(typeof(Requirement), dto.requirement))
            {
                Log.Error($"Invalid requirement value '{dto.requirement}' for SDK '{dto.name}'. Skipping SDK");
                return null;
            }

            string displayName = String.IsNullOrWhiteSpace(dto.display_name) ? dto.name : dto.display_name;

            AdTypes? adTypes = null;
            if (dto.ad_types != 0 && (dto.ad_types & ~(int)AdTypes.All) == 0)
            {
                adTypes = (AdTypes)dto.ad_types;
            }

            var versions = new List<VersionInfo>();
            if (dto.versions != null)
            {
                versions.AddRange(dto.versions.Select(versionDto => versionDto.ToDomain()).Where(version => version != null));
            }

            return new Sdk
            {
                Name = dto.name,
                DisplayName = displayName,
                Category = (Category)dto.category,
                Requirement = (Requirement)dto.requirement,
                AdTypes = adTypes,
                Versions = versions,
                Warning = dto.warning
            };
        }
    }
}

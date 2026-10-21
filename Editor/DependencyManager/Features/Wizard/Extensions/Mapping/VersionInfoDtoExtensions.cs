using System;
using System.Collections.Generic;
using System.Linq;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class VersionInfoDtoExtensions
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static VersionInfo ToDomain(this VersionInfoDto dto)
        {
            if (dto == null) return null;

            if (dto.id <= 0)
            {
                Log.Error($"Invalid version ID '{dto.id}'. Version ID must be greater than 0. Skipping version");
                return null;
            }

            if (String.IsNullOrWhiteSpace(dto.name))
            {
                Log.Error($"Version name is null or empty for version ID '{dto.id}'. Skipping version");
                return null;
            }

            var badges = new List<Badge>();
            if (dto.badges != null)
            {
                badges.AddRange(dto.badges
                    .Select(b => b.ToDomain())
                    .Where(b => b != null)
                    .GroupBy(b => b.Type)
                    .Select(g => g.First()));
            }

            return new VersionInfo
            {
                Id = dto.id,
                Name = dto.name,
                Message = dto.message,
                Badges = badges,
                Mediations = dto.mediations
            };
        }
    }
}

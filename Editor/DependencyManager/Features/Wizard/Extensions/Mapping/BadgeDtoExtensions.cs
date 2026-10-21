using System;
using AppodealInc.Mediation.Shared.Editor;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class BadgeDtoExtensions
    {
        private static readonly AppodealLogger Log = DmLogging.Log;

        public static Badge ToDomain(this BadgeDto dto)
        {
            if (dto == null) return null;

            if (!Enum.IsDefined(typeof(BadgeType), dto.type))
            {
                Log.Error($"Invalid badge type value '{dto.type}'. Skipping badge");
                return null;
            }

            return new Badge
            {
                Type = (BadgeType)dto.type,
                Message = dto.message
            };
        }
    }
}

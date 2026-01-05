using System;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal static class BadgeDtoExtensions
    {
        public static Badge ToDomain(this BadgeDto dto)
        {
            if (dto == null) return null;

            if (!Enum.IsDefined(typeof(BadgeType), dto.type))
            {
                LogHelper.LogError($"Invalid badge type value '{dto.type}'. Skipping badge");
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

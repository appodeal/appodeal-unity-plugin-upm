using System.Collections.Generic;
using System.Linq;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class VersionInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public List<Badge> Badges { get; set; }
        public List<string> Mediations { get; set; }

        public bool IsRecommended => HasBadge(BadgeType.Recommended);
        public bool IsDeprecated => HasBadge(BadgeType.Deprecated);
        public bool IsUnstable => HasBadge(BadgeType.Unstable);
        public bool IsNew => HasBadge(BadgeType.New);

        private bool HasBadge(BadgeType type)
        {
            return Badges != null && Badges.Any(badge => badge.Type == type);
        }
    }
}

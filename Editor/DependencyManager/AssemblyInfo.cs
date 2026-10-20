using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AppodealInc.Mediation.TopBarMenu.Editor")]

[assembly: InternalsVisibleTo("AppodealInc.Mediation.Editor.Tests")]

#if APPODEAL_DEV_TESTS
[assembly: InternalsVisibleTo("AppodealInc.Mediation.DependencyManager.Editor.Tests")]
#endif

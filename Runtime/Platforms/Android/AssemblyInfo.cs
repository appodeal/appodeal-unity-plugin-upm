using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AppodealStack.Monetization.Platforms")]

#if APPODEAL_DEV_TESTS
[assembly: InternalsVisibleTo("AppodealStack.Monetization.Platforms.Android.Tests")]
#endif

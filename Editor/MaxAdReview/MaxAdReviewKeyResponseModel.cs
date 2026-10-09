#if UNITY_ANDROID || APPODEAL_DEV
using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealInc.Mediation.MaxAdReview.Editor
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class MaxAdReviewKeyResponseModel
    {
        public string api_key;
    }
}
#endif

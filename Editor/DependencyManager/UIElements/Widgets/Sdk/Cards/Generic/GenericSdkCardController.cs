namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal class GenericSdkCardController : SdkCardControllerBase<GenericSdkCardView>
    {
        public GenericSdkCardController(Sdk androidModel, Sdk iosModel) : base(new GenericSdkCardView(), androidModel, iosModel) { }

        protected override void ApplyModel()
        {
            base.ApplyModel();

            if (SdkId == null) return;

            var outcome = SdkInfoScriptableObject.TryGetById(SdkId);
            if (outcome.IsSuccess)
            {
                View.SetSdkIconTexture(outcome.Value.Texture);
            }
        }
    }
}

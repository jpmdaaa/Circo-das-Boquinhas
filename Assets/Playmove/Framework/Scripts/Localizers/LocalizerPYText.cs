using Playmove.Core.Bundles;
using Playmove.Framework.Localizers;

public class LocalizerPYText : Localizer<PYText>
{
    protected override void Localize()
    {
        if (string.IsNullOrEmpty(AssetName)) return;
        Component.Text = Localization.GetAsset<string>(AssetName);
    }
}
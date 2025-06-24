using UnityEngine.Events;

namespace Playmove.Framework.Popups
{
    public class PopupButton
    {
        public UnityAction<Popup> Action;
        public string AssetName = "PopupButtonText";
        public bool ClosePopup = true;
        public string Text = "Ok";

        public PopupButton()
        {
        }

        public PopupButton(string text)
        {
            Text = text;
        }

        public PopupButton(string text, UnityAction<Popup> action)
        {
            Text = text;
            Action = action;
        }

        public PopupButton(string assetName, string text)
        {
            AssetName = assetName;
            Text = text;
        }

        public PopupButton(string assetName, string text, bool closePopup,
            UnityAction<Popup> action)
        {
            AssetName = assetName;
            Text = text;
            ClosePopup = closePopup;
            Action = action;
        }
    }
}
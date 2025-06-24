using System.Collections;
using Playmove.Core.BasicEvents;
using Playmove.Core.Bundles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Playmove.Framework
{
    public class Fader : MonoBehaviour, IPointerClickHandler
    {
        private static Fader _instance;

        /// <summary>
        ///     This is like a persistent event you need to remove the listener by yourself
        /// </summary>
        public static PlaytableEvent OnClick = new();

        /// <summary>
        ///     On every execution all listeners will be removed
        /// </summary>
        public static PlaytableEvent OnClickOneShot = new();

        private UnityAction _completed;
        private float _duration;
        private Color _from;

        private Image _image;
        private bool _isFading;

        private float _timer;
        private Color _to;

        public static Transform PopupCanvas => GameObject.Find("PlaytableCanvasPopup").transform;

        public static Fader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var asset = Instantiate(Data.GetAsset<GameObject>("Fader"), PopupCanvas, false);
                    asset.transform.SetAsFirstSibling();
                    asset.gameObject.SetActive(false);
                    _instance = asset.GetComponent<Fader>();
                }

                return _instance;
            }
        }

        private Image Image
        {
            get
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                return _image;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
            OnClickOneShot.Invoke();
            OnClickOneShot.RemoveAllListeners();
        }

        public static void FadeIn(UnityAction completed = null)
        {
            FadeTo(0.75f, 0.5f, completed);
        }

        public static void FadeOut(UnityAction completed = null)
        {
            FadeTo(0, 0.5f, completed);
        }

        private static GameObject FindObject(GameObject parent, string name)
        {
            var trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (var t in trs)
                if (t.name == name)
                    return t.gameObject;
            return null;
        }

        public static void FadeTo(float to, float duration, UnityAction completed = null)
        {
            var colorTo = Instance.Image.color;
            colorTo.a = to;
            FadeTo(colorTo, duration, completed);
        }

        public static void FadeTo(Color to, float duration, UnityAction completed = null)
        {
            Instance.Fade(Instance.Image.color, to, duration, completed);
        }

        private void Fade(float from, float to, float duration, UnityAction completed)
        {
            var colorFrom = Image.color;
            colorFrom.a = from;
            var colorTo = Image.color;
            colorTo.a = to;
            Fade(colorFrom, colorTo, duration, completed);
        }

        private void Fade(Color from, Color to, float duration, UnityAction completed)
        {
            gameObject.SetActive(true);
            if (_isFading)
            {
                _completed?.Invoke();
                _completed = completed;

                _from = from;
                _to = to;
                _timer = 0;
                _duration = duration;
                return;
            }

            StartCoroutine(FadeRoutine(from, to, duration, completed));
        }

        private IEnumerator FadeRoutine(Color from, Color to, float duration, UnityAction completed)
        {
            _completed = completed;
            _isFading = true;
            _from = from;
            _to = to;
            _timer = 0;
            _duration = duration;
            while (_timer < 1)
            {
                Image.color = Color.Lerp(_from, _to, _timer);
                _timer += Time.deltaTime / _duration;
                yield return null;
            }

            Image.color = _to;
            _completed?.Invoke();

            if (Image.color.a == 0) gameObject.SetActive(false);
            _isFading = false;
        }
    }
}
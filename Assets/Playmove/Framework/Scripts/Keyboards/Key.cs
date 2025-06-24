using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Playmove.Framework.Keyboards
{
    public class KeyEvent : UnityEvent<Key>
    {
    }

    /// <summary>
    ///     Responsible to control each key in Keyboard
    /// </summary>
    public class Key : MonoBehaviour
    {
        [SerializeField] private bool _interactable = true;

        [SerializeField] private KeyType _type = KeyType.Letter;

        [SerializeField] private string _letter;

        private Button _button;

        private CanvasGroup _group;

        private TextMeshProUGUI _textPro;

        /// <summary>
        ///     Event when the key gets clicked
        /// </summary>
        public KeyEvent OnClick = new();

        /// <summary>
        ///     Indicates if this key is Interactable
        /// </summary>
        public bool Interactable
        {
            get => _interactable;
            set
            {
                Button.interactable = _interactable = value;
                UpdateInteractableVisual();
            }
        }

        /// <summary>
        ///     Key type
        /// </summary>
        public KeyType Type => _type;

        /// <summary>
        ///     Letter value if any
        /// </summary>
        public string Letter
        {
            get => _letter;
            set
            {
                _letter = value;
                if (TextPro != null) TextPro.text = _letter;
            }
        }

        private TextMeshProUGUI TextPro
        {
            get
            {
                if (_textPro == null)
                    _textPro = GetComponentInChildren<TextMeshProUGUI>();
                return _textPro;
            }
        }

        private CanvasGroup Group
        {
            get
            {
                if (_group == null)
                {
                    _group = GetComponent<CanvasGroup>();
                    if (_group == null)
                        _group = gameObject.AddComponent<CanvasGroup>();
                }

                return _group;
            }
        }

        private Button Button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                return _button;
            }
        }

        private void Awake()
        {
            Interactable = _interactable;
            Button.onClick.AddListener(() => OnClick.Invoke(this));
        }

        private void Start()
        {
            if (Type == KeyType.Letter)
                Letter = _letter;
        }

        /// <summary>
        ///     Update the graphics of this key depending on it's Interactable state
        /// </summary>
        private void UpdateInteractableVisual()
        {
            if (Interactable)
                Group.alpha = 1;
            else
                Group.alpha = 0.5f;
        }
    }
}
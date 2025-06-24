/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using UnityEngine;

namespace TouchScript.Devices.Display
{
    /// <summary>
    ///     A simple display device which inherits from <see cref="ScriptableObject" /> and can be saved in Unity assets.
    /// </summary>
    public class DisplayDevice : ScriptableObject, IDisplayDevice
    {
        /// <summary>
        ///     Serialized device name.
        /// </summary>
        [SerializeField] protected new string name = "Unknown Device";

        /// <summary>
        ///     Serialized device DPI.
        /// </summary>
        [SerializeField] protected float dpi = 96;

        /// <summary>
        ///     OnEnable Unity method.
        /// </summary>
        protected virtual void OnEnable()
        {
            base.name = name;
        }

        /// <inheritdoc />
        public string Name
        {
            get => name;
            set
            {
                name = value;
                base.name = value;
            }
        }

        /// <inheritdoc />
        public virtual float DPI
        {
            get => dpi;
            set => dpi = value;
        }
    }
}
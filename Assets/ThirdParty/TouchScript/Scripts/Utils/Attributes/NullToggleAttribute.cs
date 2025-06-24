/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using UnityEngine;

namespace TouchScript.Utils.Attributes
{
    /// <summary>
    ///     <para>An attribute to use with NullToggle item drawer.</para>
    ///     <para>
    ///         <b>For internal use only!</b>
    ///     </para>
    /// </summary>
    public class NullToggleAttribute : PropertyAttribute
    {
        /// <summary>
        ///     Float value
        /// </summary>
        public float NullFloatValue = 0f;

        /// <summary>
        ///     Int value
        /// </summary>
        public int NullIntValue = 0;

        /// <summary>
        ///     Object value
        /// </summary>
        public Object NullObjectValue = null;
    }
}
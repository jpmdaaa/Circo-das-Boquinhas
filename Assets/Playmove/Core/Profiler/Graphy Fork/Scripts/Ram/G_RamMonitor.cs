/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Tayx.Graphy.Ram
{
    public class G_RamMonitor : MonoBehaviour
    {
        #region Methods -> Unity Callbacks

        private void Update()
        {
#if UNITY_5_6_OR_NEWER
            AllocatedRam = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
            ReservedRam = Profiler.GetTotalReservedMemoryLong() / 1048576f;
            MonoRam = Profiler.GetMonoUsedSizeLong() / 1048576f;
#else
            m_allocatedRam = Profiler.GetTotalAllocatedMemory()    / 1048576f;
            m_reservedRam = Profiler.GetTotalReservedMemory()     / 1048576f;
            m_monoRam = Profiler.GetMonoUsedSize()            / 1048576f;
#endif
        }

        #endregion

        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * --------------------------------------*/

        #region Variables -> Private

        #endregion

        #region Properties -> Public

        public float AllocatedRam { get; private set; }

        public float ReservedRam { get; private set; }

        public float MonoRam { get; private set; }

        #endregion
    }
}
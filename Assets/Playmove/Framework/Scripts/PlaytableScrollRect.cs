using UnityEngine;
using UnityEngine.UI;

namespace Playmove.Framework
{
    public class PlaytableScrollRect : ScrollRect
    {
        [SerializeField] [Range(0, 1)] protected float _minHorizontalScrollSize;
        [SerializeField] [Range(0, 1)] protected float _minVerticalScrollSize;

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (horizontalScrollbar != null && horizontalScrollbar.size < _minHorizontalScrollSize)
                horizontalScrollbar.size = _minHorizontalScrollSize;
            if (verticalScrollbar != null && verticalScrollbar.size < _minVerticalScrollSize)
                verticalScrollbar.size = _minVerticalScrollSize;
        }

        public override void Rebuild(CanvasUpdate executing)
        {
            base.Rebuild(executing);
            if (horizontalScrollbar != null && horizontalScrollbar.size < _minHorizontalScrollSize)
                horizontalScrollbar.size = _minHorizontalScrollSize;
            if (verticalScrollbar != null && verticalScrollbar.size < _minVerticalScrollSize)
                verticalScrollbar.size = _minVerticalScrollSize;
        }
    }
}
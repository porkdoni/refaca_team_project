using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IdleTycoon.UI
{
    public class UIUtility
    {
        /// <summary>Forces rebuild right away from this and all child objects</summary>
        public static void ForceRebuildRecursive(RectTransform target)
        {
            List<LayoutGroup> cache = target.GetComponentsInChildren<LayoutGroup>().ToList();
            if (cache.Count > 0)
            {
                target.GetComponentsInChildren(cache[0].GetType());

                // Need to start at the deepest child object
                cache.Reverse();

                foreach (LayoutGroup layout in cache)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)layout.transform);
                }

            }
        }
    }
}


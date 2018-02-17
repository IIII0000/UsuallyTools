using UnityEngine;

namespace Tools.Helper
{
    public static class TransformHelper
    {
        /// <summary>
        /// 递归查找子物体
        /// </summary>
        /// <param name="parent">起始位置</param>
        /// <param name="name">需要查找的物体名</param>
        /// <returns></returns>
        public static Transform FindChild(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child != null)
            {
                return child;
            }
            for (int i = 0; i < parent.childCount; i++)
            {
                var newParent = parent.GetChild(i);
                child = FindChild(newParent, name);
                if (child != null)
                {
                    return child;
                }
            }
            return null;
        }
    }
}

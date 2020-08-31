using UnityEngine;

namespace Nrtx
{
    public static class GameObjectHelper
    {
        public static GameObject FindTagInChild(GameObject go, string tag)
        {
            if (go.transform.CompareTag(tag) == true)
            {
                return go;
            }
            int childCount = go.transform.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                Transform child = go.transform.GetChild(i);
                if (child.CompareTag(tag) == true)
                {
                    return child.gameObject;
                }
                if (child.childCount > 0)
                {
                    GameObject tagInChild = FindTagInChild(child.gameObject, tag);
                    if (tagInChild != null)
                    {
                        return tagInChild;
                    }
                }
            }
            return null;
        }
    }
}
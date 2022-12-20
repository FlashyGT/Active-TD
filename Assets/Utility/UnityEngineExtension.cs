using System.Collections.Generic;

namespace UnityEngine
{
    public static class UnityEngineExtension
    {
        public static T GetComponentInDirectChildren<T>(this GameObject parent) where T : Component
        {
            return parent.GetComponentInDirectChildren<T>(false);
        }

        public static T GetComponentInDirectChildren<T>(this GameObject parent, bool includeInactive)
            where T : Component
        {
            foreach (Transform child in parent.transform)
            {
                if (includeInactive || child.gameObject.activeInHierarchy)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }

            return null;
        }

        public static List<T> GetComponentsInDirectChildren<T>(this GameObject parent) where T : Component
        {
            return parent.GetComponentsInDirectChildren<T>(false);
        }

        public static List<T> GetComponentsInDirectChildren<T>(this GameObject parent, bool includeInactive)
            where T : Component
        {
            List<T> tempList = new List<T>();
            foreach (Transform child in parent.transform)
            {
                if (includeInactive || child.gameObject.activeInHierarchy)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                    {
                        tempList.Add(component);
                    }
                }
            }

            return tempList;
        }
    }
}
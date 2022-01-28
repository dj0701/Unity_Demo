using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtentMothed
{
    public static Transform GetChild(Transform trans, string childName)
    {
        Transform childTrans = trans.Find(childName);//查找名字为childName的子物体
        if (childTrans != null)
        {
            return childTrans;
        }
        if (trans.childCount == 0)
        {
            return null;
        }
        for (int i = 0; i < trans.childCount; i++)
        {
            Transform childTF = trans.GetChild(i);
            childTrans = GetChild(childTF, childName);
            if (childTrans != null)
            {
                return childTrans;
            }
        }
        return null;
    }

    public static Transform FindChildWithName(this Transform tf, string name)
    {
        return GetChild(tf, name);
    }

}

﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace litefeel.Finder.Editor
{
    class FindMissingOnSelection : FindWindowBase<UnityObject>
    {
        protected override bool InGameObject(GameObject prefab)
        {
            var comps = prefab.GetComponentsInChildren<Component>(true);
            for(var i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                    return true;
            }
            return false;
        }

        protected override void OnItemDoubleClick(int index)
        {
            AssetDatabase.OpenAsset(m_Items[index]);
        }

    }
}


﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace litefeel.Finder.Editor
{
    abstract class FindAssetWindowBase<TAsset, TObject> : FindWindowBase<TObject>, IFinderWindow
        where TAsset : UnityObject
        where TObject : UnityObject
    {
        protected TAsset m_Asset;

        public virtual void InitAsset(UnityObject obj)
        {
            m_Asset = obj as TAsset;
        }

        protected override void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                m_Asset = EditorGUILayout.ObjectField(m_Asset, typeof(TAsset), false) as TAsset;
                OnGUIFindInScene();
            }
            EditorGUILayout.EndHorizontal();

            base.OnGUI();
        }

        protected virtual void OnGUIFindInScene()
        {
            if (m_EnabledFindInScene)
            {
                if (GUILayout.Button("FindInScene", GUILayout.ExpandWidth(false)))
                    OnClickFindInScene();
            }
        }

        protected virtual string GetFindInSceneSearchFilter()
        {
            string searchFilter;

#if UNITY_2019_2_OR_NEWER
            // Don't remove "Assets" prefix, we need to support Packages as well (https://fogbugz.unity3d.com/f/cases/1161019/)
            string path = AssetDatabase.GetAssetPath(m_Asset);
#else
            // only main assets have unique paths (remove "Assets" to make string simpler)
            string path = AssetDatabase.GetAssetPath(m_Asset).Substring(7);
#endif
            if (path.IndexOf(' ') != -1)
                path = '"' + path + '"';

            if (AssetDatabase.IsMainAsset(m_Asset))
                searchFilter = "ref:" + path;
            else
                searchFilter = "ref:" + m_Asset.GetInstanceID() + ":" + path;

            return searchFilter;
        }

        protected virtual void OnClickFindInScene()
        {
            var searchFilter = GetFindInSceneSearchFilter();
            SearchableEditorWindowUtil.ForEach((win) =>
            {
                win.SetSearchFilter(searchFilter, SearchableEditorWindow.SearchMode.All);
            }, HierarchyType.GameObjects);
        }
        
    }

}

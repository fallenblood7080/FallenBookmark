using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Fallen.Bookmark.Editor
{
    [CreateAssetMenu(fileName = "BookmarkData", menuName = "Scriptable Objects/BookmarkData")]
    public class BookmarkData : ScriptableObject
    {
        private const string PACKAGE_ASSET_PATH = "Packages/com.fallen.bookmark/Editor/Bookmark";
        private const string PROJECT_ASSET_PATH = "Assets/Editor/Bookmark";
        private const string ASSET_NAME = "BookmarkData.asset";

        private static string GetAssetPath()
        {
            if (AssetDatabase.IsValidFolder(PACKAGE_ASSET_PATH))
            {
                return PACKAGE_ASSET_PATH;
            }

            if (!AssetDatabase.IsValidFolder("Assets/Editor"))
            {
                AssetDatabase.CreateFolder("Assets", "Editor");
            }

            if (!AssetDatabase.IsValidFolder(PROJECT_ASSET_PATH))
            {
                AssetDatabase.CreateFolder("Assets/Editor", "Bookmark");
            }

            return PROJECT_ASSET_PATH;
        }

        [SerializeField,HideInInspector] private List<Object> items = new();

        public List<Object> GetBookmarkList => items;

        public static BookmarkData GetOrCreate()
        {
            string assetPath = GetAssetPath();
            BookmarkData instance = AssetDatabase.LoadAssetAtPath<BookmarkData>($"{assetPath}/{ASSET_NAME}");

            if (instance == null)
            {
                instance = CreateInstance<BookmarkData>();
                AssetDatabase.CreateAsset(instance, $"{assetPath}/{ASSET_NAME}");
                AssetDatabase.SaveAssets();
            }

            return instance;
        }

        public bool AddBookmark(Object obj)
        {
            if(obj == null)
            {
                Debug.LogError("There is error in adding bookmark");
                return false;
            }
            if (IsBookmarkExist(obj))
            {
                Debug.Log("Bookmark Exist");
                return false;
            }
            Undo.RecordObject(this, "Add Bookmark");
            items.Add(obj);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            return true;
        }

        public void RemoveBookmark(int index)
        {
            if(index < 0 || index >= items.Count)
            {
                return;
            }

            Undo.RecordObject(this, "Remove Bookmark");
            items.RemoveAt(index);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public bool IsBookmarkExist(Object obj)
        {
            return items.Contains(obj);
        }

    }
}

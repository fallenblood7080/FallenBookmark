using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Fallen.Bookmark.Editor
{
    [CreateAssetMenu(fileName = "BookmarkData", menuName = "Scriptable Objects/BookmarkData")]
    public class BookmarkData : ScriptableObject
    {
        private const string PACKAGE_ASSET_PATH = "Packages/com.fallen.bookmark/Editor/Bookmark/BookmarkData.asset";
        private const string PROJECT_FOLDER_PATH = "Assets/Editor/Bookmark";
        private const string PROJECT_ASSET_PATH = "Assets/Editor/Bookmark/BookmarkData.asset";

        [SerializeField, HideInInspector] private List<Object> items = new();

        public List<Object> GetBookmarkList => items;

        public static BookmarkData GetOrCreate()
        {
            // 1. Try loading from the project's Assets folder first
            BookmarkData instance = AssetDatabase.LoadAssetAtPath<BookmarkData>(PROJECT_ASSET_PATH);

            // 2. If not found in Assets, try loading default/bundled data from the Package folder
            if (instance == null)
            {
                instance = AssetDatabase.LoadAssetAtPath<BookmarkData>(PACKAGE_ASSET_PATH);
            }

            // 3. If it still doesn't exist anywhere, create a new asset strictly inside Assets/
            if (instance == null)
            {
                EnsureProjectFolderExists();

                instance = CreateInstance<BookmarkData>();
                AssetDatabase.CreateAsset(instance, PROJECT_ASSET_PATH);
                AssetDatabase.SaveAssets();
            }

            return instance;
        }

        private static void EnsureProjectFolderExists()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Editor"))
            {
                AssetDatabase.CreateFolder("Assets", "Editor");
            }

            if (!AssetDatabase.IsValidFolder(PROJECT_FOLDER_PATH))
            {
                AssetDatabase.CreateFolder("Assets/Editor", "Bookmark");
            }
        }

        public bool AddBookmark(Object obj)
        {
            if (obj == null)
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
            if (index < 0 || index >= items.Count)
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
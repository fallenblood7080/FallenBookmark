using UnityEditor;
using Object = UnityEngine.Object;

namespace Fallen.Bookmark.Editor
{
    public class BookmarkContextMenu
    {
        [MenuItem("Assets/Bookmark")]
        private static void BookmarkSelectedItem()
        {
            Object[] selectedObjects = Selection.objects;
            if (selectedObjects == null || selectedObjects.Length == 0) return;

            BookmarkData data = BookmarkData.GetOrCreate();
            bool addedAny = false;

            foreach (Object obj in selectedObjects)
            {
                if (obj == null) continue;

                if (data.AddBookmark(obj))
                {
                    addedAny = true;
                }
            }
            if (addedAny && EditorWindow.HasOpenInstances<BookmarkWindow>())
            {
                EditorWindow.GetWindow<BookmarkWindow>().RefreshList();
            }
        }

        [MenuItem("Assets/Bookmark", true)]
        private static bool ValidateBookmarkSelectedItem()
        {
            return Selection.objects != null && Selection.objects.Length > 0;
        }
    }
}

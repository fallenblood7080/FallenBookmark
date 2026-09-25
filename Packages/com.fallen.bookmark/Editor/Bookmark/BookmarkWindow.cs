using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fallen.Bookmark.Editor
{
    public class BookmarkWindow : EditorWindow
    {
        private BookmarkData data;
        private ListView listView;

        [MenuItem("Window/Bookmark Window")]
        public static void ShowWindow()
        {
            BookmarkWindow win = GetWindow<BookmarkWindow>();
            win.titleContent = new GUIContent("Bookmarks");
        }

        public void CreateGUI()
        {
            data = BookmarkData.GetOrCreate();

            VisualElement root = rootVisualElement;
            root.style.paddingBottom = 8;
            root.style.paddingLeft = 8;
            root.style.paddingTop = 8;
            root.style.paddingRight = 8;

            listView = new ListView(data.GetBookmarkList, 28, MakeItem, BindItem)
            {
                selectionType = SelectionType.Single,
                reorderable = true,
                style = { flexGrow = 1 }
            };
            

            //listView.selectionChanged += OnItemSelected;
            root.Add(listView);
        }

        public void RefreshList()
        {
            listView?.RefreshItems();
        }

        private VisualElement MakeItem()
        {
            VisualElement container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.style.alignItems = Align.Center;
            container.style.justifyContent = Justify.SpaceBetween;
            container.style.paddingLeft = 6;
            container.style.paddingRight = 6;

            VisualElement infoContainer = new VisualElement();
            infoContainer.style.flexDirection = FlexDirection.Row;
            infoContainer.style.alignItems = Align.Center;
            infoContainer.style.flexGrow = 1;

            Image iconImage = new Image { name = "item-icon" };
            iconImage.style.width = 16;
            iconImage.style.height = 16;
            iconImage.style.marginRight = 6;

            Label label = new Label { name = "item-label" };
            label.style.flexGrow = 1;

            infoContainer.Add(iconImage);
            infoContainer.Add(label);

            Label typeBadge = new Label { name = "type-badge" };
            typeBadge.style.fontSize = 9;
            typeBadge.style.paddingLeft = 5;
            typeBadge.style.paddingRight = 5;
            typeBadge.style.paddingTop = 1;
            typeBadge.style.paddingBottom = 1;
            typeBadge.style.marginRight = 6;
            typeBadge.style.borderTopLeftRadius = 3;
            typeBadge.style.borderTopRightRadius = 3;
            typeBadge.style.borderBottomLeftRadius = 3;
            typeBadge.style.borderBottomRightRadius = 3;
            typeBadge.style.unityFontStyleAndWeight = FontStyle.Bold;

            Button removeBtn = new Button { name = "remove-btn", text = "X" };
            removeBtn.style.width = 20;
            removeBtn.style.height = 20;
            removeBtn.RegisterCallback<ClickEvent>(OnRemoveClicked);

            container.Add(infoContainer);
            container.Add(typeBadge);
            container.Add(removeBtn);

            container.RegisterCallback<PointerEnterEvent>(evt => 
            {
                container.style.backgroundColor = new StyleColor(new Color(1f, 1f, 1f, 0.15f));
            });

            container.RegisterCallback<PointerLeaveEvent>(evt => 
            {
                container.style.backgroundColor = new StyleColor(Color.clear);
            });

            container.RegisterCallback<PointerDownEvent>(OnPointerDownOnItem);

            container.AddManipulator(new ContextualMenuManipulator(evt => PopulateContextMenu(evt, container)));

            return container;
        }

        private void OnPointerDownOnItem(PointerDownEvent evt)
        {
            if(evt.button != 0) return;

            VisualElement container = evt.currentTarget as VisualElement;
            if(container?.userData is Object obj && obj != null)
            {
                EditorGUIUtility.PingObject(obj);
            }
        }

        private void PopulateContextMenu(ContextualMenuPopulateEvent evt, VisualElement container)
        {
            if (container.userData is not Object obj || obj == null) return;

            evt.menu.AppendAction("Ping Item", action =>
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeObject = obj;
            });

            evt.menu.AppendAction("Properties", action =>
            {
                EditorUtility.OpenPropertyEditor(obj);
            });
        }

        private void BindItem(VisualElement element, int index)
        {
            if (index >= data.GetBookmarkList.Count) return;

            Object obj = data.GetBookmarkList[index];

            element.userData = obj;

            Image iconImage = element.Q<Image>("item-icon");
            Label label = element.Q<Label>("item-label");
            Label typeBadge = element.Q<Label>("type-badge");
            Button removeBtn = element.Q<Button>("remove-btn");

            removeBtn.userData = index;

            if (obj != null)
            {
                label.text = obj.name;

                GUIContent content = EditorGUIUtility.ObjectContent(obj, obj.GetType());
                if (content != null && content.image != null)
                {
                    iconImage.image = content.image;
                }

                string path = AssetDatabase.GetAssetPath(obj);
                ApplyTypeBadgeStyle(typeBadge, obj, path);
            }
            else
            {
                label.text = "[Missing]";
                iconImage.image = null;
                typeBadge.text = "MISSING";
                typeBadge.style.backgroundColor = new StyleColor(new Color(0.6f, 0.2f, 0.2f, 0.4f));
                typeBadge.style.color = new StyleColor(Color.white);
            }
        }

        private void ApplyTypeBadgeStyle(Label badge, UnityEngine.Object obj, string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                badge.text = "FOLDER";
                badge.style.backgroundColor = new StyleColor(new Color(0.2f, 0.45f, 0.7f, 0.4f));
                badge.style.color = new StyleColor(new Color(0.6f, 0.85f, 1f));
            }
            else if (obj is MonoScript)
            {
                badge.text = "SCRIPT";
                badge.style.backgroundColor = new StyleColor(new Color(0.2f, 0.6f, 0.3f, 0.4f));
                badge.style.color = new StyleColor(new Color(0.6f, 1f, 0.7f));
            }
            else if (obj is Texture2D or Sprite)
            {
                badge.text = "IMAGE";
                badge.style.backgroundColor = new StyleColor(new Color(0.6f, 0.3f, 0.6f, 0.4f));
                badge.style.color = new StyleColor(new Color(1f, 0.7f, 1f));
            }
            else if (obj is SceneAsset)
            {
                badge.text = "SCENE";
                badge.style.backgroundColor = new StyleColor(new Color(0.7f, 0.35f, 0.2f, 0.4f));
                badge.style.color = new StyleColor(new Color(1f, 0.8f, 0.6f));
            }
            else if (obj is GameObject)
            {
                badge.text = "PREFAB";
                badge.style.backgroundColor = new StyleColor(new Color(0.15f, 0.5f, 0.85f, 0.4f));
                badge.style.color = new StyleColor(new Color(0.7f, 0.9f, 1f));
            }
            else
            {
                badge.text = "ASSET";
                badge.style.backgroundColor = new StyleColor(new Color(0.3f, 0.3f, 0.3f, 0.3f));
                badge.style.color = new StyleColor(new Color(0.8f, 0.8f, 0.8f));
            }
        }

        private void OnRemoveClicked(ClickEvent evt)
        {
            // Prevent event from bubbling up to selection handlers
            evt.StopPropagation();

            Button removeBtn = evt.currentTarget as Button;
            if (removeBtn == null || removeBtn.userData is not int index)
            {
                return;
            }

            if (index < 0 || index >= data.GetBookmarkList.Count)
            {
                return;
            }

            data.RemoveBookmark(index);
            RefreshList();
        }

        // private void OnItemSelected(IEnumerable<object> selectedItems)
        // {
        //     foreach (var item in selectedItems)
        //     {
        //         if (item is Object obj && obj != null)
        //         {
        //             EditorGUIUtility.PingObject(obj);
        //             Selection.activeObject = obj;
        //         }
        //     }
        // }
    }
}
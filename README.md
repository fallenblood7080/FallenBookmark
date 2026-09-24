# Fallen Bookmark

A lightweight Unity editor tool for saving and revisiting your most-used assets, folders, scenes, and scripts.
<img width="409" height="625" alt="image" src="https://github.com/user-attachments/assets/44b1b81c-6b73-4a0b-bd7c-bfb9e39fdd9c" />


## Features

- Add selected assets or folders to a bookmark list
- Open the bookmark window from the Unity menu
- Quickly ping and select bookmarked items
- Remove entries directly from the bookmark list
- Visual badges for common item types such as folders, scenes, scripts, and prefabs


### Installation via Package Manager

1. Open **Window > Package Manager** in Unity.
2. Click the **`+`** icon in the top-left corner.
3. Select **Add package from git URL...**
4. Paste the following URL:

```text
[https://github.com/fallenblood7080/FallenBookmark.git?path=/Packages/com.fallen.bookmark](https://github.com/fallenblood7080/FallenBookmark.git?path=/Packages/com.fallen.bookmark)
```

## Usage

2. Open the bookmark window using:
   - Window > Bookmark Window
3. In the Project window, select one or more assets and use:
   - Assets > Bookmark

## Package Structure

- Editor/Bookmark/
  - BookmarkWindow.cs
  - BookmarkContextMenu.cs
  - BookmarkData.cs
  - Fallen.Bookmark.Editor.asmdef

## Notes

This package is editor-only and is intended for use inside the Unity editor.

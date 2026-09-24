# Fallen Bookmark

A lightweight Unity editor tool for saving and revisiting your most-used assets, folders, scenes, and scripts.
<img width="409" height="625" alt="image" src="https://github.com/user-attachments/assets/44b1b81c-6b73-4a0b-bd7c-bfb9e39fdd9c" />


## Features

- Add selected assets or folders to a bookmark list
- Open the bookmark window from the Unity menu
- Quickly ping and select bookmarked items
- Remove entries directly from the bookmark list
- Visual badges for common item types such as folders, scenes, scripts, and prefabs

## Installation

1. Add this package to your Unity project via the Package Manager using the local path or a git-based package reference.
2. Open the bookmark window using:
   - Window > Bookmark Window
3. In the Project window, select one or more assets and use:
   - Assets > Bookmark

## Usage

- Select any asset, scene, script, or folder in the Project window.
- Use the Assets > Bookmark menu item to add it to your bookmark list.
- Open the bookmark window to view, select, or remove saved entries.
- Clicking a bookmarked item focuses the object and pings it in the editor.

## Package Structure

- Editor/Bookmark/
  - BookmarkWindow.cs
  - BookmarkContextMenu.cs
  - BookmarkData.cs
  - Fallen.Bookmark.Editor.asmdef

## Notes

This package is editor-only and is intended for use inside the Unity editor.

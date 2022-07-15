using Project.Singleton.ConfigurationNS;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Project.Utils.Extension.ObjectNS;

namespace Project.UI.Cursors
{
    /// <summary>
    /// The class is responsible for interacting with the cursor. Works regardless of the scene.
    /// </summary>
    public static class CursorController
    {
        private static List<CustomCursor> _cursors;

        public static CursorType CurrentCursorType { get; private set; }

        private static CursorControl _cursorControl;

        private static CustomCursor _currentCustomCursor = new CustomCursor() { CursorType = CursorType.None};

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            //Input System
            _cursorControl = new CursorControl();
            _cursorControl.Enable();
            _cursorControl.Mouse.LeftClick.started += _ => StartedClick();
            _cursorControl.Mouse.LeftClick.performed += _ => EndedClick();
            //
            _cursors = ConfigurationUI.Instance.Cursors;
            SetCursor(CursorType.Default);
        }

        private static void StartedClick()
        {
            SetCursor(CurrentCursorType, cursorTextureType: CursorTextureType.Clicked);
        }

        private static void EndedClick()
        {
            SetCursor(CurrentCursorType);
        }

        public static void SetCursor(CursorType cursorType, CursorMode cursorMode = CursorMode.Auto, CursorTextureType cursorTextureType = CursorTextureType.Default)
        {
            if (_currentCustomCursor.CursorType != cursorType)
            {
                CurrentCursorType = cursorType;
                _currentCustomCursor = _cursors.FirstOrDefault(x => x.CursorType == cursorType).IsNullExceptionSctruct();
            }
            Cursor.SetCursor(_currentCustomCursor.GetTexture(cursorTextureType), Vector2.zero, cursorMode);
        }
    }
}
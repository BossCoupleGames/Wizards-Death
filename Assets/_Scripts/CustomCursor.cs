using System;
using UnityEngine;

namespace _Scripts
{
    public class CustomCursor: MonoBehaviour
    {

        [SerializeField] private Texture2D cursorTexture2Ds;

        private void Start()
        {
            Cursor.SetCursor(cursorTexture2Ds, new Vector2(10,10),CursorMode.ForceSoftware);
        }
        private void Update()
        {
           
        }
    }
}
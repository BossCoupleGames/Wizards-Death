using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    [System.Serializable]
    public class Dialog
    {
        public Sprite character;
        public string name;
        [TextArea(3, 8)]
        public string[] sentences;
    }
}
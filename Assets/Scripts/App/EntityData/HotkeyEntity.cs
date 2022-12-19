using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.NewApp.Entities
{
    [Serializable]
    public class HotkeyEntity : AbstractEntity
    {
        public int Id = -1;

        public string EventName = "";

        public InputKeyType InputKeyType1 = InputKeyType.None;
        public KeyCode KeyCode1 = KeyCode.None;

        public InputKeyType InputKeyType2 = InputKeyType.None;
        public KeyCode KeyCode2 = KeyCode.None;

        public InputKeyType InputKeyType3 = InputKeyType.None;
        public KeyCode KeyCode3 = KeyCode.None;
    }
}
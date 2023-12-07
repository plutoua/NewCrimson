using System;
using UnityEngine;

namespace TimmyFramework
{
    [Serializable]
    public class GameData
    {
        public int Speed;
        public Vector3 Position;
        public Quaternion Rotation;

        public GameData()
        {
            Speed = 10;
            Position = Vector3.zero;
            Rotation = Quaternion.identity;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class PlayerProfile
    {
        public readonly int ID;
        public readonly DateTime LastLoginDate;

        private int _freeMoney;

        public int FreeMoney
        {
            get { return _freeMoney; }
            set
            {
                _freeMoney = value; 
                OnProfileUpdate?.Invoke();
            }

        }

        public Action OnProfileUpdate;

        public PlayerProfile(int id)
        {
            ID = id;
            LastLoginDate = DateTime.Now;
        }
    }
}

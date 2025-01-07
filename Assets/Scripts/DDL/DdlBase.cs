using UnityEngine;

namespace DDL
{
    public class DdlBase : ScriptableObject
    {
        public int   min;
        public int   max;
        
        public bool InRange(int level)
        {
            return level >= min && level <= max;
        }
    }
}
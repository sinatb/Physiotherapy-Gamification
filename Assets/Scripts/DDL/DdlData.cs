using UnityEngine;
namespace DDL
{
    [CreateAssetMenu(menuName = "DDL/Data", fileName = "DDL Data")]
    public class DdlData : ScriptableObject
    {
        public int   min;
        public int   max;
        public float baseSpeed;
        public float baseSpawnInterval;

        public bool InRange(int level)
        {
            return level >= min && level <= max;
        }
    }   
}

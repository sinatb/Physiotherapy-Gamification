using UnityEngine;
namespace DDL
{
    [CreateAssetMenu(menuName = "DDL/Data", fileName = "DDL Data")]
    public class DdlData : ScriptableObject
    {
        public int   Min;
        public int   Max;
        public float BaseSpeed;
        public float BaseSpawnInterval;

        public bool InRange(int level)
        {
            return level >= Min && level <= Max;
        }
    }   
}

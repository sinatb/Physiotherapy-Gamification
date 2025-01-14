using UnityEngine;

namespace Shoulder_Stretch
{
    public class CanvasPosition : MonoBehaviour
    {
        public GameObject player;

        void Update()
        {
            gameObject.transform.position = player.transform.position;
        }
    }
}
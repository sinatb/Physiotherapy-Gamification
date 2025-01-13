using System;
using Shoulder_Stretch;
using UnityEngine;

namespace Graphics
{
    public class GraphicsManager : MonoBehaviour
    {
        public float                  skyBoxRotationSpeed;
        public Material               roadMaterial;

        private void Update()
        {
            RenderSettings.skybox.SetFloat("_Rotation",Time.time*skyBoxRotationSpeed);
            if (WallSpawner.Instance.IsRunning)
            {
                roadMaterial.mainTextureOffset -= new Vector2(0, WallSpawner.Instance.Speed/26 * Time.deltaTime);
            }
        }
    }
}
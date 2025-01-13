using System;
using UnityEngine;

namespace Graphics
{
    public class GraphicsManager : MonoBehaviour
    {
        public static GraphicsManager Instance;
        public float                  skyBoxRotationSpeed;
        public float                  roadSpeed;
        public Material               roadMaterial;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            RenderSettings.skybox.SetFloat("_Rotation",Time.time*skyBoxRotationSpeed);
            roadMaterial.mainTextureOffset -= new Vector2(0,roadSpeed*Time.deltaTime);
        }
    }
}
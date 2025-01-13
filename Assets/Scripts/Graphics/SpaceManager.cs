using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Graphics
{
    public class SpaceManager : MonoBehaviour
    {
        public float          skyBoxRotationSpeed;
        public List<Material> skyBoxes;

        private void Start()
        {
            RenderSettings.skybox = skyBoxes[Random.Range(0, skyBoxes.Count)];
        }

        private void Update()
        {
            RenderSettings.skybox.SetFloat("_Rotation",Time.time*skyBoxRotationSpeed);
        }
    }
}
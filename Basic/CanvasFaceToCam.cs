using UnityEngine;

namespace SweetCandy.Basic
{
    public class CanvasFaceToCam : MonoBehaviour
    {
        public Camera mainCamera;

        private void Update()
        {
            var rotation = mainCamera.transform.rotation;
            Vector3 targetPos = this.transform.position + rotation * Vector3.forward;
            Vector3 targetOrientation = rotation * Vector3.up;
            this.transform.LookAt(targetPos,targetOrientation);
        }
    }
}

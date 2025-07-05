using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [Tooltip("Точка для слежения (устанавливается из GameView)")]
        public Transform target;

        [Tooltip("Смещение камеры относительно цели")]
        public Vector3 offset = new Vector3(0, 10, -10);

        [Tooltip("Скорость сглаживания движения камеры")]
        public float smoothSpeed = 5f;

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
    }
}
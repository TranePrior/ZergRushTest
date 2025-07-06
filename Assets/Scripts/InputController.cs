using UnityEngine;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        [Tooltip("Ссылка на GameModel")]
        public GameModel model;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                model.currentDir.value = Direction.Up;
            if (Input.GetKeyDown(KeyCode.S))
                model.currentDir.value = Direction.Down;
            if (Input.GetKeyDown(KeyCode.A))
                model.currentDir.value = Direction.Left;
            if (Input.GetKeyDown(KeyCode.D))
                model.currentDir.value = Direction.Right;
        }
    }
}
using UnityEngine;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        [Tooltip("Ссылка на GameModel")]
        public GameModel model;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                model.currentDir.value = Direction.Up;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                model.currentDir.value = Direction.Down;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                model.currentDir.value = Direction.Left;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                model.currentDir.value = Direction.Right;
        }
    }
}
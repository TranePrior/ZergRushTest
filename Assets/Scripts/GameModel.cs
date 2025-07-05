using UnityEngine;
using ZergRush.ReactiveCore;
using System.Collections.Generic;
using UniRx;
using ZergRush;

namespace Assets.Scripts
{
    public enum Direction { Up, Down, Left, Right }

    public class GameModel : MonoBehaviour
    {
        public ZergRush.ReactiveCore.ReactiveCollection<Vector2Int> snakeBody = new ZergRush.ReactiveCore.ReactiveCollection<Vector2Int>();
        public Cell<Direction> currentDir         = new Cell<Direction>(Direction.Right);
        public Cell<Vector2Int> foodPos           = new Cell<Vector2Int>(new Vector2Int(5,5));
        public EventStream<Unit> tick             = new EventStream<Unit>();

        private void Start()
        {
            snakeBody.AddRange(new List<Vector2Int>{
                new Vector2Int(2,0),
                new Vector2Int(1,0),
                new Vector2Int(0,0)
            });
            tick.Subscribe(_ => MoveSnake());
            InvokeRepeating(nameof(EmitTick), 0.5f, 0.5f);
        }

        private void EmitTick() => tick.Send(Unit.Default);

        private void MoveSnake()
        {
            var head = snakeBody[0];
            Vector2Int step = currentDir.value switch {
                Direction.Up    => Vector2Int.up,
                Direction.Down  => Vector2Int.down,
                Direction.Left  => Vector2Int.left,
                Direction.Right => Vector2Int.right,
                _               => Vector2Int.right
            };

            var newHead = head + step;
            snakeBody.Insert(0, newHead);

            if (newHead == foodPos.value)
                RespawnFood();
            else
                snakeBody.RemoveLast();
        }

        private void RespawnFood()
        {
            foodPos.value = new Vector2Int(
                Random.Range(0, 20),
                Random.Range(0, 20)
            );
        }
    }
}
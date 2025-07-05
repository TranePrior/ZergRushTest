using UnityEngine;
using ZergRush.ReactiveCore;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class GameView : MonoBehaviour
    {
        [Tooltip("Ссылка на GameModel")]
        public GameModel model;

        [Tooltip("Префаб сегмента (Cube)")]
        public GameObject segmentPrefab;

        [Tooltip("Префаб еды (Sphere)")]
        public GameObject foodPrefab;

        [Tooltip("Время плавного перехода между клетками")]
        public float moveDuration = 0.5f;

        [Tooltip("Высота сегмента над плоскостью")]
        public float segmentHeight = 0.5f;
        
        private List<GameObject> segments       = new List<GameObject>();
        private List<Vector3>    prevPositions  = new List<Vector3>();
        private List<Vector3>    targetPositions= new List<Vector3>();

        private GameObject foodGO;

        private void Start()
        {
            InitializeSegments();
            DrawFood();
            
            AttachCamera();
            
            model.tick.Subscribe(_ => {
                OnTick();
                AttachCamera();
            });
        }

        private void InitializeSegments()
        {
            segments.Clear();
            prevPositions.Clear();
            targetPositions.Clear();

            foreach (var cell in model.snakeBody)
            {
                Vector3 w = GridToWorld(cell);
                var go = Instantiate(segmentPrefab, w, Quaternion.identity);
                SetRandomColor(go);

                segments.Add(go);
                prevPositions.Add(w);
                targetPositions.Add(w);
            }
        }

        private void OnTick()
        {
            prevPositions = new List<Vector3>(targetPositions);
            
            targetPositions.Clear();
            foreach (var cell in model.snakeBody)
                targetPositions.Add(GridToWorld(cell));
            
            if (model.snakeBody.Count > segments.Count)
            {
                Vector3 spawnPos = prevPositions[prevPositions.Count - 1];
                var go = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);
                SetRandomColor(go);

                segments.Add(go);
                prevPositions.Add(spawnPos);
                targetPositions.Add(GridToWorld(model.snakeBody[^1]));
            }
            
            StopAllCoroutines();
            StartCoroutine(AnimateMovement());
            
            DrawFood();
        }

        private IEnumerator AnimateMovement()
        {
            float elapsed = 0f;
            while (elapsed < moveDuration)
            {
                float t = elapsed / moveDuration;
                for (int i = 0; i < segments.Count; i++)
                    segments[i].transform.position =
                        Vector3.Lerp(prevPositions[i], targetPositions[i], t);

                elapsed += Time.deltaTime;
                yield return null;
            }
            
            for (int i = 0; i < segments.Count; i++)
                segments[i].transform.position = targetPositions[i];
        }

        private void DrawFood()
        {
            if (foodGO != null) Destroy(foodGO);
            Vector2Int f = model.foodPos.value;
            foodGO = Instantiate(foodPrefab, GridToWorld(f), Quaternion.identity);
        }

        private Vector3 GridToWorld(Vector2Int cell)
            => new Vector3(cell.x, segmentHeight, cell.y);

        private void SetRandomColor(GameObject go)
        {
            if (go.TryGetComponent<Renderer>(out var r))
                r.material.color = Random.ColorHSV();
        }

        private void AttachCamera()
        {
            if (segments.Count == 0) return;
            var cam = Camera.main;
            if (cam == null) return;
            if (cam.TryGetComponent<CameraFollow>(out var cf))
                cf.target = segments[0].transform;
        }
    }
}

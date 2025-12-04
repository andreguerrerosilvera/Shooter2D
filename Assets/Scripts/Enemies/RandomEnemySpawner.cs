using UnityEngine;

namespace Enemies
{
    public class RandomEnemySpawner : MonoBehaviour
    {
        [Header("Enemy Prefabs")] [Tooltip("Arrastra aquí todos los prefabs de enemigos que quieres generar")]
        public GameObject[] enemyPrefabs;

        [Header("Spawn Settings")] [Tooltip("Tiempo entre cada generación de enemigos (en segundos)")]
        public float spawnInterval = 5f;

        [Tooltip("Rango horizontal donde pueden aparecer")]
        public float spawnRangeX = 8f;

        [Tooltip("Rango vertical donde pueden aparecer")]
        public float spawnRangeY = 4f;

        [Tooltip("Marcar para generar enemigos infinitamente")]
        public bool spawnInfinite = true;

        [Tooltip("Número máximo de enemigos a generar (si Infinite está desmarcado)")]
        public int maxEnemies = 20;

        [Header("References")] public Transform target;

        public Transform projectileHolder;

        private int enemiesSpawned;
        private float nextSpawnTime;

        private void Start()
        {
            if (target == null)
            {
                var player =
                    GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    target = player.transform;
            }

            if (projectileHolder == null)
                projectileHolder =
                    GameObject.Find("ProjectileHolder")?.transform;
        }

        private void Update()
        {
            // Verificar si es tiempo de generar un enemigo
            if (Time.time >= nextSpawnTime)
                if (spawnInfinite || enemiesSpawned < maxEnemies)
                {
                    SpawnRandomEnemy();
                    nextSpawnTime = Time.time + spawnInterval;
                }
        }

        // Dibujar el área de spawn en el editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnRangeX *
                                                                2, spawnRangeY * 2, 0));
        }

        private void SpawnRandomEnemy()
        {
            // Verificar que haya enemigos configurados
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogWarning("No hay enemigos asignados al spawner!");
                return;
            }

            // Elegir un enemigo aleatorio
            var randomIndex = Random.Range(0, enemyPrefabs.Length);
            var enemyPrefab = enemyPrefabs[randomIndex];

            // Calcular posición aleatoria
            var spawnPosition = transform.position + new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX),
                Random.Range(-spawnRangeY, spawnRangeY),
                0
            );

            // Crear el enemigo
            var newEnemy = Instantiate(enemyPrefab, spawnPosition,
                Quaternion.identity);

            // Configurar el enemigo
            var enemyScript = newEnemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.followTarget = target;
            }

            // Configurar las armas del enemigo
            var shootingControllers = newEnemy.GetComponentsInChildren<ShootingController>();
            foreach (var gun in shootingControllers)
            {
                gun.projectileHolder = projectileHolder;
            }

            enemiesSpawned++;
        }
    }
}
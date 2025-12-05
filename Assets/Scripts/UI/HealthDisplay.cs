using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
///     Este script maneja la visualización de corazones en el UI basándose en la vida del jugador
/// </summary>
public class HealthDisplay : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Referencia al componente Health del jugador")]
    public Health playerHealth;

    [Tooltip("Sprite del corazón lleno")]
    public Sprite heartSprite;

    [Tooltip("Prefab que contiene una Image para representar un corazón")]
    public GameObject heartPrefab;

    [Header("Configuración")]
    [Tooltip("Espacio entre corazones")]
    public float spacing = 10f;

    [Tooltip("Tamaño de cada corazón")]
    public Vector2 heartSize = new Vector2(40f, 40f);

    // Lista de los corazones instanciados
    private List<GameObject> hearts = new List<GameObject>();

    /// <summary>
    ///     Inicializa el display de vida
    /// </summary>
    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("HealthDisplay: No se asignó el componente Health del jugador!");
            return;
        }

        // Si no hay prefab de corazón, crear uno simple
        if (heartPrefab == null)
        {
            CreateDefaultHeartPrefab();
        }

        // Crear los corazones iniciales
        InitializeHearts();

        // Suscribirse al evento de cambio de vida
        if (playerHealth.onHealthChanged != null)
        {
            playerHealth.onHealthChanged.AddListener(OnHealthChanged);
        }
    }

    /// <summary>
    ///     Limpia las suscripciones cuando el objeto se destruye
    /// </summary>
    private void OnDestroy()
    {
        if (playerHealth != null && playerHealth.onHealthChanged != null)
        {
            playerHealth.onHealthChanged.RemoveListener(OnHealthChanged);
        }
    }

    /// <summary>
    ///     Callback que se llama cuando la vida del jugador cambia
    /// </summary>
    /// <param name="newHealth">Nueva cantidad de vida</param>
    private void OnHealthChanged(int newHealth)
    {
        UpdateHearts();
    }

    /// <summary>
    ///     Crea un prefab de corazón por defecto si no se proporciona uno
    /// </summary>
    private void CreateDefaultHeartPrefab()
    {
        heartPrefab = new GameObject("HeartPrefab");
        Image image = heartPrefab.AddComponent<Image>();

        if (heartSprite != null)
        {
            image.sprite = heartSprite;
        }
        else
        {
            // Usar un sprite simple por defecto (cuadrado blanco de Unity)
            image.color = Color.red;
        }

        RectTransform rt = heartPrefab.GetComponent<RectTransform>();
        rt.sizeDelta = heartSize;
    }

    /// <summary>
    ///     Inicializa todos los corazones basándose en la vida máxima
    /// </summary>
    private void InitializeHearts()
    {
        // Limpiar corazones existentes
        foreach (GameObject heart in hearts)
        {
            if (heart != null)
                Destroy(heart);
        }
        hearts.Clear();

        // Crear corazones según la vida máxima
        for (int i = 0; i < playerHealth.maximumHealth; i++)
        {
            CreateHeart(i);
        }

        // Actualizar visibilidad inicial
        UpdateHearts();
    }

    /// <summary>
    ///     Crea un corazón individual
    /// </summary>
    /// <param name="index">Índice del corazón</param>
    private void CreateHeart(int index)
    {
        GameObject heart = Instantiate(heartPrefab, transform);
        heart.name = "Heart_" + index;

        RectTransform rt = heart.GetComponent<RectTransform>();
        if (rt != null)
        {
            // Posicionar el corazón
            float xPos = index * (heartSize.x + spacing);
            rt.anchoredPosition = new Vector2(xPos, 0);
            rt.sizeDelta = heartSize;
        }

        hearts.Add(heart);
    }

    /// <summary>
    ///     Actualiza la visibilidad de los corazones según la vida actual
    /// </summary>
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (hearts[i] != null)
            {
                // Mostrar corazón si el índice es menor que la vida actual
                hearts[i].SetActive(i < playerHealth.currentHealth);
            }
        }
    }

    /// <summary>
    ///     Actualiza el display cuando cambia la vida máxima
    /// </summary>
    public void RefreshDisplay()
    {
        InitializeHearts();
    }
}

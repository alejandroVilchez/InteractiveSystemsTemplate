using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbed : MonoBehaviour
{
    private AudioSource audioSource;
    private Renderer renderer;
    private Color originalColor;
    private Transform playerTransform;
    private bool isHeld = false;
    private Vector3 offset;
    private float timeOverObject = 0f; // Tiempo que el jugador ha estado sobre el objeto
    private float releaseHeight = 1.8f; // Altura a la que se suelta el objeto

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor * 0.75f;
            playerTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Player"))
        {
            renderer.material.color = originalColor;
            playerTransform = null;
            timeOverObject = 0f; // Resetea el contador cuando el jugador sale del objeto
        }
    }

    void Update()
    {
        if (playerTransform != null && !isHeld) // Solo cuenta el tiempo si el objeto no est� siendo agarrado
        {
            timeOverObject += Time.deltaTime; // Incrementa el contador de tiempo
            if (timeOverObject >= 2f) // Si el jugador ha estado sobre el objeto durante 2 segundos
            {
                isHeld = true; // Agarra el objeto
                // Calcula el desplazamiento entre el jugador y el objeto
                offset = transform.position - playerTransform.position;
                timeOverObject = 0f; // Resetea el contador cuando el objeto es agarrado
            }
        }

        if (isHeld)
        {
            // Si el jugador est� a una altura de 1.8 metros, suelta el objeto
            if (playerTransform.position.y >= releaseHeight)
            {
                isHeld = false;
            }
            else
            {
                // Mueve el objeto a la posici�n del jugador m�s el desplazamiento
                Vector3 newPosition = playerTransform.position + offset;
                newPosition.y = transform.position.y; // Mant�n la altura constante
                transform.position = newPosition;
            }
        }
    }
}

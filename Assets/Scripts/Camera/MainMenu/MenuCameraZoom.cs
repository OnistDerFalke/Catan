using System;
using UnityEngine;

public class MenuCameraZoom : MonoBehaviour
{
    [Tooltip("Cel, na który patrzy kamera, przy końcowej animacji.")] [SerializeField]
    private GameObject target;

    [Tooltip("Szybkość przechodzenia kamery w przybliżenie basic-dynamic i dynamic-basic.")]
    [Range(0, 5f)]
    [SerializeField]
    private float dynamicSpeed;

    [Tooltip("Szybkość przechodzenia kamery w przybliżenie przy animacji przejścia do gry.")]
    [Range(0, 5f)]
    [SerializeField]
    private float finalSpeed;

    [Tooltip("Najniższa wysokość, do której może zejść kamera.")] [SerializeField]
    private float minHeight;

    [Tooltip("Najwyższa wysokość, do której może zejść kamera.")] [SerializeField]
    private float maxHeight;

    [Tooltip("Czas po jakim ma wyświetlić się UI dynamiczne.")] [SerializeField]
    public float showDynamicContentUIDelay;

    [Tooltip("Czas po jakim ma wyświetlić się UI bazowe/statyczne.")] [SerializeField]
    public float showBasicContentUIDelay;

    [Tooltip("Czas po jakim ma wyświetlić się UI bazowe/statyczne.")] [SerializeField]
    public float runGameAnimationDelay;

    [Tooltip("Pozycja, na której będzie znajdować się kamera przy rozpoczęciu gry.")] [SerializeField]
    private Vector3 finalPosition;

    [Tooltip("Początkowy tryb przybliżenia.")] [SerializeField]
    private ZoomMode zoomMode = ZoomMode.Stopped;

    public enum ZoomMode
    {
        ZoomIn,
        ZoomOut,
        FinalZoom,
        Stopped
    }

    void Update()
    {
        switch (zoomMode)
        {
            /*Wykonanie ruchu kamery w zależności od ustawionego trybu*/
            case ZoomMode.ZoomIn:
                /*Przybliżanie kamery*/
                if (transform.position.y < minHeight)
                    zoomMode = ZoomMode.Stopped;
                transform.Translate(Vector3.forward * Time.deltaTime * dynamicSpeed);
                break;
            case ZoomMode.ZoomOut:
                /*Oddalanie kamery*/
                if (transform.position.y > maxHeight)
                    zoomMode = ZoomMode.Stopped;
                transform.Translate(-Vector3.forward * Time.deltaTime * dynamicSpeed);
                break;
            case ZoomMode.FinalZoom:
                /*Przejście kamery w pozycję końcową przed rozpoczęciem gry*/
                if (transform.position.y >= finalPosition.y)
                {
                    zoomMode = ZoomMode.Stopped;
                }
                Transform currentTransform;
                (currentTransform = transform).LookAt(target.transform);
                transform.position = Vector3.MoveTowards(currentTransform.position, finalPosition, finalSpeed);
                break;
            case ZoomMode.Stopped:
                /*Kamera pozostaje w bezruchu*/
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetZoomMode(ZoomMode mode)
    {
        zoomMode = mode;
    }
}
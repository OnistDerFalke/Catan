using System;
using UnityEngine;

public class MenuCameraZoom : MonoBehaviour
{
    [Tooltip("Szybkość przechodzenia kamery w przybliżenie.")] 
    [Range(0, 5f)] [SerializeField] private float speed;

    [Tooltip("Najniższa wysokość, do której może zejść kamera.")]
    [SerializeField] private float minHeight;
    
    [Tooltip("Najwyższa wysokość, do której może zejść kamera.")]
    [SerializeField] private float maxHeight;
    
    public enum ZoomMode
    {
        ZoomIn,
        ZoomOut,
        Stopped
    }
    
    [Tooltip("Początkowy tryb przybliżenia.")] 
    [SerializeField] private ZoomMode zoomMode = ZoomMode.Stopped;
    void Update()
    {
        switch (zoomMode)
        {
            case ZoomMode.ZoomIn:
                if (transform.position.y < minHeight) 
                    zoomMode = ZoomMode.Stopped;
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                break;
            case ZoomMode.ZoomOut:
                if (transform.position.y > maxHeight)
                    zoomMode = ZoomMode.Stopped;
                transform.Translate(-Vector3.forward * Time.deltaTime * speed);
                break;
            case ZoomMode.Stopped:
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

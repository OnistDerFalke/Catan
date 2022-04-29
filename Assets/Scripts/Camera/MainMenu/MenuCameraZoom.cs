using System;
using UnityEngine;

namespace Camera.MainMenu
{
    public class MenuCameraZoom : MonoBehaviour
    {
        [Tooltip("Cel kamery")] [SerializeField]
        private GameObject target;

        [Tooltip("Prędkość kamery")]
        [Range(0, 5f)]
        [SerializeField]
        private float dynamicSpeed;

        [Tooltip("Prędkość kamery przy końcowej animacji")]
        [Range(0, 5f)]
        [SerializeField]
        private float finalSpeed;

        [Tooltip("Najniższa wysokość kamery")] [SerializeField]
        private float minHeight;

        [Tooltip("Najwyższa wysokość kamery")] [SerializeField]
        private float maxHeight;

        [Tooltip("Czas do wyswietlenia UI dynamicznego")] [SerializeField]
        public float showDynamicContentUIDelay;

        [Tooltip("Czas do wyswietlenia UI statycznego")] [SerializeField]
        public float showBasicContentUIDelay;

        [Tooltip("Opóźnienie animacji wyświetlania")] [SerializeField]
        public float runGameAnimationDelay;

        [Tooltip("Końcowa pozycja kamery")] [SerializeField]
        private Vector3 finalPosition;

        [Tooltip("Tryb przybliżenia")] [SerializeField]
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
                case ZoomMode.ZoomIn:
                    //Przybliżanie kamery
                    if (transform.position.y < minHeight)
                        zoomMode = ZoomMode.Stopped;
                    transform.Translate(Vector3.forward * Time.deltaTime * dynamicSpeed);
                    break;
                case ZoomMode.ZoomOut:
                    //Oddalanie kamery
                    if (transform.position.y > maxHeight)
                        zoomMode = ZoomMode.Stopped;
                    transform.Translate(-Vector3.forward * Time.deltaTime * dynamicSpeed);
                    break;
                case ZoomMode.FinalZoom:
                    //Przejście kamery w pozycję końcową przed rozpoczęciem gry
                    if (transform.position.y >= finalPosition.y)
                    {
                        zoomMode = ZoomMode.Stopped;
                    }
                    Transform currentTransform;
                    (currentTransform = transform).LookAt(target.transform);
                    transform.position = Vector3.MoveTowards(currentTransform.position, finalPosition, finalSpeed);
                    break;
                case ZoomMode.Stopped:
                    //Kamera pozostaje w bezruchu
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
}
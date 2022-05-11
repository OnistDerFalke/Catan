using System;
using UnityEngine;

namespace Camera.MainMenu
{
    //Destiny: Zooming the camera to the target
    public class MenuCameraZoom : MonoBehaviour
    {
        public enum ZoomMode
        {
            ZoomIn,      
            ZoomOut,    
            FinalZoom,   
            Stopped
        }
        
        //Destiny: Basic camera settings
        [Header("Basic camera settings")][Space(5)]
        [Tooltip("Cel kamery")] [SerializeField]
        private GameObject target;
        
        //Destiny: Advanced camera settings
        [Header("Advanced camera settings")][Space(5)]
        [Tooltip("Dynamic camera movement speed")] [Range(0, 10f)] [SerializeField]
        private float dynamicSpeed;
        [Tooltip("Last animation camera movement speed")] [Range(0, 1f)] [SerializeField]
        private float finalSpeed;
        [Tooltip("Lowest camera position")] [SerializeField]
        private float minHeight;
        [Tooltip("Highest camera position")] [SerializeField]
        private float maxHeight;
        [Tooltip("Końcowa pozycja kamery")] [SerializeField]
        private Vector3 finalPosition;
        [Tooltip("Tryb przybliżenia")] [SerializeField]
        private ZoomMode zoomMode = ZoomMode.Stopped;
        
        //Destiny: UI and animations delays
        [Header("UI and animations delays")][Space(5)]
        [Tooltip("Time to show dynamic UI")] [SerializeField]
        public float showDynamicContentUIDelay;
        [Tooltip("Time to show static UI")] [SerializeField]
        public float showBasicContentUIDelay;
        [Tooltip("Animation delay")] [SerializeField]
        public float runGameAnimationDelay;

        void Update()
        {
            //Destiny: Handle any zoom mode
            switch (zoomMode)
            {
                case ZoomMode.ZoomIn:
                    //Destination: Zooming in the camera to min height
                    if (transform.position.y < minHeight)
                        zoomMode = ZoomMode.Stopped;
                    transform.Translate(Vector3.forward * Time.deltaTime * dynamicSpeed);
                    break;
                case ZoomMode.ZoomOut:
                    //Destination: Zooming out the camera to max height
                    if (transform.position.y > maxHeight)
                        zoomMode = ZoomMode.Stopped;
                    transform.Translate(-Vector3.forward * Time.deltaTime * dynamicSpeed);
                    break;
                case ZoomMode.FinalZoom:
                    //Destination: Zooming camera to the final position
                    if (transform.position.y >= finalPosition.y)
                    {
                        zoomMode = ZoomMode.Stopped;
                    }
                    Transform currentTransform;
                    (currentTransform = transform).LookAt(target.transform);
                    transform.position = Vector3.MoveTowards(currentTransform.position, finalPosition, finalSpeed);
                    break;
                case ZoomMode.Stopped:
                    //Destination: Camera is not moving (zooming)
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
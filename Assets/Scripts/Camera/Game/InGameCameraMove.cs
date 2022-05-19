using System;
using UnityEngine;

namespace Camera.Game
{
    //Destiny: Methods for moving the camera in game
    public class InGameCameraMove : MonoBehaviour
    {
        //Destiny: Basic camera settings
        [Header("Basic camera settings")][Space(5)]
        [Tooltip("Target that camera need to look at")]
        [SerializeField] private GameObject target;
        [Tooltip("Start camera position")]
        [SerializeField] private Vector3 startPosition;
        
        //Destiny: Advanced camera settings
        [Header("Rotation and zoom")][Space(5)]
        [Tooltip("Speed of camera rotation")] [Range(0f, 100f)]
        [SerializeField] private float rotationSpeed;
        [Tooltip("Speed of zooming the camera")] [Range(0f, 100f)]
        [SerializeField] private float zoomSpeed;
        [Tooltip("Vertical zoom multiplier")] [Range(0f, 5f)]
        [SerializeField] private float zoomMultiplier;
        [Tooltip("MaxZoomHeight")]
        [SerializeField] private float maxZoomHeight;
        [Tooltip("MinZoomHeight")]
        [SerializeField] private float minZoomHeight;

        //Destiny: Drag rotation parameters
        private float rotationAngle = 3*Mathf.PI/2;
        private float rotationRadius;
        private Vector3 rotationCenter;

        void Start()
        {
            //Destiny: Camera on start position looks at the target
            transform.position = startPosition;
            transform.LookAt(target.transform);
            
            //Destiny: Camera moving with dragging mouse settings
            rotationCenter = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        }

        /// <summary>
        /// Enough to update every frame for camera position and rotation changes
        /// </summary>
        void LateUpdate()
        {
            
            //Destiny: Radius may update every time on zoom (preventing camera jumps)
            rotationRadius = Mathf.Sqrt(Mathf.Pow(transform.position.x, 2) 
                                        + Mathf.Pow(transform.position.z, 2));
            
            //Destiny: Update the height of the camera (preventing camera jumps)
            rotationCenter.y = transform.position.y;
            
            //Destiny: Camera always looks on target (preventing camera jumps)
            transform.LookAt(target.transform);
            
            //Destiny: Handle drag event on clicking right mouse button based on circle
            if(Input.GetMouseButton(1)) {
                rotationAngle += rotationSpeed * Input.GetAxis("Mouse X");
                var rotationOffset = new Vector3(
                    Mathf.Sin(rotationAngle), 0, Mathf.Cos(rotationAngle)) * rotationRadius;
                transform.position = rotationCenter + rotationOffset;
            }
            
            //Destiny: Handle mouse scroll zooming
            bool canZoomOut, canZoomIn;
            
            //Destiny: Zoom restrictions
            if (transform.position.y > maxZoomHeight)
            {
                canZoomIn = true;
                canZoomOut = false;
            }
            else if (transform.position.y < minZoomHeight)
            {
                canZoomIn = false;
                canZoomOut = true;
            }
            else
            {
                canZoomIn = true;
                canZoomOut = true;
            }

            var scrollMove = Input.GetAxis("Mouse ScrollWheel");
            var zoomOffset = zoomSpeed * scrollMove;
            
            if (scrollMove > 0 && canZoomIn || scrollMove < 0 && canZoomOut)
            {
                //Destiny: Zoom on scroll
                var zoom = Vector3.MoveTowards(
                    transform.position, target.transform.position, zoomOffset);
                zoom = new Vector3(zoom.x, zoom.y - zoomOffset * zoomMultiplier, zoom.z);
                transform.position = zoom;
            }
        }
    }
}

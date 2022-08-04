using UnityEngine;

namespace Camera.Game
{
    //Destiny: Moving the camera in game
    public class InGameCameraMove : MonoBehaviour
    {
        //Destiny: Start position of the camera
        [Header("Start Camera Position")][Space(5)]
        [Tooltip("Start Camera Position")]
        [SerializeField] private Vector3 startPosition;
        
        //Destiny: Settings of camera rotation
        [Header("Camera Rotation Settings")][Space(5)]
        [Tooltip("Camera Holder")] [SerializeField] private new UnityEngine.Camera camera;
        [Tooltip("Target of the Camera")] [SerializeField] private Transform target;
        [Tooltip("Radius Camera Distance from Target")] [SerializeField] private float cameraDistanceFromTarget;
        [Tooltip("Max Rotate Angle (degrees)")] [SerializeField] private float maxRotateAngle; 
        [Tooltip("Min Rotate Angle (degrees)")] [SerializeField] private float minRotateAngle;
        
        //Destiny: Settings of camera zoom
        [Header("Camera Zoom Settings")][Space(5)]
        [Tooltip("Speed of zooming the camera")] [SerializeField] private float zoomSpeed;
        [Tooltip("Max Zoom Radius")] [SerializeField] private float maxZoomRadius; 
        [Tooltip("Min Zoom Height")] [SerializeField] private float minZoomRadius;
        
        //Destiny: Keeps last set position of the camera
        private Vector3 lastPosition;

        void Start()
        {
            //Destiny: Sets camera on start position and camera looks at target
            transform.position = startPosition;
            transform.LookAt(target.transform);
        }
        
        void Update()
        {
            //Destiny: Handles rotation
            HandleSphereRotation();
            
            //Destiny: Handles zoom
            HandleZoom();
            
            //Destiny: After rotation and zoom camera needs to look at target every time
            camera.transform.LookAt(target);
        }

        /// <summary>
        /// Changes camera transform when player wants to rotate around the map
        /// </summary>
        private void HandleSphereRotation()
        {
            //Destiny: Sets new last position
            if (Input.GetMouseButtonDown(1))
                lastPosition = camera.ScreenToViewportPoint(Input.mousePosition);
            
            if (Input.GetMouseButton(1))
            {
                //Destiny: Calculate directory of the move and resets camera position to position of the target
                var dir = lastPosition - camera.ScreenToViewportPoint(Input.mousePosition);
                camera.transform.position = target.position;

                //Destiny: Saves temporary camera location for X axis rotation clamping effect
                var tempCameraRotation = camera.transform.eulerAngles;

                //Destiny: Makes artificial rotations to check if new rotation didn't pass over the clamp limits
                camera.transform.Rotate(new Vector3(1, 0, 0), dir.y * 180);
                camera.transform.Rotate(new Vector3(0, 1, 0), -dir.x * 180, Space.World);
                
                //Destiny: If new rotation exceeded any border, last rotation is set
                if (camera.transform.eulerAngles.x < minRotateAngle) camera.transform.eulerAngles = tempCameraRotation;
                if (camera.transform.eulerAngles.x > maxRotateAngle) camera.transform.eulerAngles = tempCameraRotation;

                //Destiny: Translation and Y rotation always occurs, because restriction is just for X rotation here
                camera.transform.Rotate(new Vector3(0, 1, 0), -dir.x * 180, Space.World);
                camera.transform.Translate(new Vector3(0, 0, -cameraDistanceFromTarget));
                
                //Destiny: Updating last position
                lastPosition = camera.ScreenToViewportPoint(Input.mousePosition);
            }
        }
        
        /// <summary>
        /// Changes camera movement sphere radius when mouse is scrolled
        /// </summary>
        private void HandleZoom()
        {
            //Destiny: Changing the sphere radius
            var scrollMove = Input.GetAxis("Mouse ScrollWheel");
            var radiusOffset = scrollMove * zoomSpeed;
            cameraDistanceFromTarget -= radiusOffset;

            //Destiny: If sphere radius exceeded the limit, it's set to the border value
            if (cameraDistanceFromTarget > maxZoomRadius) cameraDistanceFromTarget = maxZoomRadius;
            if (cameraDistanceFromTarget < minZoomRadius) cameraDistanceFromTarget = minZoomRadius;
            
            //Destiny: Update camera transform if sphere radius changed
            if (radiusOffset != 0)
            {
                //Destiny: Translating camera transform to new sphere radius
                camera.transform.position = target.position;
                camera.transform.Translate(new Vector3(0, 0, -cameraDistanceFromTarget));
            }
        }
    }
}

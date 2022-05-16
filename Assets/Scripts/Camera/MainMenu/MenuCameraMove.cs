using UnityEngine;

namespace Camera.MainMenu
{
    //Destiny: Methods for moving the camera in main menu
    public class MenuCameraMove : MonoBehaviour
    {
        //Destiny: Basic camera settings
        [Header("Basic camera settings")][Space(5)]
        [Tooltip("Target that camera need to look at")]
        [SerializeField] private GameObject target;
        [Tooltip("Camera movement speed")] 
        [Range(0, 1f)] [SerializeField] private float speed;

        //Destiny: If camera is moving or not
        private bool isActive = true;

        void Update()
        {
            if (!isActive) return;
        
            //Destiny: Camera looks at target and moving around it with speed given
            transform.LookAt(target.transform);
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        public void SetActive(bool active)
        {
            isActive = active;
        }
    }
}
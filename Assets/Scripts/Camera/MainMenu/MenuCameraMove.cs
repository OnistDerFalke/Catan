using UnityEngine;

namespace Camera.MainMenu
{
    public class MenuCameraMove : MonoBehaviour
    {
        [Tooltip("Cel kamery")]
        [SerializeField] private GameObject target;

        [Tooltip("Prędkość kamery")] 
        [Range(0, 1f)] [SerializeField] private float speed;

        //Czy ruszać kamerą
        private bool isActive = true;
        void Update()
        {
            if (!isActive) return;
        
            //Ruch kamery
            transform.LookAt(target.transform);
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        public void SetActive(bool active)
        {
            isActive = active;
        }
    }
}
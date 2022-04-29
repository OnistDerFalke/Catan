using UnityEngine;

namespace Camera.Game
{
    public class InGameCameraMove : MonoBehaviour
    {
        [Tooltip("Cel kamery")]
        [SerializeField] private GameObject target;
    
        [Tooltip("Startowa pozycja kamery")]
        [SerializeField] private Vector3 startPosition;
    
        void Start()
        {
            //Kamera w pozycji początkowej wpatrzona w cel
            transform.position = startPosition;
            transform.LookAt(target.transform);
        }
    }
}

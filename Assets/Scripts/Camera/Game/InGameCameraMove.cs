using DataStorage;
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
    
        void Start()
        {
            //Destiny: Camera on start position looks at the target
            transform.position = startPosition;
            transform.LookAt(target.transform);
        }
    }
}

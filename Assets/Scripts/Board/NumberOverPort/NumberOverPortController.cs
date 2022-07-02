using UnityEngine;

namespace Board.NumberOverPort
{
    public class NumberOverPortController : MonoBehaviour
    {
        //Destiny: Player camera as a target for which infos should be visible to
        [Header("Game Camera")][Space(5)]
        [Tooltip("Game Camera")] [SerializeField]
        private GameObject cam;

        void Update()
        {
            transform.LookAt(cam.transform);
            transform.rotation = Quaternion.LookRotation(cam.transform.forward);
        }
    }
}

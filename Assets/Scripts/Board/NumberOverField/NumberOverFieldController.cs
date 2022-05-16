using UnityEngine;

namespace Board.NumberOverField
{
    //Destiny: Controls player always see the numbers over fields in right positions
    public class NumberOverFieldController : MonoBehaviour
    {
        //Destiny: Player camera as a target for which numbers should be visible to
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

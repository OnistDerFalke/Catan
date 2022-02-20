using UnityEngine;

public class MenuCameraMove : MonoBehaviour
{
    [Tooltip("Cel dookoła którego ma obracać się kamera.")]
    [SerializeField] private GameObject target;

    [Tooltip("Szybkość z jaką porusza się kamera wokół celu.")] 
    [Range(0, 1f)] [SerializeField] private float speed;

    private bool isActive = true;
    void Update()
    {
        /*Obracanie kamery wokół celu*/
        if (!isActive) return;
        transform.LookAt(target.transform);
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }
}
using UnityEngine;

public class InGameCameraMove : MonoBehaviour
{
    [Tooltip("Cel, na który patrzy kamera, centrum uwagi kamery w grze.")]
    [SerializeField] private GameObject target;
    
    [Tooltip("Pierwotna pozycja kamery w grze. Zaleca się ustawić identyczne położenie, jak przy zakończeniu" +
             "animacji finalnej w menu głównym, aby uzyskać płynne przejście pomiędzy scenami.")]
    [SerializeField] private Vector3 startPosition;
    
    void Start()
    {
        /*Ustawienie pozycji początkowej kamery w grze i ukierunkowanie rotacji na centrum mapy*/
        transform.position = startPosition;
        transform.LookAt(target.transform);
    }
}

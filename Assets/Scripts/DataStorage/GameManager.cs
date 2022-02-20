using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*Instancja singletonu*/
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }
}
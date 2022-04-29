using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*Instancja singletonu*/
    private static GameManager instance;

    /*Przechowywanie aktualnie wybranego elementu planszy*/
    public BoardElement selectedElement;
    

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        selectedElement = null;
    }
}
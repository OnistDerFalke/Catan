using Board;
using UnityEngine;

namespace DataStorage
{
    public class GameManager : MonoBehaviour
    {
        //Aktualnie wybrany element planszy
        public BoardElement selectedElement;
    

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
            selectedElement = null;
        }
    }
}
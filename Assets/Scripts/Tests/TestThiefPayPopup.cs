using DataStorage;
using Player;
using Random = UnityEngine.Random;

namespace Tests
{
    public class TestThiefPayPopup
    {
        public void Invoke(bool withKnight)
        {
            foreach (var player in GameManager.State.Players)
            {
                player.resources.AddSpecifiedResource(Resources.ResourceType.Clay, Random.Range(0, 4));
                player.resources.AddSpecifiedResource(Resources.ResourceType.Iron, Random.Range(0, 4));
                player.resources.AddSpecifiedResource(Resources.ResourceType.Wheat, Random.Range(0, 4));
                player.resources.AddSpecifiedResource(Resources.ResourceType.Wood, Random.Range(0, 4));
                player.resources.AddSpecifiedResource(Resources.ResourceType.Wool, Random.Range(0, 4));
            }
            GameManager.State.Players[0].MoveThief(withKnight);
        }
    }
}
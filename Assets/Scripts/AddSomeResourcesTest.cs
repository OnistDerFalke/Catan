using System;
using DataStorage;
using UnityEngine;
using Resources = Player.Resources;

[Serializable] public class ResourceList
{
    public int woodNumber;
    public int clayNumber;
    public int woolNumber;
    public int ironNumber;
    public int wheatNumber;
}

public class AddSomeResourcesTest : MonoBehaviour
{
    [SerializeField] private ResourceList[] resourcesToAdd;
    [SerializeField] private bool runTest;
    private void Start()
    {
        if (!runTest) return;
        for (var i = 0; i<GameManager.State.Players.Length; i++)
        {
            GameManager.State.Players[i].resources.
                AddSpecifiedResource(Resources.ResourceType.Clay, resourcesToAdd[i].clayNumber);
            GameManager.State.Players[i].resources.
                AddSpecifiedResource(Resources.ResourceType.Wood, resourcesToAdd[i].woodNumber);
            GameManager.State.Players[i].resources.
                AddSpecifiedResource(Resources.ResourceType.Wool, resourcesToAdd[i].woolNumber);
            GameManager.State.Players[i].resources.
                AddSpecifiedResource(Resources.ResourceType.Iron, resourcesToAdd[i].ironNumber);
            GameManager.State.Players[i].resources.
                AddSpecifiedResource(Resources.ResourceType.Wheat, resourcesToAdd[i].wheatNumber);
        }
    }
}
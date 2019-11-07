using UnityEngine;
using System;
using UnityEngine.Events;


public class Spawnpoint : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    private Action<EventParam> playerDeathEventListener;

    private void Awake()
    {
        playerDeathEventListener = new Action<EventParam>(RespawnPlayer);
        EventManager.AddListener(EventNames.PlayerDeathEvent, playerDeathEventListener);
    }

    void RespawnPlayer(EventParam respawnInfo)
    {
        Instantiate(playerPrefab, this.transform);
    }
}

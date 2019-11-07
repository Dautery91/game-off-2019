using UnityEngine;
using System;
using UnityEngine.Events;


public class Spawnpoint : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    //private Action<EventParam> playerDeathEventListener;

    private void Awake()
    {
        // playerDeathEventListener = new Action<EventParam>(RespawnPlayer);
        // EventManager.AddListener(EventNames.PlayerDeathEvent, playerDeathEventListener);
    }

    public void RespawnPlayer()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if(Player!=null){
            Destroy(Player);
        }
        Instantiate(playerPrefab, this.transform);
    }
}

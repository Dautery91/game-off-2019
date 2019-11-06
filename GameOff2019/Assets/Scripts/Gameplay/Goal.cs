using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Level Finished!");
            EventParam placeHolder = new EventParam();
            EventManager.RaiseEvent(EventNames.LevelFinishedEvent, placeHolder);
        }
    }
}

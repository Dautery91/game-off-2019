using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] VoidGameEvent LevelFinished;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.StopSound("ElecOn");
            }
            LevelFinished.Raise();
        }
    }
}

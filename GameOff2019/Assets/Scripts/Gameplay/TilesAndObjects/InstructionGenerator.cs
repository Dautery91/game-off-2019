using UnityEngine;
using UnityEngine.UI;

public class InstructionGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject InstructionCanvas;

    private void Awake()
    {
        InstructionCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySound("PopUpSFX");
            }

            Time.timeScale = 0;
            InstructionCanvas.SetActive(true);
        }
    }


    public void CloseInstructions()
    {
        Time.timeScale = 1;
        InstructionCanvas.SetActive(false);
    }

}

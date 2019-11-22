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
            AudioManager.instance.PlaySound("PopUpSFX");
            InstructionCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InstructionCanvas.SetActive(false);
        }

    }
}

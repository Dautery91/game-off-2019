using UnityEngine;
using UnityEngine.UI;

public class InstructionGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject InstructionCanvas;

    [SerializeField]
    GameObject OtherGameObject;

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

            if (OtherGameObject != null)
            {
                OtherGameObject.SetActive(true);
            }
            
        }
    }


    public void CloseInstructions()
    {
        Time.timeScale = 1;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        InstructionCanvas.SetActive(false);

        if (OtherGameObject != null)
        {
            OtherGameObject.SetActive(false);
        }

        Destroy(gameObject.GetComponent<InstructionGenerator>());
    }

}

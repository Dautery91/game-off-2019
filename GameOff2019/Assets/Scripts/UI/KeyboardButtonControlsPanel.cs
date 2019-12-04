using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardButtonControlsPanel : MonoBehaviour
{
    [SerializeField] GameObject DefaultButton;
    [SerializeField] GameObject BackButton;
    [SerializeField] GameObject AlternativeDefault;
    private EventSystem ES;

    private void Awake()
    {
        ES = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        if (DefaultButton.activeInHierarchy)
        {
            ES.SetSelectedGameObject(DefaultButton);
        }
        else
        {
            ES.SetSelectedGameObject(AlternativeDefault);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton.GetComponent<Button>().onClick.Invoke();
        }
    }


    private void OnDisable()
    {
        ES.SetSelectedGameObject(ES.firstSelectedGameObject, null);
        
    }
}

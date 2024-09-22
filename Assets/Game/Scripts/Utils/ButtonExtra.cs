using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtra : MonoBehaviour
{
    private Button _thisButton;
    // _audioManager => AudioManager.I;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(MakeSound);
    }

    private void Start()
    {
        _thisButton.targetGraphic.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.8f;
    }

    private void OnEnable()
    {
        _thisButton.interactable = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void MakeSound()
    {
        //_audioManager.PlaySfx("buttonclick");
        _thisButton.interactable = false;
        StartCoroutine(Reset());
    }        

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.1f);
        _thisButton.interactable = true;
    }

}

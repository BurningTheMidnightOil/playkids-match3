using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOverlayUI : Singleton<CanvasOverlayUI>
{
    [SerializeField] GameObject button;
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonUpSprite;

    public IEnumerator ResetNextButtonImage()
    {
        yield return new WaitForSeconds(0.3f);
        buttonImage.sprite = buttonUpSprite;
        button.SetActive(false);
    }
}

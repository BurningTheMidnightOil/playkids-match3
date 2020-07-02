using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOverlayUI : Singleton<CanvasOverlayUI>
{
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject retryButton;
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonUpSprite;

    public IEnumerator ResetNextButtonImage()
    {
        yield return new WaitForSeconds(0.3f);
        buttonImage.sprite = buttonUpSprite;
        nextButton.SetActive(false);
    }

    public IEnumerator ResetRetryButtonImage()
    {
        yield return new WaitForSeconds(0.3f);
        buttonImage.sprite = buttonUpSprite;
        retryButton.SetActive(false);
    }
}

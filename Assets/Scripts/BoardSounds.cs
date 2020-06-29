using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSounds : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip select;
    [SerializeField] AudioClip swap;
    [SerializeField] AudioClip clear;

    // Start is called before the first frame update
    void Start()
    {
        board.onSelect += PlaySelectFX;
        board.onSwap += PlaySwapFX;
        board.onClear += PlayClearFX;
    }

    void PlaySelectFX()
    {
        audioSource.PlayOneShot(select);
    }

    void PlaySwapFX()
    {
        audioSource.PlayOneShot(swap);
    }

    void PlayClearFX()
    {
        audioSource.PlayOneShot(clear);
    }
}

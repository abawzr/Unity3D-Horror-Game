using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCandle;
    [SerializeField]
    private GameObject lightParticle;
    public void Interact()
    {
        if (playerCandle.activeSelf)
        {
            lightParticle.SetActive(true);
        }
    }
}

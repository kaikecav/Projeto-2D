
using UnityEngine;

public class Maleta : MonoBehaviour
{
    public AudioSource somMaleta;

    public void Abrir()
    {
        if (somMaleta != null)
            somMaleta.Play();

        Debug.Log("Maleta aberta!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBtnCtrl : MonoBehaviour
{
    private int indice;
    public GameObject[] ListaAvatars;

    public void CambiarIzquierda()
    {
        ListaAvatars[indice].SetActive(false);

        indice--;
        if (indice < 0)
            indice = ListaAvatars.Length - 1;

        ListaAvatars[indice].SetActive(true);
    }
    public void CambiarDerecha()
    {
        ListaAvatars[indice].SetActive(false);

        indice++;
        if (indice >= ListaAvatars.Length)
            indice = 0;

        ListaAvatars[indice].SetActive(true);
    }

    public void ConfirmarAvatar()
    {
        PlayerPrefs.SetInt("PersonajeSeleccionado", indice);
        Debug.Log("Guardado: " + indice); 
}

}
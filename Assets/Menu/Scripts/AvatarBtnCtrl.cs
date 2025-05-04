using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBtnCtrl : MonoBehaviour
{
    private int indice;
    public GameObject[] ListaAvatars;

    private void OnEnable()
    {
        // Activar el primer avatar al abrir el selector
        indice = 0;

        ListaAvatars[indice].SetActive(true);
    }

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

        // Desactivar el avatar seleccionado después de confirmar
        ListaAvatars[indice].SetActive(false);
    }
}
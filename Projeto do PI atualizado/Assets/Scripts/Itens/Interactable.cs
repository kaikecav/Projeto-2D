using UnityEngine;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour
{
    public ObjectType objectType;       //função que determina o tipo de objeto

    public bool isInteracting;      //função que determina se está interagindo
    public Item conditionalItem;        //função que determina o item como condicional

    public abstract void Interact();        //função que permite o uso desse script
}
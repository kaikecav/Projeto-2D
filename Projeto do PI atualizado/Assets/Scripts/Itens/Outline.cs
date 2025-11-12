using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectType      //cria uma categoria para o tipo de objeto
{
    ground, door, text, dialogue, collectable, none, interactable
}

public class Outline : MonoBehaviour        //permite que outros códigos encontrem essa classificação
{
    public ObjectType objectType;
}

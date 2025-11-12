using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]      //permite criar um asset com as características deste  código
public class Item : ScriptableObject        //ScriptableObject determina que é um objeto modificável
{
    [Header("Identificação única")]     //título no menu da Unity
    public string itemID;       //cria a classe nomeável para o ID do Item

    public string itemName;     //cria a classe nomeável para o nome do Item
    public Sprite itemImage;        //cria a classe que permite colocar a imagem do item
    [TextArea]
    public string description;      //cria a classe nomeável para a descrição do Item
}

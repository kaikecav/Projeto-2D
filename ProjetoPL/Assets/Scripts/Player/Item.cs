using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]      //permite criar um asset com as caracter�sticas deste  c�digo
public class Item : ScriptableObject        //ScriptableObject determina que � um objeto modific�vel
{
    [Header("Identifica��o �nica")]     //t�tulo no menu da Unity
    public string itemID;       //cria a classe nome�vel para o ID do Item

    public string itemName;     //cria a classe nome�vel para o nome do Item
    public Sprite itemImage;        //cria a classe que permite colocar a imagem do item
    [TextArea]
    public string description;      //cria a classe nome�vel para a descri��o do Item
}

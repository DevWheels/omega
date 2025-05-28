using UnityEngine;

public class BookManager : MonoBehaviour
{
    public GameObject book1; 
    public GameObject book2; 
    public GameObject arrow; 

    void Start()
    {
        book1.SetActive(true);
        book2.SetActive(false);
        arrow.SetActive(true);
    }

    public void SwitchBooks()
    {
        bool isBook1Active = book1.activeSelf;
        book1.SetActive(!isBook1Active);
        book2.SetActive(isBook1Active);
        arrow.SetActive(book1.activeSelf); 
    }
}
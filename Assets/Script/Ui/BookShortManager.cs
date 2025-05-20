using UnityEngine;

public class BookShortManager : MonoBehaviour
{
    // Массив всех книг (перетащите их в инспекторе)
    public GameObject[] books;

    void Start()
    {
        // В начале включаем только первую книгу
        OpenBook(0); // Индекс 0 = Book1
    }

    // Метод для открытия книги по индексу (0-3)
    public void OpenBook(int bookIndex)
    {
        // Сначала деактивируем ВСЕ книги
        for (int i = 0; i < books.Length; i++)
        {
            books[i].SetActive(false);
        }

        // Затем включаем нужную
        books[bookIndex].SetActive(true);
    }
}
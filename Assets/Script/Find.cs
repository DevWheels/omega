using UnityEngine;

public class Find
{
    /// <summary>
    /// �������������� �������� ��������� �������� �������, ���� �� ��� ���������� � �� ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T FindUIElement<T>(string name) where T : Component
    {
        T foundElement = null;
        float timeout = 1f; // ������������ ����� �������� � ��������
        float checkInterval = 0.1f; // �������� �������� � ��������
        float elapsedTime = 0f;

        while (foundElement == null && elapsedTime < timeout)
        {
            foundElement = GameObject.Find(name)?.GetComponent<T>();
            elapsedTime += checkInterval;
            System.Threading.Thread.Sleep((int)(checkInterval * 100)); // ��������� ����� ��������� ���������
        }
        return foundElement;
    }

    /// <summary>
    /// ���� ������� �������, �� ����������, ��� name ��� �������� ��������� �������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject FindGameObject(string name)
    {
        GameObject foundObject = null;
        float timeout = 1f; // ������������ ����� �������� � ��������
        float checkInterval = 0.1f; // �������� �������� � ��������
        float elapsedTime = 0f;

        while (foundObject == null && elapsedTime < timeout)
        {
            foundObject = GameObject.Find(name);
            elapsedTime += checkInterval;
            System.Threading.Thread.Sleep((int)(checkInterval * 100)); // ��������� ����� ��������� ���������
        }
        return foundObject;
    }
}

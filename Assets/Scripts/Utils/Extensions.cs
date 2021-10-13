using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void ChangeColor(this Button button, Color color)
    {
        button.GetComponent<Image>().color = color;
    }

    public static int GetRandomNumber(this Vector2Int range)
    {
        return Random.Range(range.x, range.y);
    }
}

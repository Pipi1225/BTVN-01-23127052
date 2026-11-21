using UnityEngine;

public class HandlerB : MonoBehaviour
{
    [Header("Status")]
    public HandlerA handlerA { get; set; }
    [SerializeField] private bool clicked = false;

    void Awake()
    {
        clicked = false;
    }

    void OnMouseDown()
    {
        if (!clicked)
        {
            clicked = true;
            if (handlerA != null)
            {
                handlerA.points += 1;
                handlerA.activeList.Remove(gameObject);
            }

            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

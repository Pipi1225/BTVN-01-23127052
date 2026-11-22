using UnityEngine;

public class HandlerB : MonoBehaviour
{
    public HandlerA handlerA;
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
            if (handlerA != null && handlerA.activeList.Remove(gameObject))
            {
                handlerA.points += 1;
            }

            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

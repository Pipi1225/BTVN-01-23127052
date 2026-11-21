using UnityEngine;

public class HandlerB : MonoBehaviour
{
    [Header("Status")]
    [field: SerializeField] public HandlerA handlerA { get; set; }
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
                handlerA.point += 1;
                //handlerA.activeList.Remove(this.gameObject);
            }

            //Destroy(this.gameObject);
        }
    }
}

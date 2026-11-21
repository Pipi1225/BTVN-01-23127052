using System.Collections.Generic;
using UnityEngine;

public class HandlerA : MonoBehaviour
{
    private const int id = 23127052;
    private const string fullName = "Duong Gia Huy";
    
    [field: Header("Points")]
    [field: SerializeField] public int point { get; set; } = 0;

    [Header("Movement & Force")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float force = 10f;

    [Header("Spawn Object B")]
    [SerializeField] private GameObject prefabB;
    [SerializeField] private Color hoverColor;

    [Header("List Object B")]
    public List<GameObject> activeList = new List<GameObject>();

    void Start()
    {
        // 1
        float randomX = Random.Range(10f, 20f);
        transform.position = new Vector3(0, randomX, 0);
    }

    void Update()
    {
        // 2
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(x, y, 0) * moveSpeed * Time.deltaTime, Space.World);

        // 3
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float maxY = 10f + (id % 10);
            float posX = Random.Range(-10f, 10f);
            float posY = Random.Range(10f, maxY);
            float posZ = Random.Range(-10f, 10f);

            GameObject newB = Instantiate(prefabB, new Vector3(posX, posY, posZ), Quaternion.identity);
            if (newB is not null)
            {
                newB.GetComponent<HandlerB>().handlerA = this;
            }
            
            Vector3 randomForce = new Vector3(Random.Range(-force, force), force, Random.Range(-force, force));
            newB.GetComponent<Rigidbody>()?.AddForce(randomForce);

            activeList.Add(newB);
        }
    }
}

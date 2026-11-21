using System.Collections.Generic;
using UnityEngine;

public class HandlerA : MonoBehaviour
{
    public const int mssv = 23127052;
    public const string fullName = "Duong Gia Huy";
    
    [field: Header("Points")]
    public int point { get; set; } = 0;

    [Header("Movement & Force")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float force = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Status")]
    [SerializeField] private bool isDeleting = false;
    [SerializeField] private float deleteTimer = 0f;
    [SerializeField] private bool isRotating = false;

    [Header("Spawn Object B")]
    [SerializeField] private GameObject prefabB;
    [SerializeField] private Color hoverColor;

    [Header("List Object B")]
    public List<GameObject> activeList = new List<GameObject>();

    void Start()
    {
        isDeleting = false;
        deleteTimer = 0f;
        isRotating = false;

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
            float maxY = 10f + (mssv % 10);
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

        // 4
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isDeleting = true;
            deleteTimer = 0f;
        }

        if (isDeleting)
        {
            activeList.RemoveAll(obj => obj == null);

            if (activeList.Count == 0)
            {
                isDeleting = false;
            }
            else
            {
                deleteTimer += Time.deltaTime;
                if (deleteTimer >= 2f)
                {
                    deleteTimer = 0f;
                    int randomIndex = Random.Range(0, activeList.Count);

                    activeList.RemoveAt(randomIndex);
                    GameObject targetB = activeList[randomIndex];
                    if (targetB != null)
                    {
                        targetB.GetComponent<HandlerB>()?.DestroyObject();
                    }
                }
            }
        }

        // 5
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotating = !isRotating;
        }

        if (isRotating)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        // 6
    }
}

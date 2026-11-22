using System.Collections.Generic;
using UnityEngine;

public class HandlerA : MonoBehaviour
{
    public const int mssv = 23127052;
    public const string fullName = "Duong Gia Huy";
    public int points = 0;

    [Header("Movement & Force")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float force = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    private float autoMoveSpeed = 2 + mssv % 10;

    [Header("Status")]
    [SerializeField] private bool isDeleting = false;
    [SerializeField] private float deleteTimer = 0f;
    [SerializeField] private bool isRotating = false;
    [SerializeField] private bool isObjectBRotating = false;
    [SerializeField] private bool isEating = false;
    
    [Header("Scale Status")]
    [SerializeField] private float scaleMultiplier = 1.5f;
    [SerializeField] private float scaleSpeed = 5f;
    private Vector3 originalScale;


    private enum autoMoving { None, Y, X, Z };

    [Header("Auto Moving Status")]
    [SerializeField] private autoMoving isAutoMoving = autoMoving.None;
    [SerializeField] private Vector3 startPosM;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private int moveDir = 1;

    [Header("Hover Object")]
    [SerializeField] private Color hoverColor;

    [Header("Spawn Object B")]
    [SerializeField] private GameObject prefabB;

    [Header("List Object B")]
    public List<GameObject> activeList = new List<GameObject>();

    private Renderer meshRenderer;

    void Start()
    {
        isDeleting = false;
        deleteTimer = 0f;
        isRotating = false;
        isObjectBRotating = false;
        isEating = false;
        isAutoMoving = autoMoving.None;

        autoMoveSpeed = 2 + mssv % 10;

        meshRenderer = GetComponent<Renderer>();

        // 1
        float randomX = Random.Range(10f, 20f);
        transform.position = new Vector3(0, randomX, 0);

        startPosM = transform.position;
        originalScale = transform.localScale;
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
            if (newB != null)
            {
                newB.GetComponent<HandlerB>().handlerA = this;
                Vector3 randomForce = new Vector3(Random.Range(-force, force), force, Random.Range(-force, force));
                newB.GetComponent<Rigidbody>()?.AddForce(randomForce);

                activeList.Add(newB);
            }
        }

        // 4
        if (Input.GetKeyDown(KeyCode.Q) && !isDeleting)
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

                    GameObject targetB = activeList[randomIndex];
                    activeList.RemoveAt(randomIndex);
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            isObjectBRotating = !isObjectBRotating;

            foreach (GameObject obj in activeList)
            {
                if (obj != null)
                {
                    if (isObjectBRotating)
                    {
                        var rb = obj.GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.isKinematic = true;

                        obj.transform.position = new Vector3(obj.transform.position.x, Random.Range(1f, 11f), obj.transform.position.z);
                    }
                    else
                    {
                        var rb = obj.GetComponent<Rigidbody>();
                        rb.useGravity = true;
                        rb.isKinematic = false;
                    }
                }
            }
        }

        if (isObjectBRotating)
        {
            if (activeList.Count == 0)
            {
                isObjectBRotating = false;
            }
            else
            {
                foreach (GameObject obj in activeList)
                {
                    if (obj != null)
                    {
                        obj.transform.Rotate(new Vector3(1f, 1f, 0.6f) * rotationSpeed * Time.deltaTime);
                    }
                }
            }
        }

        // 7
        if (Input.GetKeyDown(KeyCode.M))
        {
            // isAutoMoving = (autoMoving)(((int)isAutoMoving + 1) % 4);
            if (isAutoMoving == autoMoving.None)
            {
                startPosM = transform.position;
                isAutoMoving = autoMoving.Z;
            }
            else
            {
                isAutoMoving = autoMoving.None;
            }
        }

        if (isAutoMoving != autoMoving.None)
        {
            Vector3 moveDirection = Vector3.zero;
            float currentPosition = 0f;
            float maxPosition = 0f;
            switch (isAutoMoving)
            {
                case autoMoving.X:
                    moveDirection = Vector3.right;
                    currentPosition = transform.position.x;
                    maxPosition = startPosM.x;
                    break;
                case autoMoving.Y:
                    moveDirection = Vector3.up;
                    currentPosition = transform.position.y;
                    maxPosition = startPosM.y;
                    break;
                case autoMoving.Z:
                    moveDirection = Vector3.forward;
                    currentPosition = transform.position.z;
                    maxPosition = startPosM.z;
                    break;
            }

            transform.Translate(moveDirection * autoMoveSpeed * moveDir * Time.deltaTime, Space.World);
            if (currentPosition >= maxPosition + moveDistance)
            {
                moveDir = -1;
            }
            else if (currentPosition <= maxPosition - moveDistance)
            {
                moveDir = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (GameObject obj in activeList)
            {
                if (obj != null)
                {
                    obj.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            isEating = !isEating;
        }

        if (isEating)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale * scaleMultiplier, Time.deltaTime * scaleSpeed);
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale, Time.deltaTime * scaleSpeed);
        }
    }

    // 8
    void OnMouseEnter()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.white;
        }
    }

    // 9

    void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            fontSize = 25
        };
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 300, 60), "Points: " + points, style);
        GUI.Label(new Rect(10, 45, 300, 40), "Eat Mode (V): " + (isEating ? "ON" : "OFF"), style);
    }

    void OnTriggerStay(Collider collision)
    {
        if (!isEating)
        {
            return;
        }

        if (collision.gameObject.CompareTag("ObjectB"))
        {
            if (activeList.Remove(collision.gameObject))
            {
                points += 2;
                collision.gameObject.GetComponent<HandlerB>()?.DestroyObject();
            }
        }
    }
}

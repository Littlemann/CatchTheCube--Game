using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    float speed = 6f;
    public bool isMoving;
    bool onBonus;
    Vector3 firstSize;
    Rigidbody rb;

    
    [SerializeField] AudioClip hitSfx;
    [SerializeField] Slider slider;
    [SerializeField] Vector3 wireCube;
    [SerializeField] GameObject powerLeft, powerRight;

    RaycastHit hit;

    GameManager gameManager;
    LevelController levelController;
    void Start()
    {
        onBonus = false;
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        levelController = FindObjectOfType<LevelController>();
        StartingEvents();
        firstSize = transform.localScale;
    }

    public void StartingEvents()
    {
        powerLeft.SetActive(false);
        powerRight.SetActive(false);
        isMoving = false;
    }

    void SetSlider()
    {
        slider.gameObject.SetActive(true);
        slider.value = rb.velocity.magnitude;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back * -5) * 1000, Color.green);
        if (powerLeft.activeInHierarchy == true)
        {
            powerLeft.transform.Rotate(new Vector3(0, 0, -360) * 1.5f * Time.deltaTime);
            powerRight.transform.Rotate(new Vector3(0, 0, 360) * 1.5f * Time.deltaTime);
        }
        Movement();
        if (gameManager.isLevelFinished)
        {
            Rampa();
        }
    }

    void Movement()
    {
        if (isMoving)
        {
            rb.velocity = Vector3.forward * speed * 1.5f; //Vector3.forward * speed * 50f * Time.deltaTime;,
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position/* + new Vector3(0,0,1.5f)*/, wireCube);
    }

    void Rampa()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back * -5), out hit, Mathf.Infinity))
        {
            SetSlider();
            onBonus = false;
            if (hit.transform.gameObject.CompareTag("Rampa"))
            {
                Quaternion rotasyon = Quaternion.FromToRotation(-transform.forward, hit.normal) * transform.rotation;//raycaste göre rotasyon ayarlanýr
                transform.rotation = Quaternion.Slerp(transform.rotation, rotasyon, Time.deltaTime * 100);
                if(rb.velocity.magnitude <= 25f)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        rb.AddForce(Vector3.forward * 3000000 * Time.deltaTime);
                    }
                    rb.AddForce(Vector3.forward * 60000 * Time.deltaTime);
                }                
            }
            else if (rb.velocity == Vector3.zero)
            {
                
                StartCoroutine(gameManager.OyunSonuAyarlarý());
                slider.gameObject.SetActive(false);
            }
        }
    }
    void ObjeleriFýrlat()
    {

        Collider[] hitColliders = Physics.OverlapBox(transform.position, wireCube);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.tag.Equals("Obje"))
                collider.GetComponent<Rigidbody>().velocity += Vector3.forward * 3f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("CarpismaCollider"))
        {
            powerLeft.SetActive(false);
            powerRight.SetActive(false);
            isMoving = false;
            ObjeleriFýrlat();
            Destroy(other.gameObject);
            rb.velocity = Vector3.zero;
        }

        if (other.gameObject.tag.Equals("PowerUp"))
        {
            powerLeft.SetActive(true);
            powerRight.SetActive(true);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag.Equals("Obje"))
        {
            AudioSource.PlayClipAtPoint(hitSfx,transform.position);
        }

        if (other.gameObject.tag.Equals("Finish"))
        {
            isMoving = false;
            other.gameObject.tag = "Untagged";
            gameManager.isLevelFinished = true;
            levelController.NextLevel();
        }
        if (other.gameObject.tag.Equals("Bonus") && rb.velocity == Vector3.zero)
        {
            if(!onBonus)
            Debug.Log("You gain " + other.gameObject.name + "0 diamonds");
            onBonus = true;
        }
    }

    public void ResetSize()
    {
        transform.localScale = firstSize;
    }
    public void SizeUp()
    {
        isMoving = true;
        transform.localScale = new Vector3(transform.localScale.x * 1.05f, transform.localScale.y, transform.localScale.z * 1.05f);
        transform.position += new Vector3(0, 0.1f, 0);
    }

    public void BackToTheCheckPoint(Transform checkTrans)
    {
        transform.position = checkTrans.localPosition;
    }
}

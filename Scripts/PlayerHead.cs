using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    PlayerInfo info;
    public static Vector3 offset;
    public Material mat;

    //Compoments
    Rigidbody2D rb;
    public Collider2D coll;
    public Collider2D collT;

    public bool isThrown;
    bool isAttaching;
    public bool readyForAction;
    float spcTime;

    AudioSource source;
    public AudioClip attachClip;
    public AudioClip detachClip;
    private void Awake()
    {
        info = FindObjectOfType<PlayerInfo>();
        info.head = this;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        coll.isTrigger = true;
        if (offset == Vector3.zero)
        {
            offset = transform.localPosition;
        }
        mat.SetFloat("_Line", 0f);
        source = GetComponent<AudioSource>();
    }
    private void Start()
    {

    }
    void ReleaseCollider()
    {
        coll.isTrigger = false;
        isThrown = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spcTime = 0.2f;
        }
        if (spcTime > 0f)
        {
            spcTime -= Time.deltaTime;
        }
    }
    public void ThrowHead(Vector2 dir,float speed)
    {
        source.clip = detachClip;
        source.Play();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-5f,5f), ForceMode2D.Impulse);
        transform.parent = null;
        
    }

    IEnumerator Attach()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        mat.SetFloat("_Line", 0f);
        while (Vector3.Distance(transform.localPosition,offset) > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, offset, 6f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 6f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = offset;
        transform.rotation = Quaternion.identity;
        isThrown = false;
        isAttaching = false;
        readyForAction = false;
        source.clip = attachClip;
        source.Play();
    }

    public IEnumerator AttachGiven(Transform newPos)
    {
        rb.velocity = Vector2.zero;
        //rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        while (Vector3.Distance(transform.position, newPos.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position,newPos.position, 8f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.position = newPos.position;
        coll.isTrigger = true;
        transform.parent = newPos;
        mat.SetFloat("_Line", 0f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isAttaching && collision.gameObject.CompareTag("Arms"))
        {
            if (isThrown)
            {
                readyForAction = true;
            }
            if (isThrown)
            {
                mat.SetFloat("_Line", 0.015f);
                if(spcTime > 0f)
                {
                    Debug.Log("ATTACH");
                    isAttaching = true;
                    transform.parent = collision.gameObject.transform;
                    StopAllCoroutines();
                    StartCoroutine(Attach());
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    coll.isTrigger = true;
                }
                
            }

        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.parent == null)
            ReleaseCollider();

        if (collision.gameObject.CompareTag("Arms") && !isAttaching)
        {
            readyForAction = false;
            mat.SetFloat("_Line", 0f);
        }

    }
}

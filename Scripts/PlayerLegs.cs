using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLegs : MonoBehaviour
{
    PlayerInfo info;

    [Header("Movement")]
    public float speed;
    public Vector2 dragConst;
    public Vector2 dragLimits;
    bool enableInput = true;
    bool foundHands;
    bool searchHands;

    BoxCollider2D coll;

    //Compoments
    Rigidbody2D rb;
    Animator anim;

    //Input
    float inputX;

    AudioSource source;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        info = GetComponent<PlayerInfo>();
        info.legs = this;
        info.attachedHands += ChangeBigSize;
        info.attachedHands += ForceFoundHands;
        info.detachedHands += ChangeSmallSize;
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (enableInput && PlayerInfo.enableControl)
            inputX = Input.GetAxisRaw("Horizontal");
        else
        {
            inputX = 0;
        }
    }

    private void FixedUpdate()
    {
        if (inputX != 0)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
            anim.SetBool("Running", true);
            if (inputX < 0f)
            {
                transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            anim.SetBool("Running", false);
        }


        if (Mathf.Abs(rb.velocity.x) > dragLimits.y)
        {
            rb.velocity = new Vector2(rb.velocity.x * dragConst.x, rb.velocity.y);
        }
        else if (Mathf.Abs(rb.velocity.x) > dragLimits.x)
        {
            rb.velocity = new Vector2(rb.velocity.x * dragConst.y, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public bool IsAboveEmpty()
    {
        var hit = Physics2D.Raycast(transform.position + Vector3.right * 0.3f,Vector3.up,0.9f,info.ground);
        var hit1 = Physics2D.Raycast(transform.position + Vector3.right * -0.3f, Vector3.up, 0.9f, info.ground);
        if (hit.collider == null && hit1.collider == null)
        {
            return true;
        }
        return false;
    }


    void ChangeBigSize() { coll.size = new Vector2(0.87f, 1.17f); coll.offset = new Vector2(0, 0.365f); }
    void ChangeSmallSize() { coll.size = new Vector2(0.87f, 0.408f); coll.offset = new Vector2(0f,-0.008f); }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.right * 0.3f, transform.position + Vector3.right * 0.3f + Vector3.up * 0.9f);
        Gizmos.DrawLine(transform.position + Vector3.right * -0.3f, transform.position + Vector3.right * -0.3f + Vector3.up * 0.9f);
    }

    public IEnumerator Attach()
    {
        enableInput = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        searchHands = true;
        while (!foundHands && IsAboveEmpty() == true)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, info.hands.transform.position, 15f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if (IsAboveEmpty() == false && !foundHands)
        {
            enableInput = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            searchHands = true;
            info.hands.isAttaching = false;
        }
        else
        {
            searchHands = false;
            enableInput = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            info.hands.AttachFunc();
            source.Play();
            yield return null;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arms")&& collision.isTrigger == true && searchHands)
        {
            foundHands = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arms") && collision.isTrigger == true && searchHands)
        {
            foundHands = false;
        }
    }

    void ForceFoundHands()
    {
        foundHands = false;
        searchHands = false;
    }
}


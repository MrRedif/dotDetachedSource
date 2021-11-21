using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHands : MonoBehaviour
{
    public Material mat;
    bool inRangeOfSmallConnect;

    [HideInInspector]public PlayerInfo info;
    Vector3 offset;

    [Header("Head")]
    Vector2 startPos;
    Vector2 endPos;
    public bool takeMousePos;
    public GameObject headPrefab;
    public float headSpeedMult;
    public LayerMask headLayer;

    [Header("Movement")]
    public float speed;
    public Vector2 dragConst;
    public Vector2 dragLimits;
    float inputY;
    Rigidbody2D rb;

    bool isDetached;
    public bool isAttaching;
    float spcTime;

    public Collider2D coll;
    //public Collider2D collT;

    AudioSource source;
    public AudioClip attachClip;
    public AudioClip detachClip;
    int first = 0;

    private void Awake()
    {

        info = GetComponentInParent<PlayerInfo>();
        info.hands = this;
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.bodyType = RigidbodyType2D.Kinematic;
        offset = transform.localPosition;
        mat.SetFloat("_Line", 0f);
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        
        StartCoroutine(Attach());
    }

    private void FixedUpdate()
    {
        if (inputY != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputY * speed);

        }

        if (Mathf.Abs(rb.velocity.y) > dragLimits.y)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * dragConst.x);
        }
        else if (Mathf.Abs(rb.velocity.y) > dragLimits.x)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * dragConst.y);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.y,0f);
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDetached && PlayerInfo.enableControl)
        {
            spcTime = 0.25f;
        }
        if (spcTime > 0f)
        {
            spcTime -= Time.deltaTime;
        }

        if(PlayerInfo.enableControl)
        inputY = Input.GetAxisRaw("Vertical");

        if (!isDetached && Input.GetKeyDown(KeyCode.Space) && !info.head.readyForAction && PlayerInfo.enableControl)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector2.up * 20,ForceMode2D.Impulse);
            coll.isTrigger = false;
            transform.parent = null;
            isDetached = true;
            source.clip = detachClip;
            source.Play();
            info.detachedHands?.Invoke();
        }

        if (isDetached && !isAttaching)
        {
            var hit = Physics2D.Raycast(transform.position + Vector3.down * 1.2f + Vector3.right * 0.2f, Vector2.down, 100f,info.lookForLegs);
            var hit1 = Physics2D.Raycast(transform.position + Vector3.down * 1.2f + Vector3.left * 0.2f, Vector2.down, 100f,info.lookForLegs);
            if (hit.collider.gameObject == info.legs.gameObject && hit1.collider.gameObject == info.legs.gameObject)
            {      
                if (Mathf.Abs(transform.position.y - 0.5f - info.legs.transform.position.y) > 0.5f && info.legs.IsAboveEmpty())
                {
                    mat.SetFloat("_Line", 0.015f);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        StartCoroutine(info.legs.Attach());
                        isAttaching = true;
                    }
                }
                
            }
            else if(!inRangeOfSmallConnect)
            {
                mat.SetFloat("_Line", 0.0f);
            }
        }

        if (info.head.isThrown == false)
        {
            ThrowHeadPart();
        }
        else
        {/*
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(info.head.gameObject);
                Instantiate(headPrefab, transform.position + PlayerHead.offset, Quaternion.identity).transform.parent = transform;
            }
            */
        }
    }
    void ThrowHeadPart()
    {

        if (Input.GetMouseButtonDown(0) && takeMousePos == false && PlayerInfo.enableControl)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,10000f,headLayer);
            if (hit.collider != null)
            {

                    Debug.Log("Head");
                    takeMousePos = true;
                    startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

        }
        if (takeMousePos && Input.GetMouseButtonUp(0))
        {
            takeMousePos = false;
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) * new Vector2(1, 1);
            float dist = Vector2.Distance(endPos, startPos);
            Debug.Log(dist);
            float headSpeed = Mathf.Clamp(dist, 0f, 4f);
            if (headSpeed > 1f)
                info.head.ThrowHead((startPos - endPos).normalized,headSpeedMult * headSpeed);
        }
    }

    IEnumerator Attach()
    {
        source.clip = attachClip;
        if (first != 0) source.Play();
        mat.SetFloat("_Line", 0f);
        info.attachedHands?.Invoke();
        while (Vector3.Distance(transform.localPosition, offset) > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, offset, 5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = offset;
        transform.localScale = new Vector3(1f, 1f, 1f);
        isDetached = false;
        isAttaching = false;
        coll.isTrigger = true;
        if (first == 0) first = 1;

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!isAttaching && collision.gameObject.CompareTag("Player"))
        if (isDetached)
        {
                
                if (info.legs.IsAboveEmpty())
                {
                    mat.SetFloat("_Line", 0.015f);
                    inRangeOfSmallConnect = true;
                    if (spcTime > 0)
                    {
                        AttachFunc();
                    }

                }
                else
                {
                    mat.SetFloat("_Line", 0.0f);
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAttaching && collision.gameObject.CompareTag("Player"))
            if (isDetached)
            {
                inRangeOfSmallConnect = false;
                mat.SetFloat("_Line", 0.0f);
            }
    }



    public void AttachFunc()
    {
        isAttaching = true;
        transform.parent = info.legs.transform;
        StopAllCoroutines();
        coll.isTrigger = true;
        StartCoroutine(Attach());
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.down * 1.2f + Vector3.right * 0.2f, transform.position + Vector3.down * 10f + Vector3.right * 0.2f);
        Gizmos.DrawLine(transform.position + Vector3.down * 1.2f + Vector3.left * 0.2f, transform.position + Vector3.down * 10f + Vector3.left * 0.2f);
    }
}

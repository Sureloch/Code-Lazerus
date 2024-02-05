using UnityEngine;

public class BodyPartScript : MonoBehaviour
{
    private bool IsVuneralable = false;
    public Material damage;
    public Material defaultMat;
    public GameObject SnakeHead;
    public int index;
    public int numOfParts;
    public float changeSkinTime;
    public float windowOfOpportunity;
    private float timer;
    private bool slowed = false;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        timer = changeSkinTime;
        SnakeHead = GameObject.Find("SnakeHead(Clone)");
    }
    void RandomSkinChange()
    {
        float flipFactor = Random.Range(0.0f, 1.0f);
        if (flipFactor > 0.49f)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = damage;
            IsVuneralable = true;
        }
        else
        {
            this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
            IsVuneralable = false;
        }
    }
    void ChangeBack()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = defaultMat;
    }
    void CheckHealth()
    {
        if (health <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckHealth();
        if (timer < 0.0f)
        {
            //RandomSkinChange();
            SnakeHead.GetComponent<SnakeBoss>().slowed = true;
            timer = changeSkinTime;
        }
        else
        {
            if (timer < changeSkinTime - windowOfOpportunity)
            {
                //ChangeBack();
                SnakeHead.GetComponent<SnakeBoss>().slowed = false;
            }
            timer -= Time.fixedDeltaTime;
        }

        if (index >= SnakeHead.GetComponent<SnakeBoss>().BodyParts.Count)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            if (!IsVuneralable)
            {

                collision.gameObject.GetComponent<PlayerScript>().TakeDamage(10);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

}

using UnityEngine;

public class KretanjeCevi : MonoBehaviour
{
    public Transform Lopta;
    public AudioClip Clip;
    public AudioClip Muzika;
    public AudioSource As;
    public float YBrzina = 5f;
    public Transform povrsina;
    public Transform platforma;
    public float ubrzanje = 1f;
    bool jesteSmanjiloZivot = false;
    bool prosao = false;
    Camera mainCam;
    private void Start()
    {
        mainCam = Camera.main;
        if (!As.isPlaying)
            DontDestroyOnLoad(As);
        As.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
    }
    private void FixedUpdate()
    {
        if (FindObjectOfType<BGScroll>().jeKameraPala)
        {
            transform.Translate(new Vector3(0f,YBrzina,0f) * Time.fixedDeltaTime * ubrzanje, Space.World);
            Vector2 dno = mainCam.ViewportToScreenPoint(new Vector2(0.5f, 0));
            if (transform.position.y > povrsina.position.y)
            {
                transform.position = dno;//platforma.position;
                int i = Random.Range(0, 3);
                transform.rotation = Quaternion.Euler(0, 0, 90 * i);
                jesteSmanjiloZivot = false;
                prosao = false;
            }
            ubrzanje *= 1.00025f;
            if (!prosao && transform.position.y > Lopta.position.y)
            {
                prosao = true;
                if (Mathf.Round(transform.eulerAngles.z) != 0 && !jesteSmanjiloZivot)
                {
                    FindObjectOfType<LifeMenager>().brZivota--;
                    jesteSmanjiloZivot = true;
                }
            }
        }
    }
    public Collider2D col2D;
    private void Update()
    {
        Vector3 worldPoint;
        Vector2 touchPos;

        if (Input.touchCount == 0) return;
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            touchPos = new Vector2(worldPoint.x, worldPoint.y);
            if (col2D == Physics2D.OverlapPoint(touchPos))
            {
                As.PlayOneShot(Clip);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }     
    }
}

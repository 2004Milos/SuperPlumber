using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public Material material;
    Vector2 offset;
    public bool jeKameraPala = false;
    public float BrzinaX;
    public float BrzinaY;
    public Transform MestoGdePada;
    public Transform Camera;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    Vector2 PrevOffset;
    void Start()
    {
        offset = new Vector2(BrzinaX, BrzinaY);
        PrevOffset = offset;
    }
    void FixedUpdate()
    {
        if (BrzinaY != PrevOffset.y || BrzinaX != PrevOffset.x) {
            offset = new Vector2(BrzinaX, BrzinaY);
            PrevOffset = offset;
        }
        
        if (jeKameraPala)
            material.mainTextureOffset = new Vector2(0, (material.mainTextureOffset + offset * Time.fixedDeltaTime * FindObjectOfType<KretanjeCevi>().ubrzanje).y % material.mainTexture.height) ;
        else if (Mathf.Abs(MestoGdePada.position.y - Camera.position.y) < 2)
            jeKameraPala = true;

        FindObjectOfType<LifeMenager>().Score = Mathf.Abs(material.mainTextureOffset.y);
    }
}

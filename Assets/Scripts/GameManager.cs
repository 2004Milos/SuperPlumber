using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject startpanel;
    [SerializeField] Sprite[] sprites;

    public Hashtable sprht { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        sprht = new Hashtable();
        startpanel.SetActive(true);

        sprht.Add(10, sprites[0]); //PRVA CIFRA: 1 - ima kretanje po x; 0 - nema, ima kretanje po y // DRUGA CIFRA: 0/1 - prva ili druga verzija sprite
        sprht.Add(11, sprites[1]);
        sprht.Add(00, sprites[2]);
        sprht.Add(01, sprites[3]);

        //Zglobovi: (vertikalno -> horizontalno)
        sprht.Add("0110", sprites[4]); //PRVA CIFRA SMER Y + 3, DRUGA IFRA SMER X + 3 //+3 da ne bi doslo do ponavljanja vrednosti kljuca
        sprht.Add("01-10", sprites[5]);
        sprht.Add("0-110", sprites[6]);
        sprht.Add("0-1-10", sprites[7]);

        //Zglobovi horizontalno -> vertikalno
        sprht.Add("-100-1", sprites[4]); //PRVA CIFRA SMER Y + 3, DRUGA IFRA SMER X + 3 //+3 da ne bi doslo do ponavljanja vrednosti kljuca
        sprht.Add("100-1", sprites[5]);
        sprht.Add("-1001", sprites[6]);
        sprht.Add("1001", sprites[7]);
    }

    public void Play()
    {
        startpanel.SetActive(false);

        Camera.main.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

    }
}

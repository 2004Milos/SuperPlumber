using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    Vector2 smer; //levo, desno, gore, dole
    [SerializeField] float brzina;   //brzine scroll-a
    Material material;//material zemlje
    Camera cam;
    GameManager gm;
    public List<GameObject> cevi;
    [SerializeField] GameObject cevpref;

    Vector2 coord; //koordinata za postavljanje novih cevi 
    int verzijaCevi;

    float time;
    bool flagsmer;

    Vector2 sledecismer;
    Vector2 sledecicoord;
    void Awake()
    {
        Debug.Log(Screen.width + " " + Screen.height);
        cam = GameObject.FindGameObjectWithTag("PomocnaKamera").GetComponent<Camera>();
        smer = Vector2.zero;
        OdrediSmer(false, true); //ne sme da se penje na gore, jer bi bilo nerealno
        sledecismer = Vector2.zero;
        sledecicoord = Vector2.zero;

        material = this.GetComponent<Renderer>().material;
        gm = FindObjectOfType<GameManager>();

        //dodavanje pocetne generacije cevi, koja se ispravno stavljene:
        coord = cam.ViewportToWorldPoint((smer + Vector2.one) / 2);
        Vector2 campoint = cam.WorldToViewportPoint(coord);
        campoint = new Vector2((float)Math.Round(campoint.x, 2), (float)Math.Round(campoint.x, 2));
        cevi = new List<GameObject>();


        float cameraHeight = cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * Screen.width / Screen.height; // cameraHeight * aspect ratio

        cevpref.transform.localScale = Vector3.one * cameraHeight / 15f;

        int i = 0;
        while (i < 3)
        {
            cevi.Add(Instantiate(cevpref));
            int j = (int)(Math.Abs(smer.x * 10) + verzijaCevi);
            cevi[cevi.Count - 1].GetComponent<SpriteRenderer>().sprite = (Sprite)gm.sprht[j];

            cevi[cevi.Count - 1].transform.position = coord;
            
            coord += -1*smer * cevi[cevi.Count - 1].GetComponent<SpriteRenderer>().bounds.size*0.95f;
            campoint = cam.WorldToViewportPoint(coord);
            verzijaCevi = Convert.ToInt32(!(verzijaCevi == 1)); //1->0, 0->1
            if (!(campoint.y <= 1 && campoint.y >= 0 && campoint.x <= 1 && campoint.x >= 0))
                i++;
        }


        coord = cam.ViewportToWorldPoint((smer + Vector2.one) / 2); //Vracanje vrednosti na pocetnu
        Destroy(cam.gameObject); //brisanje pomocne kamere pomocu koje se postavljju cevi na pocetni polozaj
        cam = Camera.main;
        verzijaCevi = 1;
    }

    private void Start()
    {
        time = Time.time;
    }

    void Update()
    {
        material.mainTextureOffset += smer * brzina*Time.deltaTime;
        for(int i = 0; i < cevi.Count; i++)
        {
            Vector3 v3 = smer * brzina * Time.deltaTime;
            cevi[i].transform.position -= v3;
        }

        if(brzina < 11)
        brzina *= brzina < 8.3 ? 1.00001f : 1.000004f;

        if (sledecismer != Vector2.zero) //ako je u najavi promena smera, ne dodavati cevi u starom smeru
        {
            if (Math.Round((cam.WorldToViewportPoint(cevi[0].transform.position) * V2Abs(smer)).x,2) == 0.5 || Math.Round((cam.WorldToViewportPoint(cevi[0].transform.position) * V2Abs(smer)).y, 2) == 0.5)
            {
                smer = sledecismer;
                sledecismer = Vector2.zero;
                coord = sledecicoord;
                sledecicoord = Vector2.zero;
            }
        }
        else
        { 
            int poslednja = cevi.Count - 1;
            Vector2 v2 = cam.WorldToViewportPoint(cevi[poslednja].transform.position);
            if (v2.x - cevi[poslednja].transform.localScale.x / 2 > 1 || v2.x + cevi[poslednja].transform.localScale.x / 2 < 0 ||
                v2.y - cevi[poslednja].transform.localScale.y / 2 > 1 || v2.y + cevi[poslednja].transform.localScale.y / 2 < 0) //ako je cev izasla iz vidokruga
            {
                Destroy(cevi[poslednja]);
                cevi.RemoveAt(poslednja);

                if (cevi.Count > 8) return;
                cevi.Insert(0, Instantiate(cevpref));


                int j = (int)(Math.Abs(smer.x * 10) + verzijaCevi);
                cevi[0].GetComponent<SpriteRenderer>().sprite = (Sprite)gm.sprht[j];

                Vector2 pozicijaproslog = cevi[1].transform.position;
                cevi[0].transform.position = pozicijaproslog + smer * cevpref.transform.localScale * 2.5f; //Satviti ga na pocetak od cevi[len-2]

                verzijaCevi = Convert.ToInt32(!(verzijaCevi == 1)); //1->0, 0->1
            }
        }

        if (Time.time - time > 10)
        {
            OdrediSmer(material.mainTextureOffset.y < 0, false);
            time = Time.time;

            cevi.Insert(0, Instantiate(cevpref));
            string key = smer.x + "" + smer.y + "" + sledecismer.x + "" + sledecismer.y;
            cevi[0].GetComponent<SpriteRenderer>().sprite = (Sprite)gm.sprht[key];
            Vector2 pozicijaproslog = cevi[1].transform.position;
            cevi[0].transform.position = pozicijaproslog + smer * cevpref.transform.localScale * 2.3f; //Satviti ga na pocetak od cevi[len-2]

            int i = 0;
            verzijaCevi = 1;
            Vector2 campoint;
            while (i < 8)
            {
                cevi.Insert(0, Instantiate(cevpref));
                int k = (int)(Math.Abs(sledecismer.x * 10) + verzijaCevi);
                cevi[0].GetComponent<SpriteRenderer>().sprite = (Sprite)gm.sprht[k];

                pozicijaproslog = cevi[1].transform.position;
                cevi[0].transform.position = pozicijaproslog + sledecismer * cevpref.transform.localScale * 2.3f; 

                campoint = cam.WorldToViewportPoint(cevi[0].transform.position);
                verzijaCevi = Convert.ToInt32(!(verzijaCevi == 1)); //1->0, 0->1
                if (!(campoint.y <= 1 && campoint.y >= 0 && campoint.x <= 1 && campoint.x >= 0))
                    i++;
            }
        }
    }

    void OdrediSmer(bool smeNaGore, bool primeniodmah)
    {

        if (primeniodmah)
        {
            Vector2 starismer = smer;
            while (Math.Abs(smer.x) == Math.Abs(starismer.x) && Math.Abs(smer.y) == Math.Abs(starismer.y)) //ne sme da ide levo pa desno, vec da skrene za 90 stepeni
            {
                float n = UnityEngine.Random.Range(smeNaGore ? 0 : 1, 4) * (Mathf.PI / 2); //ako sme da se penje, n moze biti 0 jer je cos(0)=1, tj. y smera ce biti pozitivno (na gore)
                smer = new Vector2((float)Math.Round(Mathf.Sin(n)), (float)Math.Round(Mathf.Cos(n))); //random izmedju (0,1), (0,-1), (1,0), (-1,0)
                coord = cam.ViewportToWorldPoint((smer + Vector2.one) / 2); //Update koordinate za postavljenje novih cevi
            }
        }
        else
        {
            sledecismer = smer;
            while (Math.Abs(sledecismer.x) == Math.Abs(smer.x) && Math.Abs(smer.y) == Math.Abs(smer.y)) //ne sme da ide levo pa desno, vec da skrene za 90 stepeni
            {
                float n = UnityEngine.Random.Range(smeNaGore ? 0 : 1, 4) * (Mathf.PI / 2); //ako sme da se penje, n moze biti 0 jer je cos(0)=1, tj. y smera ce biti pozitivno (na gore)
                sledecismer = new Vector2((float)Math.Round(Mathf.Sin(n)), (float)Math.Round(Mathf.Cos(n))); //random izmedju (0,1), (0,-1), (1,0), (-1,0)
                sledecicoord = cam.ViewportToWorldPoint((smer + Vector2.one) / 2); //Update koordinate za postavljenje novih cevi
            }
        }

    }

    Vector2 V2Abs(Vector2 v2)
    {
        return new Vector2(Math.Abs(v2.x), Math.Abs(v2.y));
    }
}

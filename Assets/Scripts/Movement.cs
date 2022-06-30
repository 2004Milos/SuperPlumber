using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    Vector2 smer; //levo, desno, gore, dole
    [SerializeField] float brzina;   //brzine scroll-a
    Material material;//material zemlje
    Camera camera;
    GameManager gm;
    public List<GameObject> cevi;
    [SerializeField] GameObject cevpref;

    Vector2 coord; //koordinata za postavljanje novih cevi 
    int verzijaCevi;

    void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("PomocnaKamera").GetComponent<Camera>();
        OdrediSmer(false); //ne sme da se penje na gore, jer bi bilo nerealno

        material = this.GetComponent<Renderer>().material;
        gm = FindObjectOfType<GameManager>();

        //dodavanje pocetne generacije cevi, koja se ispravno stavljene:
        coord = camera.ViewportToWorldPoint((smer + Vector2.one) / 2);
        Vector2 campoint = camera.WorldToViewportPoint(coord);
        campoint = new Vector2((float)Math.Round(campoint.x, 2), (float)Math.Round(campoint.x, 2));
        cevi = new List<GameObject>();


        float cameraHeight = camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * Screen.width / Screen.height; // cameraHeight * aspect ratio

        cevpref.transform.localScale = Vector3.one * cameraHeight / 15f;

        int i = 0;
        while (i < 2)
        {
            cevi.Add(Instantiate(cevpref));
            int j = (int)(Math.Abs(smer.x * 10) + verzijaCevi);
            cevi[cevi.Count - 1].GetComponent<SpriteRenderer>().sprite = (Sprite)gm.sprht[j];

            cevi[cevi.Count - 1].transform.position = coord;
            
            coord += -1*smer * cevi[cevi.Count - 1].GetComponent<SpriteRenderer>().bounds.size*0.95f;
            campoint = camera.WorldToViewportPoint(coord);
            verzijaCevi = Convert.ToInt32(!(verzijaCevi == 1)); //1->0, 0->1
            if (!(campoint.y <= 1 && campoint.y >= 0 && campoint.x <= 1 && campoint.x >= 0))
                i++;
        }


        coord = camera.ViewportToWorldPoint((smer + Vector2.one) / 2); //Vracanje vrednosti na pocetnu
        Destroy(camera.gameObject); //brisanje pomocne kamere pomocu koje se postavljju cevi na pocetni polozaj
        camera = Camera.main;
        verzijaCevi = 1;
    }


    void Update()
    {
        material.mainTextureOffset += smer * brzina*Time.deltaTime;
        for(int i = 0; i < cevi.Count; i++)
        {
            Vector3 v3 = smer * brzina * Time.deltaTime;
            cevi[i].transform.position -= v3;
        }

        brzina *= 1.00001f;

        int poslednja = cevi.Count - 1;

        Vector2 v2 = camera.WorldToViewportPoint(cevi[poslednja].transform.position);
        if (v2.x - cevi[poslednja].transform.localScale.x / 2 > 1 || v2.x + cevi[poslednja].transform.localScale.x / 2 < 0 ||
            v2.y - cevi[poslednja].transform.localScale.y / 2 > 0.75 || v2.y + cevi[poslednja].transform.localScale.y / 2 < 0) //ako je cev izasla iz vidokruga
        {
            Destroy(cevi[poslednja]);
            cevi.RemoveAt(poslednja);
            
            cevi.Insert(0, Instantiate(cevpref));


            int j = (int)(Math.Abs(smer.x * 10) + verzijaCevi);
            cevi[0].GetComponent<SpriteRenderer>().sprite = (Sprite)gm.sprht[j];

            Vector2 pozicijaproslog = cevi[1].transform.position;
            cevi[0].transform.position = pozicijaproslog + smer * cevpref.transform.localScale*2.5f; //Satviti ga na pocetak od cevi[len-2]

            verzijaCevi = Convert.ToInt32(!(verzijaCevi == 1)); //1->0, 0->1
        }

    }

    void OdrediSmer(bool smeNaGore)
    {
        float n = UnityEngine.Random.Range(smeNaGore ? 0 : 1, 4) * (Mathf.PI / 2); //ako sme da se penje, n moze biti 0 jer je cos(0)=1, tj. y smera ce biti pozitivno (na gore)
        smer = new Vector2((float)Math.Round(Mathf.Sin(n)), (float)Math.Round(Mathf.Cos(n))); //random izmedju (0,1), (0,-1), (1,0), (-1,0)
        coord = camera.ViewportToWorldPoint((smer + Vector2.one) / 2); //Update koordinate za postavljenje novih cevi
    }
}

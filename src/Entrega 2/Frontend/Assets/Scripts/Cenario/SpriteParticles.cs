using System.Collections.Generic;
using UnityEngine;

// Partículas leves feitas com SpriteRenderer (sem depender de shaders extras).
public class SpriteParticles : MonoBehaviour
{
    class P
    {
        public Transform t;
        public SpriteRenderer sr;
        public Vector3 v;
        public float life, max, spin, size, grow, grav, drag;
        public Color c;
        public float baseW;
        public bool alive;
    }

    List<P> pool = new List<P>();
    int capacidade = 300;
    Camera cam;

    public static SpriteParticles Criar(int capacidade)
    {
        GameObject go = new GameObject("Particulas");
        SpriteParticles sp = go.AddComponent<SpriteParticles>();
        sp.capacidade = capacidade;
        return sp;
    }

    void Awake()
    {
        cam = Camera.main;
    }

    P Pegar()
    {
        foreach (P p in pool) if (!p.alive) return p;
        if (pool.Count >= capacidade) return null;

        GameObject go = new GameObject("p");
        go.transform.SetParent(transform, false);
        P np = new P();
        np.t = go.transform;
        np.sr = go.AddComponent<SpriteRenderer>();
        np.sr.sortingOrder = 20;
        np.alive = false;
        go.SetActive(false);
        pool.Add(np);
        return np;
    }

    // Emite n partículas ao redor de pos.
    public void Emitir(Sprite s, Vector3 pos, Color cor, int n, float vel, float tam, float vida, float grav, float espalhar, Vector3 dir)
    {
        if (s == null) return;
        for (int i = 0; i < n; i++)
        {
            P p = Pegar();
            if (p == null) return;

            Vector3 rnd = Random.insideUnitSphere;
            p.v = (dir + rnd * espalhar).normalized * vel * Random.Range(0.45f, 1f);
            p.t.position = pos + rnd * 0.05f;
            p.sr.sprite = s;
            p.baseW = Mathf.Max(0.01f, s.bounds.size.x);
            p.size = tam * Random.Range(0.6f, 1.2f);
            p.grow = -0.3f;
            p.max = vida * Random.Range(0.7f, 1.1f);
            p.life = p.max;
            p.grav = grav;
            p.drag = 1.2f;
            p.spin = Random.Range(-240f, 240f);
            p.c = cor;
            p.alive = true;
            p.t.gameObject.SetActive(true);
            p.t.localScale = Vector3.one * (p.size / p.baseW);
            p.t.rotation = Random.rotation;
            p.sr.color = cor;
        }
    }

    // Partícula que paira (poeira mágica, confete, estrelas): sobe/desce devagar.
    public void EmitirAmbiente(Sprite s, Vector3 pos, Color cor, float tam, float vida, Vector3 vel, float gira)
    {
        if (s == null) return;
        P p = Pegar();
        if (p == null) return;
        p.v = vel;
        p.t.position = pos;
        p.sr.sprite = s;
        p.baseW = Mathf.Max(0.01f, s.bounds.size.x);
        p.size = tam * Random.Range(0.6f, 1.3f);
        p.grow = 0f;
        p.max = vida;
        p.life = vida;
        p.grav = 0f;
        p.drag = 0f;
        p.spin = gira * Random.Range(-1f, 1f);
        p.c = cor;
        p.alive = true;
        p.t.gameObject.SetActive(true);
        p.t.localScale = Vector3.one * (p.size / p.baseW);
        p.t.rotation = Random.rotation;
        p.sr.color = new Color(cor.r, cor.g, cor.b, 0f);
    }

    void Update()
    {
        if (cam == null) cam = Camera.main;
        float dt = Time.deltaTime;

        foreach (P p in pool)
        {
            if (!p.alive) continue;

            p.life -= dt;
            if (p.life <= 0f)
            {
                p.alive = false;
                p.t.gameObject.SetActive(false);
                continue;
            }

            p.v.y -= p.grav * dt;
            if (p.drag > 0f) p.v *= Mathf.Max(0f, 1f - p.drag * dt);
            p.t.position += p.v * dt;

            float k = p.life / p.max;               // 1 -> 0
            float aparece = Mathf.Clamp01((1f - k) * 6f);
            float alpha = Mathf.Clamp01(k * 2f) * aparece;
            Color c = p.c;
            c.a = p.c.a * alpha;
            p.sr.color = c;

            float s = p.size * (1f + p.grow * (1f - k)) / p.baseW;
            p.t.localScale = new Vector3(s, s, s);

            if (cam != null)
            {
                p.t.rotation = cam.transform.rotation * Quaternion.Euler(0f, 0f, p.spin * (p.max - p.life));
            }
        }
    }
}

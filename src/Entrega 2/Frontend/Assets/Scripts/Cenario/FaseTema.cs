using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Dá a "cara" de cada fase: cores, céu, neblina, pós-processamento, cenário 3D extra,
// partículas, brilho nas pistas e reações ao jogo. Tudo é montado em tempo de execução.
public class FaseTema : MonoBehaviour
{
    [Header("0 = descobrir pelo nome da cena (Phase1/2/3)")]
    public int fase = 0;
    public float bpm = 90f;
    public bool esconderUfos = false;   // tira as naves de cenário (fase de doces)

    [Header("Texturas do tema (opcionais)")]
    public Texture2D paleta;     // troca a paleta dos modelos 3D
    public Texture2D chaoTex;
    public Texture2D rioTex;
    public Sprite ceu;

    [Header("Sprites de efeito")]
    public Sprite spGlow;
    public Sprite spEstrela;
    public Sprite spConfete;
    public Sprite spAnel;
    public Sprite spDiamante;

    // ------------------------------------------------------------ presets
    struct Preset
    {
        public Color ambCeu, ambEquador, ambChao, neblina, luz, bloomTint, vinheta, camera;
        public float densNeblina, luzInt, bloom, bloomLimiar, vinhetaInt, saturacao;
        public Color acento1, acento2, acento3;
        public Color margem;
    }

    Preset P;

    static Preset Criar(int f)
    {
        Preset p = new Preset();
        if (f == 2)
        {
            p.ambCeu = new Color(0.9f, 0.45f, 0.6f); p.ambEquador = new Color(0.55f, 0.25f, 0.5f); p.ambChao = new Color(0.18f, 0.1f, 0.35f);
            p.neblina = new Color(0.62f, 0.1f, 0.3f); p.densNeblina = 0.011f;
            p.luz = new Color(1f, 0.88f, 0.92f); p.luzInt = 1.12f;
            p.bloom = 0.7f; p.bloomLimiar = 0.82f; p.bloomTint = new Color(1f, 0.65f, 0.78f);
            p.vinheta = new Color(0.2f, 0f, 0.15f); p.vinhetaInt = 0.3f; p.saturacao = 12f;
            p.camera = new Color(0.45f, 0.03f, 0.2f);
            p.acento1 = new Color(0.95f, 0.1f, 0.3f); p.acento2 = new Color(0.25f, 0.35f, 1f); p.acento3 = new Color(1f, 0.85f, 0.92f);
            p.margem = new Color(0.8f, 0.2f, 0.4f);
        }
        else if (f == 3)
        {
            p.ambCeu = new Color(0.55f, 0.45f, 1f); p.ambEquador = new Color(0.35f, 0.3f, 0.8f); p.ambChao = new Color(0.15f, 0.1f, 0.35f);
            p.neblina = new Color(0.2f, 0.12f, 0.45f); p.densNeblina = 0.014f;
            p.luz = new Color(0.78f, 0.82f, 1f); p.luzInt = 1.05f;
            p.bloom = 1.0f; p.bloomLimiar = 0.7f; p.bloomTint = new Color(0.6f, 0.9f, 1f);
            p.vinheta = new Color(0.05f, 0f, 0.2f); p.vinhetaInt = 0.38f; p.saturacao = 10f;
            p.camera = new Color(0.08f, 0.03f, 0.25f);
            p.acento1 = new Color(0.3f, 0.95f, 1f); p.acento2 = new Color(0.8f, 0.4f, 1f); p.acento3 = new Color(1f, 0.5f, 0.85f);
            p.margem = new Color(0.35f, 0.45f, 1f);
        }
        else
        {
            p.ambCeu = new Color(1f, 0.74f, 0.92f); p.ambEquador = new Color(0.85f, 0.56f, 0.76f); p.ambChao = new Color(0.5f, 0.3f, 0.45f);
            p.neblina = new Color(1f, 0.78f, 0.9f); p.densNeblina = 0.010f;
            p.luz = new Color(1f, 0.93f, 0.95f); p.luzInt = 1.1f;
            p.bloom = 0.55f; p.bloomLimiar = 0.85f; p.bloomTint = new Color(1f, 0.8f, 0.95f);
            p.vinheta = new Color(0.4f, 0.05f, 0.3f); p.vinhetaInt = 0.25f; p.saturacao = 14f;
            p.camera = new Color(0.98f, 0.55f, 0.8f);
            p.acento1 = new Color(1f, 0.4f, 0.75f); p.acento2 = new Color(0.55f, 1f, 0.3f); p.acento3 = new Color(1f, 0.9f, 0.4f);
            p.margem = Color.white;
        }
        return p;
    }

    static readonly Color[] CoresPista =
    {
        new Color(0.62f, 0.15f, 0.95f),   // pista 0 - roxo
        new Color(0.3f, 0.95f, 0.35f),    // pista 1 - verde
        new Color(1f, 0.92f, 0.15f),      // pista 2 - amarelo
        new Color(1f, 0.15f, 0.68f)       // pista 3 - rosa
    };

    // ------------------------------------------------------------ estado
    Camera cam;
    Light luzPrincipal;
    float fovBase, luzBase, punch = 0f;
    SpriteParticles part;
    Dictionary<string, GameObject> modelos = new Dictionary<string, GameObject>();
    HashSet<string> moldesExtras = new HashSet<string> { "floor", "lollypop", "popsicle-chocolate", "candy-bar", "chocolate", "cookie", "cookie-chocolate" };
    Material matBase;
    Bloom bloomVol;
    Vignette vigVol;

    class Pad { public SpriteRenderer pad, faixa; public Vector3 pos; public float pulso; public Color cor; }
    List<Pad> pads = new List<Pad>();
    class Anel { public Transform t; public SpriteRenderer sr; public float tt = 99f, dur = 0.5f, tam = 2f; }
    List<Anel> aneis = new List<Anel>();
    SpriteRenderer aura;
    float ambTimer = 0f;
    Vector2 areaX = new Vector2(-4f, 5f);

    // ------------------------------------------------------------ ciclo
    void Awake()
    {
        if (fase <= 0) fase = GameSession.FaseDaCenaAtual();
        fase = Mathf.Clamp(fase, 1, 3);
        P = Criar(fase);
        cam = Camera.main;
        if (cam == null) cam = FindFirstObjectByType<Camera>();

        ColetarModelos();
        if (esconderUfos) EsconderUfos();
        PegarMaterialBase();
        Ambiente();
        PosProcessamento();
        if (paleta != null) TrocarPaleta(paleta);
        ChaoRioMargem();
        FundoCeu();
        part = SpriteParticles.Criar(420);
        ConstruirCenario();
        ConstruirPistas();
        ConfigurarBoss();

        foreach (string n in moldesExtras)
            if (modelos.ContainsKey(n) && modelos[n] != null) modelos[n].SetActive(false);
    }

    void OnEnable()
    {
        GameEvents.OnLanePressed += AoApertar;
        GameEvents.OnLaneJudged += AoJulgar;
        GameEvents.OnBossDefeated += AoVencer;
    }

    void OnDisable()
    {
        GameEvents.OnLanePressed -= AoApertar;
        GameEvents.OnLaneJudged -= AoJulgar;
        GameEvents.OnBossDefeated -= AoVencer;
    }

    // ------------------------------------------------------------ modelos
    static string LimparNome(string n)
    {
        int i = n.IndexOf(" (");
        return i > 0 ? n.Substring(0, i) : n;
    }

    void ColetarModelos()
    {
        string[] querer = { "snow-tile-hill", "tower-round-crystals", "tower-round-base", "snow-detail-crystal", "snow-detail-rocks",
            "enemy-ufo-c-weapon", "enemy-ufo-d-weapon", "floor", "snow-tile", "lollypop", "popsicle-chocolate", "candy-bar", "chocolate", "cookie", "cookie-chocolate" };
        HashSet<string> set = new HashSet<string>(querer);

        foreach (Transform t in FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            string n = LimparNome(t.name);
            if (!set.Contains(n)) continue;
            if (modelos.ContainsKey(n)) continue;
            if (t.GetComponentInChildren<Renderer>(true) == null) continue;
            modelos[n] = t.gameObject;
        }
    }

    void EsconderUfos()
    {
        foreach (Transform t in FindObjectsByType<Transform>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (t.name.StartsWith("enemy-ufo")) t.gameObject.SetActive(false);
        }
    }

    void PegarMaterialBase()
    {
        foreach (Renderer r in FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (r.name.StartsWith("Chao_Doce") && r.sharedMaterial != null)
            {
                matBase = r.sharedMaterial;
                return;
            }
        }
    }

    Material NovoMat(Color c, float suave)
    {
        Material m;
        if (matBase != null) m = new Material(matBase);
        else m = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (m.HasProperty("_BaseMap")) m.SetTexture("_BaseMap", null);
        m.SetColor("_BaseColor", c);
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", suave);
        if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", 0f);
        return m;
    }

    GameObject Colocar(string modelo, Vector3 pos, float escala, float rotY, Transform pai, Color? tint)
    {
        GameObject tpl;
        if (!modelos.TryGetValue(modelo, out tpl) || tpl == null) return null;

        GameObject go = Instantiate(tpl);
        go.SetActive(true);
        go.name = modelo + "_deco";
        Vector3 sBase = tpl.transform.lossyScale;
        go.transform.SetParent(pai, false);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.Euler(0f, rotY, 0f);
        go.transform.localScale = sBase * escala;

        if (tint.HasValue)
        {
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>())
            {
                Material[] ms = r.materials;
                foreach (Material m in ms)
                    if (m != null && m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", tint.Value);
            }
        }
        return go;
    }

    // ------------------------------------------------------------ ambiente
    void Ambiente()
    {
        RenderSettings.ambientMode = AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = P.ambCeu;
        RenderSettings.ambientEquatorColor = P.ambEquador;
        RenderSettings.ambientGroundColor = P.ambChao;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = P.neblina;
        RenderSettings.fogDensity = P.densNeblina;

        if (cam != null)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = P.camera;
            fovBase = cam.fieldOfView;
        }

        foreach (Light l in FindObjectsByType<Light>(FindObjectsSortMode.None))
        {
            if (l.type == LightType.Directional)
            {
                luzPrincipal = l;
                l.color = P.luz;
                l.intensity = P.luzInt;
                luzBase = P.luzInt;
                l.shadows = LightShadows.Soft;
                break;
            }
        }
    }

    void PosProcessamento()
    {
        if (cam == null) return;
        GameObject go = new GameObject("PosProcessamento");
        Volume vol = go.AddComponent<Volume>();
        vol.isGlobal = true;
        vol.priority = 10f;
        VolumeProfile prof = ScriptableObject.CreateInstance<VolumeProfile>();
        vol.profile = prof;

        bloomVol = prof.Add<Bloom>(true);
        bloomVol.intensity.Override(P.bloom);
        bloomVol.threshold.Override(P.bloomLimiar);
        bloomVol.scatter.Override(0.72f);
        bloomVol.tint.Override(P.bloomTint);

        vigVol = prof.Add<Vignette>(true);
        vigVol.intensity.Override(P.vinhetaInt);
        vigVol.smoothness.Override(0.55f);
        vigVol.color.Override(P.vinheta);

        ColorAdjustments ca = prof.Add<ColorAdjustments>(true);
        ca.saturation.Override(P.saturacao);
        ca.contrast.Override(8f);
        ca.postExposure.Override(0.15f);

        Tonemapping tm = prof.Add<Tonemapping>(true);
        tm.mode.Override(TonemappingMode.Neutral);

        UniversalAdditionalCameraData data = cam.GetUniversalAdditionalCameraData();
        if (data != null) data.renderPostProcessing = true;
    }

    // ------------------------------------------------------------ materiais
    bool EhMolde(Transform t)
    {
        while (t != null)
        {
            string n = LimparNome(t.name);
            if (moldesExtras.Contains(n)) return true;
            t = t.parent;
        }
        return false;
    }

    void TrocarPaleta(Texture2D pal)
    {
        Dictionary<Material, Material> cache = new Dictionary<Material, Material>();
        foreach (Renderer r in FindObjectsByType<MeshRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (EhMolde(r.transform)) continue;
            Material[] ms = r.sharedMaterials;
            bool mudou = false;
            for (int i = 0; i < ms.Length; i++)
            {
                Material m = ms[i];
                if (m == null || !m.HasProperty("_BaseMap")) continue;
                Texture t = m.GetTexture("_BaseMap");
                if (t == null || t.name != "colormap") continue;

                Material inst;
                if (!cache.TryGetValue(m, out inst))
                {
                    inst = new Material(m);
                    inst.SetTexture("_BaseMap", pal);
                    if (inst.HasProperty("_MainTex")) inst.SetTexture("_MainTex", pal);
                    cache[m] = inst;
                }
                ms[i] = inst;
                mudou = true;
            }
            if (mudou) r.sharedMaterials = ms;
        }
    }

    void ChaoRioMargem()
    {
        Renderer chao = null;
        foreach (Renderer r in FindObjectsByType<MeshRenderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            string n = r.name;
            if (n.StartsWith("Chao_Doce"))
            {
                chao = r;
                if (chaoTex != null)
                {
                    Material m = r.material;
                    m.SetTexture("_BaseMap", chaoTex);
                    m.SetTextureScale("_BaseMap", new Vector2(10f, 8f));
                    m.SetColor("_BaseColor", Color.white);
                }
            }
            else if (n.StartsWith("Rio_Chocolate"))
            {
                if (rioTex != null)
                {
                    Material m = r.material;
                    m.SetTexture("_BaseMap", rioTex);
                    m.SetColor("_BaseColor", Color.white);
                }
            }
            else if (n.StartsWith("Margem_Chocolate"))
            {
                if (fase > 1) r.material.SetColor("_BaseColor", P.margem);
            }
        }

        // chão distante: evita o "vazio" no horizonte
        if (chao != null)
        {
            GameObject far = Instantiate(chao.gameObject);
            far.name = "Chao_Distante";
            Vector3 s = chao.transform.localScale;
            far.transform.localScale = new Vector3(s.x * 3f, s.y, s.z * 1.5f);
            float meia = 5f * s.z;
            far.transform.position = new Vector3(chao.transform.position.x, chao.transform.position.y - 0.005f,
                chao.transform.position.z - meia - 5f * s.z * 1.5f);
            Renderer fr = far.GetComponent<Renderer>();
            if (fr != null && chaoTex != null)
            {
                Material m = fr.material;
                m.SetTextureScale("_BaseMap", new Vector2(30f, 12f));
            }
        }
    }

    // ------------------------------------------------------------ céu
    void FundoCeu()
    {
        if (ceu == null) return;
        GameObject go = new GameObject("CeuFundo");
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = ceu;
        sr.sortingOrder = -100;
        float w = ceu.bounds.size.x;
        float escala = 94f / w;
        go.transform.localScale = new Vector3(escala, escala, 1f);
        float h = ceu.bounds.size.y * escala;
        go.transform.position = new Vector3(0.4f, h * 0.5f - 1f, -27f);
        go.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    // ------------------------------------------------------------ cenário 3D
    float R(System.Random rnd, float a, float b) { return a + (float)rnd.NextDouble() * (b - a); }

    void ConstruirCenario()
    {
        System.Random rnd = new System.Random(1000 + fase * 37);
        Transform raiz = new GameObject("Decoracao_Fase" + fase).transform;

        if (fase > 1) ConstruirChaoELaterais(raiz, rnd);

        // 1) horizonte: colinas e torres
        for (int i = 0; i < 24; i++)
        {
            float x = -46f + i * 3.9f + R(rnd, -1f, 1f);
            float z = -17f + R(rnd, -3f, 2.5f);
            if (i % 6 == 3)
            {
                float s = R(rnd, 5f, 7f);
                Colocar("tower-round-base", new Vector3(x, 0f, z), s, R(rnd, 0f, 360f), raiz, null);
                Colocar("tower-round-crystals", new Vector3(x, 0.21f * s + 0.37f * s, z), s, R(rnd, 0f, 360f), raiz, null);
            }
            else
            {
                float s = R(rnd, 5f, 9.5f);
                Colocar("snow-tile-hill", new Vector3(x, 0f, z), s, R(rnd, 0f, 360f), raiz, null);
            }
        }

        // 2) props nos lados da pista (doces gigantes / cristais)
        string[] lista;
        float[] tam;
        if (fase == 1) { lista = new[] { "lollypop", "popsicle-chocolate", "cookie", "chocolate", "candy-bar" }; tam = new[] { 6f, 5f, 7f, 8f, 8f }; }
        else if (fase == 2) { lista = new[] { "lollypop", "candy-bar", "cookie-chocolate", "popsicle-chocolate", "chocolate" }; tam = new[] { 7f, 9f, 7f, 5.5f, 8f }; }
        else { lista = new[] { "snow-detail-crystal", "snow-detail-crystal", "snow-detail-rocks", "tower-round-crystals", "snow-detail-crystal" }; tam = new[] { 5.5f, 7f, 5f, 3f, 4.5f }; }

        Color[] tintes = { P.acento1, P.acento2, P.acento3 };
        for (int i = 0; i < 18; i++)
        {
            int k = i % lista.Length;
            float lado = (i % 2 == 0) ? 1f : -1f;
            float x = lado > 0 ? R(rnd, 6.4f, 11.5f) : R(rnd, -12f, -5.6f);
            float z = R(rnd, -10f, 4.5f);
            Color? tint = null;
            if (fase == 3 && lista[k] != "snow-detail-rocks") tint = Color.Lerp(Color.white, tintes[i % 3], 0.75f);
            float s = tam[k] * R(rnd, 0.8f, 1.25f);
            GameObject g = Colocar(lista[k], new Vector3(x, 0f, z), s, R(rnd, 0f, 360f), raiz, tint);
            if (g != null && fase == 3 && lista[k] == "snow-detail-crystal")
            {
                Flutuar f = g.AddComponent<Flutuar>();
                f.amplitude = 0.12f; f.velocidade = 1.2f; f.giro = 8f;
            }
        }

        // 3) balinhas (esferas brilhantes) espalhadas pelo chão
        for (int i = 0; i < 26; i++)
        {
            GameObject b = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(b.GetComponent<Collider>());
            b.name = "Balinha";
            b.transform.SetParent(raiz, false);
            float r = R(rnd, 0.25f, 0.7f);
            float lado = (i % 2 == 0) ? 1f : -1f;
            float x = lado > 0 ? R(rnd, 5.8f, 12f) : R(rnd, -13f, -5.2f);
            b.transform.position = new Vector3(x, r * 0.75f, R(rnd, -12f, 6f));
            b.transform.localScale = new Vector3(r * 2f, r * 1.7f, r * 2f);
            Color c = Color.Lerp(tintes[i % 3], Color.white, 0.15f);
            b.GetComponent<Renderer>().material = NovoMat(c, 0.9f);
            b.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
        }

        // 4) coisas que flutuam no céu
        if (fase == 3)
        {
            string[] ufos = { "enemy-ufo-c-weapon", "enemy-ufo-d-weapon" };
            for (int i = 0; i < 5; i++)
            {
                GameObject g = Colocar(ufos[i % 2], new Vector3(R(rnd, -14f, 14f), R(rnd, 4.5f, 8f), R(rnd, -18f, -6f)), R(rnd, 3f, 4.5f), R(rnd, 0f, 360f), raiz, null);
                if (g != null)
                {
                    Flutuar f = g.AddComponent<Flutuar>();
                    f.amplitude = 0.45f; f.velocidade = R(rnd, 0.8f, 1.6f); f.giro = R(rnd, 25f, 70f);
                    f.deriva = new Vector3(R(rnd, 2.5f, 5f), 0f, 0f);
                }
            }
        }

        for (int i = 0; i < 7; i++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(o.GetComponent<Collider>());
            o.name = fase == 3 ? "Planetinha" : "Balao";
            o.transform.SetParent(raiz, false);
            float r = R(rnd, 0.7f, fase == 3 ? 2.0f : 1.3f);
            o.transform.position = new Vector3(R(rnd, -15f, 16f), R(rnd, 3.5f, 8.5f), R(rnd, -22f, -9f));
            o.transform.localScale = Vector3.one * r * 2f;
            Color c = Color.Lerp(tintes[i % 3], Color.white, 0.1f);
            o.GetComponent<Renderer>().material = NovoMat(c, 0.95f);
            o.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            Flutuar f = o.AddComponent<Flutuar>();
            f.amplitude = R(rnd, 0.25f, 0.7f); f.velocidade = R(rnd, 0.5f, 1.1f); f.giro = 10f;
        }
    }

    // chão da fase (floor.fbx de cada fase) e margens elevadas com enfeites ao redor da pista
    void ConstruirChaoELaterais(Transform raiz, System.Random rnd)
    {
        Transform chaoRaiz = new GameObject("ChaoFase" + fase).transform;
        chaoRaiz.SetParent(raiz, false);
        for (int ix = -8; ix <= 9; ix++)
        {
            for (int iz = -3; iz <= 13; iz++)
            {
                Colocar("floor", new Vector3(ix + 0.45f, 0f, iz + 0.5f), 1f, 0f, chaoRaiz, null);
            }
        }

        Color[] tintes = { P.acento1, P.acento2, P.acento3 };
        string[] enfeites = (fase == 2)
            ? new[] { "lollypop", "candy-bar", "cookie-chocolate", "chocolate" }
            : new[] { "snow-detail-crystal", "snow-detail-crystal", "snow-detail-rocks", "snow-detail-crystal" };
        float[] tam = (fase == 2) ? new[] { 3.4f, 4.5f, 3.6f, 4f } : new[] { 3f, 3.6f, 2.6f, 2.4f };

        for (int lado = -1; lado <= 1; lado += 2)
        {
            float xBorda = lado > 0 ? 6.5f : -5.7f;
            for (int iz = 2; iz <= 13; iz++)
            {
                Colocar("snow-tile", new Vector3(xBorda + R(rnd, -0.05f, 0.05f), 0f, iz + 0.5f), 1f, 0f, chaoRaiz, null);
                if (iz % 2 == 0)
                    Colocar("snow-tile", new Vector3(xBorda + lado * 1.0f, 0f, iz + 0.5f), 1f, 0f, chaoRaiz, null);
            }
            for (int i = 0; i < 7; i++)
            {
                int k = (i + (lado > 0 ? 0 : 2)) % enfeites.Length;
                float x = xBorda + lado * R(rnd, 0.0f, 2.6f);
                float z = 2.6f + i * 1.55f + R(rnd, -0.3f, 0.3f);
                Color? tint = null;
                if (fase == 3 && enfeites[k] != "snow-detail-rocks") tint = Color.Lerp(Color.white, tintes[(i + 1) % 3], 0.75f);
                GameObject g = Colocar(enfeites[k], new Vector3(x, 0.2f, z), tam[k] * R(rnd, 0.8f, 1.15f), R(rnd, 0f, 360f), chaoRaiz, tint);
                if (g != null && fase == 3)
                {
                    Flutuar f = g.AddComponent<Flutuar>();
                    f.amplitude = 0.05f; f.velocidade = 1.4f; f.giro = 6f;
                }
            }
        }
    }

    // ------------------------------------------------------------ pistas
    void ConstruirPistas()
    {
        NoteSpawner sp = FindFirstObjectByType<NoteSpawner>();
        TeclaPista[] teclas = FindObjectsByType<TeclaPista>(FindObjectsSortMode.None);
        if (sp == null || teclas.Length == 0 || spGlow == null) return;

        Transform raiz = new GameObject("PistasFX").transform;
        foreach (TeclaPista t in teclas)
        {
            Renderer rr = t.GetComponent<Renderer>();
            float topo = rr != null ? rr.bounds.max.y : t.transform.position.y;
            int lane = 0; float melhor = 999f;
            for (int i = 0; i < sp.laneX.Length; i++)
            {
                float d = Mathf.Abs(sp.laneX[i] - t.transform.position.x);
                if (d < melhor) { melhor = d; lane = i; }
            }
            Color c = CoresPista[Mathf.Clamp(lane, 0, 3)];

            Pad p = new Pad();
            p.cor = c;
            p.pos = new Vector3(t.transform.position.x, topo + 0.03f, t.transform.position.z);

            GameObject g = new GameObject("Pad" + lane);
            g.transform.SetParent(raiz, false);
            g.transform.position = p.pos;
            g.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            p.pad = g.AddComponent<SpriteRenderer>();
            p.pad.sprite = spGlow;
            p.pad.sortingOrder = 5;
            g.transform.localScale = Vector3.one * (1.9f / spGlow.bounds.size.x);

            GameObject f = new GameObject("Faixa" + lane);
            f.transform.SetParent(raiz, false);
            float zIni = sp.spawnZ, zFim = p.pos.z;
            f.transform.position = new Vector3(p.pos.x, topo + 0.015f, (zIni + zFim) * 0.5f);
            f.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            p.faixa = f.AddComponent<SpriteRenderer>();
            p.faixa.sprite = spGlow;
            p.faixa.sortingOrder = 4;
            float sw = spGlow.bounds.size.x;
            f.transform.localScale = new Vector3(0.62f / sw, (zFim - zIni) * 1.05f / sw, 1f);

            pads.Add(p);
            pads[pads.Count - 1].pulso = 0f;
            padLane[pads.Count - 1] = lane;
        }

        // anéis pré-criados
        for (int i = 0; i < 10; i++)
        {
            GameObject g = new GameObject("Anel");
            g.transform.SetParent(raiz, false);
            g.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            Anel a = new Anel();
            a.t = g.transform;
            a.sr = g.AddComponent<SpriteRenderer>();
            a.sr.sprite = spAnel != null ? spAnel : spGlow;
            a.sr.sortingOrder = 6;
            g.SetActive(false);
            aneis.Add(a);
        }
    }

    int[] padLane = new int[8];

    Pad PadDaPista(int lane)
    {
        for (int i = 0; i < pads.Count; i++) if (padLane[i] == lane) return pads[i];
        return null;
    }

    void NovoAnel(Vector3 pos, Color c, float tam)
    {
        foreach (Anel a in aneis)
        {
            if (a.tt < a.dur) continue;
            a.tt = 0f; a.dur = 0.55f; a.tam = tam;
            a.t.position = pos + Vector3.up * 0.02f;
            a.t.gameObject.SetActive(true);
            a.sr.color = c;
            return;
        }
    }

    // ------------------------------------------------------------ boss
    void ConfigurarBoss()
    {
        foreach (SpriteRenderer sr in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            if (sr.sprite == null || !sr.sprite.name.StartsWith("Boss_")) continue;
            if (sr.GetComponent<BossFX>() == null) sr.gameObject.AddComponent<BossFX>();

            if (spGlow != null)
            {
                GameObject g = new GameObject("AuraBoss");
                g.transform.SetParent(sr.transform, false);
                g.transform.localPosition = new Vector3(0f, 0f, 0.3f);
                g.transform.localRotation = Quaternion.identity;
                aura = g.AddComponent<SpriteRenderer>();
                aura.sprite = spGlow;
                aura.sortingOrder = sr.sortingOrder - 1;
                aura.color = new Color(P.acento1.r, P.acento1.g, P.acento1.b, 0.45f);
                float sw = spGlow.bounds.size.x;
                float bw = sr.sprite.bounds.size.x;
                float bh = sr.sprite.bounds.size.y;
                g.transform.localScale = new Vector3(bw * 1.25f / sw, bh * 2.0f / sw, 1f);
            }
            break;
        }
    }

    // ------------------------------------------------------------ eventos
    void AoApertar(int lane)
    {
        Pad p = PadDaPista(lane);
        if (p != null) p.pulso = Mathf.Max(p.pulso, 0.7f);
    }

    void AoJulgar(int lane, Judgment j)
    {
        Pad p = PadDaPista(lane);
        Vector3 pos = p != null ? p.pos : new Vector3(0.4f, 0.4f, 10.4f);
        Color c = p != null ? p.cor : Color.white;

        if (j == Judgment.Perfect)
        {
            if (p != null) p.pulso = 1.4f;
            part.Emitir(spEstrela, pos + Vector3.up * 0.3f, Color.Lerp(c, Color.white, 0.45f), 14, 4.2f, 0.42f, 0.8f, 5f, 0.8f, Vector3.up);
            part.Emitir(spGlow, pos + Vector3.up * 0.3f, c, 4, 1.2f, 1.4f, 0.45f, 0f, 0.2f, Vector3.up);
            NovoAnel(pos, Color.Lerp(c, Color.white, 0.5f), 3.2f);
            punch = 1f;
        }
        else if (j == Judgment.Good)
        {
            if (p != null) p.pulso = 1f;
            part.Emitir(spEstrela, pos + Vector3.up * 0.3f, c, 7, 3f, 0.3f, 0.65f, 5f, 0.8f, Vector3.up);
            NovoAnel(pos, c, 2.2f);
            punch = 0.5f;
        }
        else
        {
            Color vermelho = new Color(1f, 0.2f, 0.3f);
            part.Emitir(spDiamante != null ? spDiamante : spEstrela, pos + Vector3.up * 0.3f, vermelho, 9, 2.6f, 0.26f, 0.7f, 7f, 0.9f, Vector3.up);
            punch = -0.6f;
        }
    }

    void AoVencer(int f)
    {
        // chuva de confete e estrelas na vitória
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-3f, 4f), Random.Range(3f, 7f), Random.Range(2f, 9f));
            Color c = (i % 3 == 0) ? P.acento1 : (i % 3 == 1 ? P.acento2 : P.acento3);
            part.Emitir(spConfete != null ? spConfete : spEstrela, pos, c, 30, 6f, 0.35f, 2.2f, 4f, 1f, Vector3.up);
            part.Emitir(spEstrela, pos, Color.white, 10, 5f, 0.5f, 1.6f, 3f, 1f, Vector3.up);
        }
    }

    // ------------------------------------------------------------ frame
    void Update()
    {
        float dt = Time.deltaTime;
        float t = Time.time;
        float beat = Mathf.Pow(1f - Mathf.Repeat(t * bpm / 60f, 1f), 3f);

        if (luzPrincipal != null) luzPrincipal.intensity = luzBase * (1f + 0.10f * beat);
        if (bloomVol != null) bloomVol.intensity.Override(P.bloom * (1f + 0.25f * beat));

        // soco de câmera
        punch = Mathf.MoveTowards(punch, 0f, dt * 5f);
        if (cam != null)
        {
            cam.fieldOfView = fovBase - punch * 1.8f;
        }

        // pads
        foreach (Pad p in pads)
        {
            p.pulso = Mathf.MoveTowards(p.pulso, 0f, dt * 3.2f);
            Color c = p.cor;
            c.a = Mathf.Clamp01(0.30f + 0.12f * beat + p.pulso * 0.7f);
            p.pad.color = c;
            float esc = (1.9f + p.pulso * 0.7f) / spGlow.bounds.size.x;
            p.pad.transform.localScale = Vector3.one * esc;
            Color fc = p.cor;
            fc.a = 0.10f + 0.06f * beat + p.pulso * 0.22f;
            p.faixa.color = fc;
        }

        // anéis
        foreach (Anel a in aneis)
        {
            if (a.tt >= a.dur) { if (a.t.gameObject.activeSelf) a.t.gameObject.SetActive(false); continue; }
            a.tt += dt;
            float k = Mathf.Clamp01(a.tt / a.dur);
            float s = Mathf.Lerp(0.6f, a.tam, 1f - (1f - k) * (1f - k));
            a.t.localScale = Vector3.one * (s / a.sr.sprite.bounds.size.x);
            Color c = a.sr.color; c.a = 1f - k; a.sr.color = c;
        }

        // aura do boss
        if (aura != null)
        {
            Color c = aura.color;
            c.a = 0.35f + 0.2f * beat;
            aura.color = c;
        }

        Ambiente(dt);
    }

    void Ambiente(float dt)
    {
        if (part == null) return;
        ambTimer -= dt;
        if (ambTimer > 0f) return;
        ambTimer = 0.07f;

        Color[] cores = { P.acento1, P.acento2, P.acento3, Color.white };
        Color c = cores[Random.Range(0, cores.Length)];
        Vector3 pos = new Vector3(Random.Range(-5.5f, 6.5f), Random.Range(2.5f, 7.5f), Random.Range(-6f, 9f));

        if (fase == 1)
        {
            part.EmitirAmbiente(spConfete, pos, c, 0.3f, 7f, new Vector3(Random.Range(-0.25f, 0.25f), -0.55f, 0f), 120f);
        }
        else if (fase == 2)
        {
            if (Random.value < 0.6f) part.EmitirAmbiente(spEstrela, pos, P.acento3, 0.35f, 6f, new Vector3(0f, 0.3f, 0f), 60f);
            else part.EmitirAmbiente(spDiamante != null ? spDiamante : spEstrela, pos, (Random.value < 0.5f ? P.acento1 : P.acento2), 0.22f, 7f, new Vector3(Random.Range(-0.2f, 0.2f), -0.4f, 0f), 90f);
        }
        else
        {
            if (Random.value < 0.6f) part.EmitirAmbiente(spEstrela, pos, Color.Lerp(c, Color.white, 0.4f), 0.32f, 5f, new Vector3(0f, 0.18f, 0f), 30f);
            else part.EmitirAmbiente(spGlow, pos, new Color(c.r, c.g, c.b, 0.55f), 0.7f, 8f, new Vector3(Random.Range(-0.15f, 0.15f), 0.35f, 0f), 0f);
        }
    }
}


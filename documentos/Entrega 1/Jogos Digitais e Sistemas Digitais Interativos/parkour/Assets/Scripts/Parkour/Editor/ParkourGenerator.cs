using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Parkour;

/// Menu: Parkour > Gerar Fase
/// Monta a fase do mapa.pdf com peças 3D arredondadas, checkpoints, goal, jogador e câmera.
public static class ParkourGenerator
{
    // ====== AJUSTE AQUI ======
    const float Z = 6f;     // distância entre as fileiras (para frente)
    const float H = 1f;     // quanto cada fileira sobe
    const float L = 3f;     // X do lado esquerdo
    const float R = 37f;    // X do lado direito

    // Visual
    const float Thick = 0.8f;    // espessura das plataformas (mais alto = mais "gordinho")
    const float PadThick = 1.2f; // espessura de start / checkpoints / goal
    const float Round = 0.25f;   // arredondamento das quinas das plataformas
    const float PadRound = 0.35f;

    // Preset estilo Roblox
    const float WalkSpeed = 7f;
    const float JumpHeight = 2.3f;
    const float GravityValue = -55f;
    const float CameraSensitivity = 2f;   // sensibilidade do mouse (graus por pixel = 0.15 x isso)
    const float CameraDistance = 7f;
    const float CameraFov = 70f;

    // Último trecho (escadinha 1-2-3): qual bloco é o SEGURO em cada coluna,
    // contando a partir do lado direito (de onde você chega). Código do mapa: 3233211
    static readonly int[] SafeRow = { 3, 2, 3, 3, 2, 1, 1 };
    // =========================

    static Material pink, purple, orange;
    static Transform root;
    static Transform startRespawn;
    static int cpCount;

    [MenuItem("Parkour/Gerar Fase")]
    public static void Generate()
    {
        var old = GameObject.Find("Parkour_Level");
        if (old) Object.DestroyImmediate(old);
        root = new GameObject("Parkour_Level").transform;
        cpCount = 0;

        pink = Mat("Plataforma_Rosa", new Color(0.90f, 0.05f, 0.45f));
        purple = Mat("Checkpoint_Roxo", new Color(0.45f, 0.05f, 0.65f));
        orange = Mat("Start_Goal_Laranja", new Color(1f, 0.5f, 0f));

        Vector3 padSize = new Vector3(5, PadThick, 4);

        // ---------- Fileira 0: START -> blocos -> checkpoint (direita) ----------
        float y = 0, z = 0;
        startRespawn = Pad("Start", new Vector3(L, y, z), padSize, orange);
        for (int i = 0; i < 7; i++)
            Box($"F0_Bloco{i}", new Vector3(9f + i * 3.8f, y, z), new Vector3(2.4f, Thick, 2.4f), pink);
        Checkpoint(new Vector3(R, y, z));

        // ---------- Fileira 1: tábuas inclinadas (direita -> esquerda) ----------
        y = 1 * H; z = 1 * Z;
        for (int i = 0; i < 9; i++)
            Box($"F1_Tabua{i}", new Vector3(33f - i * 3f, y, z), new Vector3(1.2f, Thick, 3f), pink, i < 4 ? 30f : 0f);
        Checkpoint(new Vector3(L, y, z));

        // ---------- Fileira 2: bolinhas em ziguezague (esquerda -> direita) ----------
        y = 2 * H; z = 2 * Z;
        for (int i = 0; i < 8; i++)
            Disc($"F2_Bola{i}", new Vector3(8f + i * (24f / 7f), y, z + (i % 2 == 0 ? -1.2f : 1.2f)), 2f, Thick, pink);
        Checkpoint(new Vector3(R, y, z));

        // ---------- Fileira 3: pilares finos + blocos (direita -> esquerda) ----------
        y = 3 * H; z = 3 * Z;
        for (int i = 0; i < 5; i++)
            Box($"F3_Pilar{i}", new Vector3(33f - i * 6f, y, z), new Vector3(1f, Thick, 3.5f), pink);
        for (int i = 0; i < 4; i++)
            Box($"F3_Bloco{i}", new Vector3(30f - i * 6f, y, z + (i % 2 == 0 ? 1f : -1f)), new Vector3(1.4f, Thick, 1.4f), pink);
        Checkpoint(new Vector3(L, y, z));

        // ---------- Fileira 4: argolas (esquerda -> direita) ----------
        y = 4 * H; z = 4 * Z;
        for (int i = 0; i < 6; i++)
            Ring($"F4_Argola{i}", new Vector3(8f + i * 4.7f, y, z), 1.25f, 0.55f, 0.35f);
        Checkpoint(new Vector3(R, y, z));

        // ---------- Fileira 5: vigas em "U" (vai pela esquerda e volta) ----------
        y = 5 * H;
        float zA = 5 * Z, zB = 6 * Z;
        for (int i = 0; i < 4; i++) Box($"F5A_Viga{i}", new Vector3(31f - i * 7f, y, zA), new Vector3(5f, Thick, 1.4f), pink);
        Pad("Curva", new Vector3(L, y, (zA + zB) / 2f), new Vector3(5, PadThick, 10), purple);
        for (int i = 0; i < 4; i++) Box($"F5B_Viga{i}", new Vector3(10f + i * 7f, y, zB), new Vector3(5f, Thick, 1.4f), pink);
        Checkpoint(new Vector3(R, y, zA));
        Checkpoint(new Vector3(R, y, zB));

        // ---------- Fileira 6: escadinha 1-2-3 (só um bloco por coluna é seguro) ----------
        y = 6 * H;
        float zBase = zB + 4.2f;
        for (int c = 0; c < 7; c++)
        {
            for (int r = 1; r <= 3; r++)
            {
                var b = Box($"F6_Col{c + 1}_Linha{r}", new Vector3(31f - c * 4f, y, zBase + (r - 1) * 2.8f), new Vector3(2.2f, Thick, 2.2f), pink);
                if (r != SafeRow[c]) b.AddComponent<FallingPlatform>();
                // número da fileira escrito em cima do bloco (1 = mais perto, 3 = mais longe)
                Label("Numero", r.ToString(), b.transform,
                      new Vector3(0f, Thick * 0.5f + 0.02f, 0f), Quaternion.Euler(90f, 0f, 0f),
                      0.1f, Color.white);
            }
        }

        // ---------- Placa com o código, no checkpoint antes da escadinha ----------
        BuildSign(new Vector3(R, 5 * H, zB + 1.7f));
        Pad("GOAL", new Vector3(2f, y, zBase + 2.8f), new Vector3(4, PadThick, 6), orange, true);

        BuildPlayerCameraLight();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Selection.activeGameObject = root.gameObject;
        AssetDatabase.SaveAssets();
        Debug.Log("Parkour: fase gerada! Aperte Play. (R = voltar pro checkpoint)");
    }

    // ---------- Texto e placa ----------
    static Font labelFont;

    static TextMesh Label(string name, string text, Transform parent, Vector3 localPos, Quaternion localRot, float charSize, Color color)
    {
        if (!labelFont) labelFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (!labelFont) labelFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPos;
        go.transform.localRotation = localRot;

        var tm = go.AddComponent<TextMesh>();
        tm.font = labelFont;
        tm.text = text;
        tm.fontSize = 100;
        tm.characterSize = charSize;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.fontStyle = FontStyle.Bold;
        tm.color = color;
        go.GetComponent<MeshRenderer>().sharedMaterial = labelFont.material;
        return tm;
    }

    /// Placa em pé mostrando o código da escadinha (lido de SafeRow, na ordem em que você encontra as colunas).
    static void BuildSign(Vector3 baseCenter)
    {
        var dark = Mat("Placa_Escura", new Color(0.08f, 0.08f, 0.12f));
        string code = string.Join(" ", System.Array.ConvertAll(SafeRow, n => n.ToString()));

        Box("Placa_Poste1", baseCenter + new Vector3(-1.8f, 1.2f, 0f), new Vector3(0.3f, 1.2f, 0.3f), purple, 0f, null, 0.1f);
        Box("Placa_Poste2", baseCenter + new Vector3(1.8f, 1.2f, 0f), new Vector3(0.3f, 1.2f, 0.3f), purple, 0f, null, 0.1f);
        var board = Box("Placa", baseCenter + new Vector3(0f, 2.8f, 0f), new Vector3(5.2f, 1.8f, 0.25f), dark, 0f, null, 0.1f);

        // o texto fica na frente da placa (lado de quem vem olhando pra escadinha)
        Label("Placa_Titulo", "CÓDIGO: direita p/ esquerda", board.transform,
              new Vector3(0f, 0.55f, -0.16f), Quaternion.identity, 0.032f, Color.white);
        Label("Placa_Codigo", code, board.transform,
              new Vector3(0f, -0.15f, -0.16f), Quaternion.identity, 0.08f, new Color(1f, 0.9f, 0.2f));
    }

    // ---------- Preset Roblox (também dá pra rodar sozinho, sem refazer a fase) ----------
    [MenuItem("Parkour/Aplicar Preset Roblox")]
    public static void ApplyRobloxPreset()
    {
        var player = GameObject.Find("Player");
        var cam = Camera.main;
        var ctrl = player ? player.GetComponent<ThirdPersonController>() : null;
        var tpc = cam ? cam.GetComponent<ThirdPersonCamera>() : null;
        if (!ctrl || !tpc)
        {
            Debug.LogWarning("Parkour: preciso de um 'Player' (com ThirdPersonController) e da Main Camera (com ThirdPersonCamera) na cena.");
            return;
        }
        ApplyRobloxValues(ctrl, tpc, cam);
        EditorUtility.SetDirty(ctrl);
        EditorUtility.SetDirty(tpc);
        EditorUtility.SetDirty(cam);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("Parkour: preset Roblox aplicado!");
    }

    static void ApplyRobloxValues(ThirdPersonController ctrl, ThirdPersonCamera tpc, Camera cam)
    {
        // movimento: sem aceleração (liga/desliga na hora), controle total no ar, pulo sempre completo
        ctrl.walkSpeed = WalkSpeed;
        ctrl.sprintSpeed = WalkSpeed;
        ctrl.groundAcceleration = 200f;
        ctrl.airAcceleration = 120f;
        ctrl.turnSmoothTime = 0.03f;
        ctrl.jumpHeight = JumpHeight;
        ctrl.gravity = GravityValue;
        ctrl.lowJumpMultiplier = 1f;
        ctrl.coyoteTime = 0.1f;
        ctrl.autoJumpWhileHeld = true;

        // câmera: gira segurando o botão direito, scroll dá zoom, FOV 70
        tpc.rotateOnlyWithRightMouse = true;
        tpc.sensitivity = CameraSensitivity;
        tpc.distance = CameraDistance;
        tpc.minDistance = 2f;
        tpc.maxDistance = 25f;
        cam.fieldOfView = CameraFov;
    }

    // ---------- Jogador / câmera / luz ----------
    static void BuildPlayerCameraLight()
    {
        var player = GameObject.Find("Player");
        if (!player)
        {
            player = new GameObject("Player");
            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Corpo";
            Object.DestroyImmediate(body.GetComponent<Collider>());
            body.transform.SetParent(player.transform, false);
            body.transform.localPosition = new Vector3(0, 1, 0);
            body.GetComponent<Renderer>().sharedMaterial = Mat("Jogador", new Color(0.2f, 0.8f, 0.4f));

            var nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nose.name = "Frente";
            Object.DestroyImmediate(nose.GetComponent<Collider>());
            nose.transform.SetParent(player.transform, false);
            nose.transform.localPosition = new Vector3(0, 1.5f, 0.4f);
            nose.transform.localScale = Vector3.one * 0.3f;
            nose.GetComponent<Renderer>().sharedMaterial = Mat("Jogador_Frente", Color.white);

            var cc = player.AddComponent<CharacterController>();
            cc.height = 2f; cc.radius = 0.4f; cc.center = new Vector3(0, 1, 0); cc.stepOffset = 0.3f;
            player.AddComponent<ThirdPersonController>();
            player.AddComponent<PlayerRespawn>();
        }
        player.transform.SetPositionAndRotation(startRespawn.position, Quaternion.LookRotation(Vector3.forward));

        var ctrl = player.GetComponent<ThirdPersonController>();

        var cam = Camera.main;
        if (!cam)
        {
            var c = new GameObject("Main Camera") { tag = "MainCamera" };
            cam = c.AddComponent<Camera>();
            c.AddComponent<AudioListener>();
        }
        var tpc = cam.GetComponent<ThirdPersonCamera>();
        if (!tpc) tpc = cam.gameObject.AddComponent<ThirdPersonCamera>();
        tpc.target = player.transform;
        ctrl.cameraTransform = cam.transform;
        ApplyRobloxValues(ctrl, tpc, cam);

        if (!GameObject.Find("Directional Light"))
        {
            var l = new GameObject("Directional Light");
            var light = l.AddComponent<Light>();
            light.type = LightType.Directional;
            l.transform.rotation = Quaternion.Euler(50, -30, 0);
        }
    }

    // ---------- Peças ----------
    static GameObject Solid(string name, Mesh mesh, Vector3 center, float yaw, Material m, Transform parent, Vector3? boxSize)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent ? parent : root, false);
        go.transform.position = center;
        go.transform.rotation = Quaternion.Euler(0, yaw, 0);
        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        go.AddComponent<MeshRenderer>().sharedMaterial = m;
        if (boxSize.HasValue) go.AddComponent<BoxCollider>().size = boxSize.Value;
        else go.AddComponent<MeshCollider>().sharedMesh = mesh;
        return go;
    }

    /// Bloco arredondado. "top" = centro da face de cima.
    static GameObject Box(string name, Vector3 top, Vector3 size, Material m, float yaw = 0f, Transform parent = null, float round = -1f)
    {
        float r = round < 0f ? Round : round;
        var mesh = RoundedMeshes.GetBox(size, r);
        return Solid(name, mesh, top + Vector3.down * size.y * 0.5f, yaw, m, parent, size);
    }

    /// Disco arredondado (bolinhas).
    static GameObject Disc(string name, Vector3 top, float diameter, float thickness, Material m)
    {
        var mesh = RoundedMeshes.GetDisc(diameter * 0.5f, thickness, 0.3f);
        return Solid(name, mesh, top + Vector3.down * thickness * 0.5f, 0f, m, null, null);
    }

    /// Argola (rosquinha). majorR = raio até o meio do tubo; a = largura do tubo; b = altura do tubo.
    static GameObject Ring(string name, Vector3 top, float majorR, float a, float b)
    {
        var mesh = RoundedMeshes.GetTorus(majorR, a, b);
        return Solid(name, mesh, top + Vector3.down * b, 0f, pink, null, null);
    }

    static void Checkpoint(Vector3 top)
    {
        cpCount++;
        Pad($"Checkpoint_{cpCount}", top, new Vector3(5, PadThick, 4), purple);
    }

    /// Cria a plataforma + gatilho (Checkpoint ou GoalZone). Retorna o ponto de respawn.
    static Transform Pad(string name, Vector3 top, Vector3 size, Material m, bool goal = false)
    {
        Box(name, top, size, m, 0f, null, PadRound);

        var respawn = new GameObject(name + "_Respawn").transform;
        respawn.SetParent(root, false);
        respawn.position = top + Vector3.up * 0.2f;

        var trig = new GameObject(name + "_Trigger");
        trig.transform.SetParent(root, false);
        trig.transform.position = top + Vector3.up * 1.5f;
        var col = trig.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(size.x - 0.5f, 3f, size.z - 0.5f);

        if (goal) trig.AddComponent<GoalZone>();
        else trig.AddComponent<Checkpoint>().respawnPoint = respawn;
        return respawn;
    }

    // ---------- Materiais (salvos em Assets/Parkour/Materiais) ----------
    static Material Mat(string name, Color c)
    {
        RoundedMeshes.EnsureFolder("Assets/Parkour/Materiais");
        string path = $"Assets/Parkour/Materiais/{name}.mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m) return m;

        var sh = Shader.Find("Universal Render Pipeline/Lit");
        if (!sh) sh = Shader.Find("Standard");
        m = new Material(sh);
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
        if (m.HasProperty("_Color")) m.SetColor("_Color", c);
        AssetDatabase.CreateAsset(m, path);
        return m;
    }
}

/// Gera malhas arredondadas (e salva em Assets/Parkour/Malhas para reaproveitar).
public static class RoundedMeshes
{
    const int Arc = 3;  // quantos passos nas quinas arredondadas

    public static void EnsureFolder(string path)
    {
        string[] parts = path.Split('/');
        string cur = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            string next = cur + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(cur, parts[i]);
            cur = next;
        }
    }

    static string F(float v) => v.ToString("0.###", CultureInfo.InvariantCulture);

    static Mesh LoadOrSave(string key, System.Func<Mesh> build)
    {
        EnsureFolder("Assets/Parkour/Malhas");
        string path = $"Assets/Parkour/Malhas/{key}.asset";
        var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existing) return existing;
        var mesh = build();
        mesh.name = key;
        AssetDatabase.CreateAsset(mesh, path);
        return mesh;
    }

    // ---------------- Caixa arredondada ----------------
    public static Mesh GetBox(Vector3 size, float r)
    {
        return LoadOrSave($"Box_{F(size.x)}_{F(size.y)}_{F(size.z)}_r{F(r)}", () => BuildBox(size, r));
    }

    static float[] Axis(float h, float r)
    {
        var l = new List<float>();
        for (int k = Arc; k >= 0; k--) l.Add(-(h - r) - r * Mathf.Sin(k * Mathf.PI * 0.5f / Arc));
        for (int k = 0; k <= Arc; k++) l.Add((h - r) + r * Mathf.Sin(k * Mathf.PI * 0.5f / Arc));
        return l.ToArray();
    }

    static Mesh BuildBox(Vector3 size, float r)
    {
        Vector3 h = size * 0.5f;
        r = Mathf.Min(r, Mathf.Min(h.x, Mathf.Min(h.y, h.z)) * 0.95f);
        Vector3 inner = h - Vector3.one * r;

        var verts = new List<Vector3>();
        var norms = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();
        float[][] ax = { Axis(h.x, r), Axis(h.y, r), Axis(h.z, r) };

        for (int a = 0; a < 3; a++)
        for (int s = -1; s <= 1; s += 2)
        {
            int u = (a + 1) % 3, v = (a + 2) % 3;
            float[] lu = ax[u], lv = ax[v];
            int start = verts.Count;

            for (int j = 0; j < lv.Length; j++)
            for (int i = 0; i < lu.Length; i++)
            {
                Vector3 p = Vector3.zero;
                p[a] = s * h[a]; p[u] = lu[i]; p[v] = lv[j];
                Vector3 c = new Vector3(
                    Mathf.Clamp(p.x, -inner.x, inner.x),
                    Mathf.Clamp(p.y, -inner.y, inner.y),
                    Mathf.Clamp(p.z, -inner.z, inner.z));
                Vector3 d = p - c;
                Vector3 n;
                if (d.sqrMagnitude > 1e-8f) n = d.normalized;
                else { n = Vector3.zero; n[a] = s; }
                verts.Add(c + n * r);
                norms.Add(n);
                uvs.Add(new Vector2(i / (float)(lu.Length - 1), j / (float)(lv.Length - 1)));
            }

            for (int j = 0; j < lv.Length - 1; j++)
            for (int i = 0; i < lu.Length - 1; i++)
            {
                int i0 = start + j * lu.Length + i, i1 = i0 + 1, i2 = i0 + lu.Length, i3 = i2 + 1;
                tris.AddRange(new[] { i0, i1, i2, i1, i3, i2 });
            }
        }
        return Make(verts, norms, uvs, tris);
    }

    // ---------------- Disco arredondado ----------------
    public static Mesh GetDisc(float radius, float thick, float r)
    {
        return LoadOrSave($"Disco_{F(radius)}_{F(thick)}_r{F(r)}", () => BuildDisc(radius, thick, r));
    }

    static Mesh BuildDisc(float R, float thick, float r)
    {
        float hh = thick * 0.5f;
        r = Mathf.Min(r, hh * 0.95f, R * 0.5f);
        var prof = new List<Vector2>();
        prof.Add(new Vector2(0, hh));
        for (int k = 0; k <= Arc + 1; k++)
        {
            float a = Mathf.PI * 0.5f * (1f - k / (float)(Arc + 1));
            prof.Add(new Vector2(R - r + r * Mathf.Cos(a), hh - r + r * Mathf.Sin(a)));
        }
        for (int k = 0; k <= Arc + 1; k++)
        {
            float a = Mathf.PI * 0.5f * (k / (float)(Arc + 1));
            prof.Add(new Vector2(R - r + r * Mathf.Cos(a), -(hh - r) - r * Mathf.Sin(a)));
        }
        prof.Add(new Vector2(0, -hh));

        int seg = 32, n = prof.Count;
        var verts = new List<Vector3>();
        var norms = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();

        for (int j = 0; j <= seg; j++)
        {
            float phi = 2f * Mathf.PI * j / seg;
            float cs = Mathf.Cos(phi), sn = Mathf.Sin(phi);
            for (int i = 0; i < n; i++)
            {
                Vector2 t = prof[Mathf.Min(i + 1, n - 1)] - prof[Mathf.Max(i - 1, 0)];
                Vector2 n2 = new Vector2(-t.y, t.x).normalized;
                verts.Add(new Vector3(prof[i].x * cs, prof[i].y, prof[i].x * sn));
                norms.Add(new Vector3(n2.x * cs, n2.y, n2.x * sn));
                uvs.Add(new Vector2(j / (float)seg, i / (float)(n - 1)));
            }
        }
        for (int j = 0; j < seg; j++)
        for (int i = 0; i < n - 1; i++)
        {
            int i0 = j * n + i, i1 = i0 + 1, i2 = i0 + n, i3 = i2 + 1;
            tris.AddRange(new[] { i0, i1, i2, i1, i3, i2 });
        }
        return Make(verts, norms, uvs, tris);
    }

    // ---------------- Rosquinha (argola) ----------------
    public static Mesh GetTorus(float majorR, float a, float b)
    {
        return LoadOrSave($"Argola_{F(majorR)}_{F(a)}_{F(b)}", () => BuildTorus(majorR, a, b));
    }

    static Mesh BuildTorus(float Rm, float a, float b)
    {
        int seg = 32, tube = 16;
        var verts = new List<Vector3>();
        var norms = new List<Vector3>();
        var uvs = new List<Vector2>();
        var tris = new List<int>();

        for (int j = 0; j <= seg; j++)
        {
            float phi = 2f * Mathf.PI * j / seg;
            float cs = Mathf.Cos(phi), sn = Mathf.Sin(phi);
            for (int i = 0; i <= tube; i++)
            {
                float th = 2f * Mathf.PI * i / tube;
                float cx = Rm + a * Mathf.Cos(th);
                verts.Add(new Vector3(cx * cs, b * Mathf.Sin(th), cx * sn));
                Vector2 n2 = new Vector2(b * Mathf.Cos(th), a * Mathf.Sin(th)).normalized;
                norms.Add(new Vector3(n2.x * cs, n2.y, n2.x * sn));
                uvs.Add(new Vector2(j / (float)seg, i / (float)tube));
            }
        }
        for (int j = 0; j < seg; j++)
        for (int i = 0; i < tube; i++)
        {
            int i0 = j * (tube + 1) + i, i1 = i0 + 1, i2 = i0 + tube + 1, i3 = i2 + 1;
            tris.AddRange(new[] { i0, i1, i2, i1, i3, i2 });
        }
        return Make(verts, norms, uvs, tris);
    }

    // ---------------- util ----------------
    static Mesh Make(List<Vector3> v, List<Vector3> n, List<Vector2> uv, List<int> t)
    {
        // garante que as faces apontam pra fora (compara com a normal do vértice)
        for (int i = 0; i < t.Count; i += 3)
        {
            int a = t[i], b = t[i + 1], c = t[i + 2];
            Vector3 cr = Vector3.Cross(v[b] - v[a], v[c] - v[a]);
            if (Vector3.Dot(cr, n[a] + n[b] + n[c]) < 0f) { t[i + 1] = c; t[i + 2] = b; }
        }
        var m = new Mesh();
        m.SetVertices(v);
        m.SetNormals(n);
        m.SetUVs(0, uv);
        m.SetTriangles(t, 0);
        m.RecalculateBounds();
        return m;
    }
}

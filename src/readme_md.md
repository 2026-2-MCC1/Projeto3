# Entregas do PI
### **DECLARAÇÃO DE USO DE INTELIGÊNCIA ARTIFICIAL**

## Entrega 1
<br><br>

### Módulo do sistema 1

| **Aluno(s) Responsável(veis)** | Alessandro Galvao |
| :--- | :--- |
| **Ferramenta utilizada** | Claude (Anthropic) |
| **Versão/modelo** | Claude Sonnet 4.6 |
| **Disciplina / Trabalho** | Projeto interdiciplinar |
| **Finalidade de uso** | Orientação para compreensão de conceitos do Unity e resolução de dúvidas durante o desenvolvimento. O Claude atuou como mentor, explicando o raciocínio por trás das soluções para que o aluno pudesse implementar com entendimento próprio. |
| ***Prompts* principais utilizados** | 1. "Preciso que você me ajude a achar vídeos no YouTube que me ensinem a criar designs 3D com Mesh e Prefabs. Pelo que eu entendi, ele cria o model com o Mesh e depois adiciona um Prefab para estilizar."<br>2. "Olha, está spawnando mas a direção que está spawnando está errada. Preciso que você me diga onde no código eu aponto para a frente. O código atual está apontando o spawner para baixo." (seguido do código Note Spawner.cs para análise) |
| **Trechos ou entregas afetadas** | Compreensão do pipeline de Mesh e Prefabs no Unity; correção da direção de movimento das notas no script Note Spawner.cs |


### Módulo do sistema 2

| **Aluno(s) Responsável(veis)** | Murilo |
| :--- | :--- |
| **Ferramenta utilizada** | Gemini e Unity 6 LTS |
| **Versão/modelo** | Unity 6 (6000.3.0f1) / Gemini LLM |
| **Disciplina / Trabalho** | Projeto interdiciplinar |
| **Finalidade de uso** | Apoio técnico na implementação de sistemas de combate baseados em eventos (GameEvents. OnNoteJudged), criação de HUD responsivo via script (OnGUI), gerenciamento de transições de cenas por índice e nome, além de depuração de erros de compilação (C#) e problemas de interatividade em elementos de UI (como o bloqueio por Raycast Target em telas de resultado). |
| ***Prompts* principais utilizados** | • Solicitação de implementação de HUD de combate funcional e rastreamento de estado de boss e jogador.<br>• Consulta sobre como automatizar o avanço de fases via índice de build (buildIndex + 1) sem hardcoding de nomes.<br>• Pedidos de correção de bugs em botões de UI ("Tente Novamente" na TelaDerrota) e resolução de erros de namespace/métodos duplicados (CS0101, CS0111).<br>• Resolução de avisos de build sobre cenas ausentes no perfil do Unity (TelaVitoria). |
| **Trechos ou entregas afetadas** | • **Combate.cs:** Lógica de controle de vida do boss, vidas do jogador, pontuação, subscrição de eventos de notas e corrotinas de atraso para transição de tela (1.5s).<br>• **Menu Resultados.cs:** Script centralizado para botões de navegação, reinício de fase e avanço automático de nível.<br>• **Cenas do Projeto (Phase1, TelaDerrota, TelaVitoria):** Ajustes de hierarquia de UI, remoção de Raycast Targets em painéis de fundo estáticos bloqueantes, e registro de cenas no Build Settings. |

### Módulo do sistema 3

| **Aluno(s) Responsável(veis)** | Valeska Affonso Marques |
| :--- | :--- |
| **Ferramenta utilizada** | Claude (Anthropic) e YouTube |
| **Versão/modelo** | Claude Sonnet 4.6 |
| **Disciplina / Trabalho** | Projeto interdiciplinar |
| **Finalidade de uso** | Orientação técnica durante o desenvolvimento dos scripts relacionados ao sistema de combate do jogo no Unity, com foco na implementação e compreensão da lógica de batalha e das zonas de colisão (HitZone). O Claude foi utilizado como apoio para esclarecer dúvidas sobre o funcionamento dos scripts, compreender a comunicação entre os componentes e auxiliar na identificação e resolução de problemas durante a implementação. |
| ***Prompts* principais utilizados** | 1. "Estou desenvolvendo um jogo no Unity utilizando C# e preciso de ajuda para implementar a lógica do sistema de combate. Quero entender como organizar o script Combate.cs, responsável por controlar as interações de combate entre o personagem e o boss, considerando os acertos e os danos causados. Explique como estruturar essa lógica e como fazer com que ela funcione corretamente dentro do jogo."<br>2. "Preciso de ajuda para configurar e corrigir o funcionamento do script HitZone.cs no Unity. Quero detectar quando um objeto entra na zona de colisão e utilizar essa informação para identificar os acertos durante a partida. Como posso trabalhar com os métodos de colisão e integrar essa detecção ao sistema de combate? Explique o funcionamento e os possíveis ajustes necessários." |
| **Trechos ou entregas afetadas** | Desenvolvimento e implementação dos scripts Combate.cs e HitZone.cs, responsáveis pela lógica de combate e pelo reconhecimento das zonas de acerto. |

## Entrega 2
<br><br>

### Módulo do sistema 1

| **Aluno(s) Responsável(veis)** | Alessandro Galvao |
| :--- | :--- |
| **Ferramenta utilizada** | Claude (Anthropic) |
| **Versão/modelo** | Claude Sonnet 4.6 |
| **Disciplina / Trabalho** | Projeto interdiciplinar |
| **Finalidade de uso** | Mentoria técnica para reestruturação do sistema de spawn de notas - aluno trouxe o passo a passo que havia planejado e consultou o Claude para entender como implementar a lógica de forma mais simples e explicável, dado que o professor sinalizou excesso de IA no projeto. |
| ***Prompts* principais utilizados** | "Preciso de ajuda para reestruturar o sistema de spawn de notas no Unity. A ideia é criar 8 objetos vazios na cena - 4 pontos de saída (Spawn_D, Spawn_F, Spawn_K, Spawn_L) e 4 pontos de chegada (Hit_D, Hit_F, Hit_K, Hit_L) - um par para cada tecla. Depois, configurar o NoteSpawner no Inspector ligando esses pontos ao script, para que cada nota nasça no ponto de spawn da sua pista e chegue exatamente no ponto de hit correspondente." |
| **Trechos ou entregas afetadas** | Reestruturação do NoteSpawner.cs com sistema de pontos de spawn e hit por pista; configuração dos objetos Spawn_D/F/K/L e Hit_D/F/K/L na hierarquia do Unity; decisão de simplificar o código. |


### Módulo do sistema 2

| **Aluno(s) Responsável(veis)** | Murilo |
| :--- | :--- |
| **Ferramenta utilizada** | Gemini e Unity 6 LTS |
| **Versão/modelo** | Unity 6 (6000.3.0f1) / Gemini LLM |
| **Disciplina / Trabalho** | Projeto interdiciplinar |
| **Finalidade de uso** | Apoio técnico na implementação de sistemas de combate baseados em eventos (GameEvents. OnNoteJudged), criação de HUD responsivo via script (OnGUI), gerenciamento de transições de cenas por índice e nome, além de depuração de erros de compilação (C#) e problemas de interatividade em elementos de UI (como o bloqueio por Raycast Target em telas de resultado). |
| ***Prompts* principais utilizados** | • Solicitação de implementação de HUD de combate funcional e rastreamento de estado de boss e jogador.<br>• Consulta sobre como automatizar o avanço de fases via índice de build (buildIndex + 1) sem hardcoding de nomes.<br>• Pedidos de correção de bugs em botões de UI ("Tente Novamente" na TelaDerrota) e resolução de erros de namespace/métodos duplicados (CS0101, CS0111).<br>• Resolução de avisos de build sobre cenas ausentes no perfil do Unity (TelaVitoria). |
| **Trechos ou entregas afetadas** | • **Combate.cs:** Lógica de controle de vida do boss, vidas do jogador, pontuação, subscrição de eventos de notas e corrotinas de atraso para transição de tela (1.5s).<br>• **Menu Resultados.cs:** Script centralizado para botões de navegação, reinício de fase e avanço automático de nível.<br>• **Cenas do Projeto (Phase1, TelaDerrota, TelaVitoria):** Ajustes de hierarquia de UI, remoção de Raycast Targets em painéis de fundo estáticos bloqueantes, e registro oficial de todas as cenas no Build Settings para garantir o carregamento correto em tempo de execução.<br><br>**Resolução e Conclusão das Pendências:**<br>1. Inclusão bem-sucedida da cena TelaVitoria (juntamente com TelaDerrota e Phase1) na lista de cenas do Build Settings, eliminando o erro de carregamento ao derrotar o boss.<br>2. Ajuste definitivo da interatividade do botão "Tente Novamente" na TelaDerrota após a remoção do bloqueio de Raycast Target no painel de fundo, permitindo o reinício fluido do combate. |

### Módulo do sistema 3

| **Aluno(s) Responsável(veis)** | Valeska Affonso Marques |
| :--- | :--- |
| **Ferramenta utilizada** | Claude (Anthropic) e YouTube |
| **Versão/modelo** | Claude Sonnet 4.6 |
| **Disciplina / Trabalho** | Projeto interdiciplinar |
| **Finalidade de uso** | Apoio na estilização e organização visual dos cenários das três fases do jogo, além da integração das implementações com as partes desenvolvidas pelos demais integrantes da equipe. O Claude foi utilizado principalmente para auxiliar na configuração e correção do Placar (HUD), especialmente na exibição e no posicionamento dos elementos de pontuação na tela. A ferramenta também serviu como apoio para compreender os ajustes necessários nas referências de posicionamento e na organização dos elementos da interface dentro do Unity. |
| ***Prompts* principais utilizados** | 1. "Estou desenvolvendo um jogo no Unity e preciso de ajuda para configurar e corrigir o Placar (HUD). Quero exibir a pontuação do jogador na tela e posicionar corretamente os elementos, evitando desalinhamentos ou sobreposições. Como posso configurar o Canvas, os elementos de texto e o RectTransform para garantir que o placar seja exibido corretamente durante o jogo? Explique como ajustar as posições e referências dos elementos para que a interface fique organizada."<br>2. "Estou com dificuldades no posicionamento dos elementos do Placar (HUD) no Unity, pois preciso trabalhar com diferentes posições e referências na tela. Como posso configurar corretamente os elementos para que a pontuação apareça no local desejado? Analise as possibilidades de ajuste no RectTransform e explique como corrigir possíveis problemas de alinhamento e posicionamento." |
| **Trechos ou entregas afetadas** | Estilização e organização visual dos cenários das fases 1, 2 e 3; configuração e correção do Placar (HUD), incluindo a exibição e o posicionamento dos elementos de pontuação; integração dos cenários e elementos visuais com as implementações desenvolvidas pelos demais integrantes da equipe. |

***Declaro que o uso acima descrito está em conformidade com o GUIA PARA O USO RESPONSÁVEL DE
INTELIGÊNCIA ARTIFICIAL NA FECAP e que o conteúdo final é de minha plena responsabilidade
autoral.***
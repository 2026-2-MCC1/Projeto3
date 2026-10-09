# FECAP - Fundação de Comércio Álvares Penteado

<p align="center">
<a href= "https://www.fecap.br/"><img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhZPrRa89Kma0ZZogxm0pi-tCn_TLKeHGVxywp-LXAFGR3B1DPouAJYHgKZGV0XTEf4AE&usqp=CAU" alt="FECAP - Fundação de Comércio Álvares Penteado" border="0"></a>
</p>

# TurtleHero

## Nome do Grupo

TurtleHero

## Integrantes: <a href="https://www.linkedin.com/in/murilo-ribeiro-vasconcellos-5bb6493b0/">Murilo Ribeiro Vasconcellos</a>, <a href="[LINK DO LINKEDIN]">Alessandro Galvao Silva</a>, <a href="[LINK DO LINKEDIN]">Valeska Affonso Marques</a>

## Professores Orientadores: <a href="https://www.linkedin.com/in/victorbarq/">Victor Bruno Alexander Rosetti de Quiroz</a></a>

## Descrição

<p align="center">
<img src="imagens/capa.png" alt="TurtleHero" border="0">
</p>

**TurtleHero** é um jogo de ritmo 2D, no estilo *Guitar Hero*, desenvolvido na **Unity** com **C#**. O jogador controla a **TurtleHero** (a Tortuguita transformada em guerreira do sabor) e acompanha notas que descem por 4 pistas, apertando o botão correspondente quando elas chegam à zona de acerto. Cada acerto causa dano ao boss; cada erro custa uma das 3 vidas.

No mundo doce da Arcor, três bosses malignos — **Big Big Bigão**, **7Belos** e **Plutonita** — aprisionaram 12 doces em suas fortalezas (4 por boss). Para libertá-los, o jogador precisa derrotar os três bosses no ritmo. Cada doce libertado vai para a **caixa de bombom** e desbloqueia uma **curiosidade real** sobre o produto, unindo diversão, recompensa afetiva e conteúdo educativo.

O projeto foi desenvolvido em grupo de 3 integrantes, com apoio de IAs generativas, e o processo de criação (IAs utilizadas, prompts, erros e correções, créditos de assets) está documentado na seção 7 do GDD, na pasta `documentos`.

### 🎮 Como jogar

- 4 botões fixos na parte inferior da tela; as notas descem até a **zona de acerto**.
- Aperte o botão da pista correspondente quando a nota chegar nele.
- O acerto é avaliado em **Perfeito**, **Bom** ou **Errou**.
- Acertos causam dano ao boss (Perfeito causa mais dano que Bom); **Errou** faz você perder 1 das **3 vidas**.
- Zerar a barra de vida do boss vence a fase; perder as 3 vidas é derrota.
- Cada boss derrotado liberta 4 doces, que vão para a caixa de bombom. Clique em um doce para ler sua curiosidade.

### ⭐ Pontuação e combo

Os pontos de cada acerto são os pontos base multiplicados pelo multiplicador de combo. Um **Errou** zera o combo. Os valores são propostas iniciais, ajustadas nos testes de usabilidade.

| Resultado | Pontos base | Efeito no combo |
| --- | --- | --- |
| Perfeito | 100 | +1 |
| Bom | 50 | +1 |
| Errou | 0 | volta a 0 |

| Combo atual | Multiplicador |
| --- | --- |
| 0 a 9 | x1 |
| 10 a 19 | x2 |
| 20 a 29 | x3 |
| 30 ou mais | x4 |

### 👾 Fases

A velocidade e a densidade das notas aumentam a cada fase. A Fase 1 serve de aprendizado.

| Fase | Boss | Tema | Doces libertados | Dificuldade |
| --- | --- | --- | --- | --- |
| 1 | Big Big Bigão | Chiclete | Big Big, Poosh, Rocklets, Block | Menor |
| 2 | 7Belos | Caramelo | 7Belo, Pirulito 7Belo, Butter Toffees, Torrone | Intermediária |
| 3 | Plutonita | Espacial | Plutonita, Bon o Bon, Tortuguita, Chocolate ao Leite | Maior |

> Poosh, Pirulito 7Belo, Torrone e Chocolate ao Leite são sugestões a confirmar, conforme o GDD.

## 🛠 Estrutura de pastas

-Raiz<br>
|<br>
|-->documentos<br>
  &emsp;|GDD_TurtleHero_v1_15.docx<br>
|-->executáveis<br>
  &emsp;|-->windows<br>
|-->imagens<br>
|-->src<br>
  &emsp;|-->Assets<br>
  &emsp;|-->Packages<br>
  &emsp;|-->ProjectSettings<br>
|readme.md<br>

A pasta raiz contem dois arquivos que devem ser alterados:

<b>README.MD</b>: Arquivo que serve como guia e explicação geral sobre seu projeto. O mesmo que você está lendo agora.

Há também 4 pastas que seguem da seguinte forma:

<b>documentos</b>: Toda a documentação estará nesta pasta, incluindo o GDD (Documento para Design de Games) do TurtleHero.

<b>executáveis</b>: Binários e executáveis do projeto devem estar nesta pasta.

<b>imagens</b>: Imagens do sistema (arte do herói, bosses, doces e telas).

<b>src</b>: Pasta que contém o projeto Unity (código-fonte em C#, cenas e assets).

## 🛠 Instalação

<b>Windows:</b>

Não há instalação! Apenas executável!
Encontre o TurtleHero.exe na pasta executáveis/windows e execute-o como qualquer outro programa.

> Ajuste esta seção se o grupo também gerar builds para outras plataformas (por exemplo, WebGL ou Android).

## 💻 Configuração para Desenvolvimento

Para abrir este projeto você necessita das seguintes ferramentas:

- <a href="https://unity.com/download">Unity Hub e Unity Editor</a> (versão: 6000.3.6f)
- Uma IDE com suporte a C# (Visual Studio ou VS Code)
- <a href="https://git-scm.com/">Git</a>

Passo a passo:

```sh
git clone https://github.com/2026-2-MCC1/Projeto3.git
```

1. Abra o Unity Hub e clique em **Add > Add project from disk**.
2. Selecione a pasta `src` do repositório.
3. Abra o projeto com a versão do Unity indicada acima (o Hub oferece a instalação, se necessário).
4. Abra a cena do menu inicial e clique em **Play**.

Os valores de balanceamento (dano por acerto, vida do boss, velocidade das notas e janela de acerto) ficam em variáveis ajustáveis no Inspector da Unity.

## 📋 Licença/License

O jogo usa nomes, doces e curiosidades de marcas da **Arcor** (marcas de terceiros), sem licença de uso comercial: o projeto é de uso **apenas acadêmico e sem fins comerciais**, e o jogo não deve ser distribuído comercialmente.

Código e materiais autorais do grupo: utilize o link <https://chooser-beta.creativecommons.org/> para gerar uma licença CC BY 4.0 e adicione-a aqui.

## 🎓 Referências

Aqui estão as referências usadas no projeto.

1. <https://github.com/iuricode/readme-template>
2. <https://github.com/gabrieldejesus/readme-model>
3. <https://chooser-beta.creativecommons.org/>
4. <https://www.toptal.com/developers/gitignore>
5. UNITY TECHNOLOGIES. **Unity Manual** e **Scripting API**. Disponível em: <https://docs.unity3d.com>. Acesso em: [data].
6. UNITY TECHNOLOGIES. **Unity Download**. Disponível em: <https://unity.com/download>. Acesso em: [data].
7. CSIKSZENTMIHALYI, Mihaly. **Flow**: the psychology of optimal experience. New York: Harper & Row, 1990.
8. TURTLEHERO. **Documentação Oficial do Projeto**: versão 1.0 — conceito inicial. Documento interno da equipe, 2026.
9. Músicas, efeitos sonoros, fontes e demais assets externos: [informar autor, link, licença e data de acesso, conforme a seção 7.7 do GDD].

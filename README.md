# 🎮 Dungeon Crawler

> Um projeto de desenvolvimento de jogos focado em **Programação Orientada a Objetos, Arquitetura de Software, Design Patterns e Engenharia de Software**.

---

## 📌 Sobre o Projeto

O **Dungeon Crawler** é o **Projeto 01** da trilha de desenvolvimento de jogos do portfólio.

O objetivo é construir um jogo de exploração de masmorras, combate e progressão de personagens, utilizando o projeto como laboratório prático para aprofundar conceitos de **Engenharia de Software** e **desenvolvimento de sistemas complexos**.

Mais do que desenvolver um jogo funcional, o projeto busca aplicar uma abordagem profissional de desenvolvimento, desde a definição do produto e modelagem do domínio até testes, documentação, integração contínua e distribuição.

---

## 🎯 Objetivos

O projeto tem como principais objetivos:

* Consolidar conhecimentos de **Programação Orientada a Objetos (POO)**;
* Aplicar princípios de **Clean Code**;
* Explorar **Arquitetura de Software**;
* Utilizar **Design Patterns** quando apropriado;
* Desenvolver um domínio desacoplado da camada de apresentação;
* Implementar regras de negócio de forma testável;
* Trabalhar com testes automatizados;
* Aplicar boas práticas de versionamento;
* Implementar CI/CD;
* Documentar decisões arquiteturais;
* Construir um projeto completo e apresentável como portfólio profissional.

---

## 🏰 Conceito

O jogador deverá explorar masmorras, enfrentar inimigos, coletar recursos e evoluir seu personagem.

O ciclo principal do jogo será baseado em:

```text
EXPLORAR
    ↓
ENCONTRAR
    ↓
ENFRENTAR
    ↓
VENCER
    ↓
COLETAR
    ↓
EVOLUIR
    ↓
EXPLORAR NOVAMENTE
```

O projeto poderá evoluir progressivamente, adicionando novos sistemas e mecânicas conforme o desenvolvimento avançar.

---

## 🧩 Principais Sistemas

A definição final dos sistemas será realizada durante a fase de planejamento do projeto.

Entre os sistemas previstos estão:

* 👤 Personagens;
* ⚔️ Sistema de combate;
* 👾 Inimigos;
* 🗺️ Exploração de dungeons;
* 🎒 Inventário;
* 🗡️ Itens e equipamentos;
* ❤️ Vida e atributos;
* ✨ Habilidades;
* 📈 Progressão de personagem;
* 💾 Persistência do estado do jogo;
* 🎲 Eventos e mecânicas de jogo.

---

## 🏗️ Arquitetura

As regras do jogo ficam em um núcleo em C# puro, separado da Unity, que apenas apresenta o jogo:

```text
Presentation (Unity) ──► Infrastructure ──► Application ──► Domain
```

* **Domain** — regras do jogo; não depende de nada além do .NET;
* **Application** — casos de uso;
* **Infrastructure** — aleatoriedade, persistência, logging;
* **Presentation** — Unity: cenas, UI, input.

A direção das dependências é verificada por testes automatizados. O desenho completo está em [docs/architecture/ARCHITECTURE.md](docs/architecture/ARCHITECTURE.md) e as decisões em [docs/architecture/adr/](docs/architecture/adr/).

---

## 🛠️ Tecnologias

| Área | Tecnologia |
|---|---|
| Linguagem | C# 9 (núcleo) |
| Núcleo do jogo | .NET Standard 2.1 |
| Engine | Unity (camada de apresentação — ainda não configurada) |
| Testes | xUnit sobre .NET 10 |
| Controle de versão | Git / GitHub |

---

## ▶️ Como Executar

Pré-requisito: [.NET SDK 10](https://dotnet.microsoft.com/download).

```text
git clone https://github.com/PabloHoDev/dotnet-dungeon-crawler.git
cd dotnet-dungeon-crawler
dotnet run --project src/DungeonCrawler.Cli
```

O jogo roda no terminal. A versão em Unity ainda não existe; as regras são as mesmas e ficam no núcleo.

Para rodar os testes:

```text
dotnet test
```

Use `--seed` para repetir exatamente a mesma partida:

```text
dotnet run --project src/DungeonCrawler.Cli -- --seed 42
```

---

## 🕹️ Como Jogar

1. Crie um personagem e escolha a classe: **Guerreiro**, **Arcanista** ou **Ladino**.
2. No hub, inicie uma expedição na Cripta Esquecida: cinco andares, com um Goblin de Elite no terceiro, um mini-chefe no quarto e o chefe no quinto.
3. Em cada trecho, escolha a sala. Nas bifurcações, a sala do tesouro dá um item sem luta; a de combate dá experiência.
4. No combate, a cada rodada escolha entre atacar, usar uma habilidade (gasta Energia ou Mana) ou defender (metade do dano e recupera recurso).
5. Ao subir de nível, abra a tela de Personagem (`p`) para distribuir 5 pontos de atributo e 1 ponto de habilidade.
6. Equipe o que encontrar na tela de Inventário (`i`).

Morrer encerra a expedição, mas nível, experiência, atributos, habilidades e itens são mantidos para a próxima.

---

## 📚 Metodologia de Desenvolvimento

O projeto será desenvolvido de forma incremental e organizado em **Etapas** e **Fases**.

Cada etapa deverá passar por um processo formal de validação antes do avanço para a próxima.

### Processo de Validação

```text
DEFINIÇÃO
    ↓
IMPLEMENTAÇÃO
    ↓
VERIFICAÇÃO
    ↓
IDENTIFICAÇÃO DE PENDÊNCIAS
    ↓
CORREÇÃO
    ↓
VALIDAÇÃO FINAL
    ↓
APROVAÇÃO
    ↓
PRÓXIMA ETAPA
```

O avanço do projeto ocorrerá somente após a validação dos critérios definidos para cada etapa.

---

## 🗺️ Roadmap

O roadmap completo, com etapas e fases, está em [docs/architecture/ROADMAP.md](docs/architecture/ROADMAP.md).

* [x] Visão e definição do produto;
* [ ] Configuração inicial (falta a Unity);
* [ ] Arquitetura (falta a integração com a Unity);
* [x] Núcleo de domínio;
* [x] Personagens;
* [x] Sistema de combate;
* [x] Inimigos;
* [x] Itens e inventário;
* [x] Exploração;
* [x] Progressão;
* [ ] Persistência;
* [ ] Interface (existe a versão em terminal; falta a versão em Unity);
* [ ] Testes;
* [ ] Qualidade;
* [ ] Dockerização;
* [ ] CI/CD;
* [ ] Documentação;
* [ ] Release.

---

## 📖 Documentação

A documentação técnica do projeto será mantida no diretório:

```text
docs/
```

```text
docs/
├── product/          Definição do produto (visão, mecânicas, MVP, requisitos)
├── architecture/     ARCHITECTURE.md, ROADMAP.md e adr/
└── development/      REPOSITORY.md (estrutura, build, convenções)
```

---

## 🧪 Qualidade

A qualidade será tratada como parte do processo de desenvolvimento.

O projeto deverá buscar:

* Testes automatizados;
* Validação contínua;
* Revisão de código;
* Controle de qualidade;
* Documentação das decisões importantes;
* Integração contínua;
* Verificação automatizada do projeto.

---

## 📂 Estrutura

```text
dotnet-dungeon-crawler/
│
├── src/
│   ├── DungeonCrawler.Domain/
│   ├── DungeonCrawler.Application/
│   ├── DungeonCrawler.Infrastructure/
│   └── DungeonCrawler.Cli/            Jogo em terminal
│
├── tests/
│   ├── DungeonCrawler.Domain.Tests/
│   ├── DungeonCrawler.Application.Tests/
│   └── DungeonCrawler.Architecture.Tests/
│
├── docs/
│
├── DungeonCrawler.sln
└── README.md
```

O projeto Unity (`Assets/`, `Packages/`, `ProjectSettings/`) será criado na raiz. Detalhes em [docs/development/REPOSITORY.md](docs/development/REPOSITORY.md).

---

## 🚧 Status do Projeto

**Em desenvolvimento.**

| Etapa | Situação |
|---|---|
| ETAPA 00 — Visão e Definição do Produto | Concluída |
| ETAPA 01 — Setup e Repositório | Solução .NET, testes e `.gitignore` prontos; falta configurar a Unity |
| ETAPA 02 — Arquitetura | Definida e verificada por testes; falta a integração com a Unity |
| ETAPA 03 — Núcleo de Domínio | Implementado e testado |
| ETAPAS 04 a 09 — Personagens, Combate, Inimigos, Itens, Dungeon, Progressão | Implementadas no escopo do MVP e jogáveis no terminal |

Ficaram fora do MVP: efeitos de status em combate, itens consumíveis, eventos de sala e Save/Load.

### Próximas Etapas

> **ETAPA 01 / 02 — configurar a Unity** e, depois, **ETAPA 11 — Interface**

---

## 👨‍💻 Objetivo de Portfólio

Este projeto faz parte de uma estratégia de desenvolvimento de portfólio voltada à demonstração prática de diferentes competências técnicas.

Enquanto projetos anteriores tiveram foco em áreas como **automação, dados, BI e sistemas corporativos**, o Dungeon Crawler tem como foco principal:

```text
POO
   +
Arquitetura
   +
Design Patterns
   +
Engenharia de Software
   +
Game Development
```

O objetivo é demonstrar a capacidade de projetar, desenvolver, testar, documentar e evoluir um sistema complexo desde sua concepção até sua distribuição.

---

## 🧭 O Que o Projeto Demonstra

```text
              DUNGEON CRAWLER
                     │
        ┌────────────┼────────────┐
        ↓            ↓            ↓
       C#          Unity       GitHub
        │            │            │
        ↓            ↓            ↓
       POO       Gameplay      Workflow
        │            │            │
        └────────────┼────────────┘
                     ↓
                Arquitetura
                     │
             ┌───────┴───────┐
             ↓               ↓
           SOLID           Testes
             │               │
             └───────┬───────┘
                     ↓
          Engenharia de Software
                     │
        ┌────────────┼────────────┐
        ↓            ↓            ↓
     Design       Testing       DevOps
        │            │            │
      SOLID      Unit Tests     CI/CD
     Patterns    Integration    Docker
    Clean Code    Coverage      Release
```

Dentro do portfólio, cada stack cobre uma área diferente da Engenharia de Software:

| Stack | Foco |
|---|---|
| Python | Automação, dados, ETL |
| **C# (este projeto)** | **POO, jogos, arquitetura** |
| TypeScript | Web, APIs, full stack |
| C++ / Rust | Sistemas, performance |

---

## 📜 Licença

Distribuído sob a licença MIT. Veja [LICENSE](LICENSE).

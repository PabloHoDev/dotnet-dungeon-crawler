# Product Vision — Dungeon Crawler

## 1. Visão Geral

O **Dungeon Crawler** é um jogo 2D do gênero **Dungeon Crawler / RPG**, desenvolvido em **C#** utilizando a **Unity** como game engine.

O projeto tem como objetivo criar uma experiência de exploração de dungeons, combate estratégico e progressão de personagem, enquanto demonstra a aplicação prática de princípios de Engenharia de Software e desenvolvimento orientado a objetos.

O jogo será desenvolvido inicialmente para **PC**, com escopo controlado por meio de um **MVP (Minimum Viable Product)**, priorizando uma experiência completa, funcional e tecnicamente consistente.

---

## 2. Conceito do Produto

O jogador assumirá o papel de um aventureiro que explora dungeons perigosas em busca de recursos, equipamentos e recompensas.

Ao longo de sua jornada, deverá enfrentar desafios, superar inimigos e evoluir seu personagem para avançar por áreas progressivamente mais difíceis.

A experiência será construída em torno da relação entre:

* Exploração;
* Combate;
* Gerenciamento de recursos;
* Progressão;
* Risco e recompensa.

O jogador deverá tomar decisões durante suas expedições, equilibrando o desejo de avançar com a necessidade de preservar seus recursos e se preparar para desafios futuros.

---

## 3. Gênero

O projeto será classificado como:

* **Dungeon Crawler**
* **RPG**
* **2D**
* **Combate baseado em turnos**

A combinação desses elementos permitirá desenvolver um projeto com escopo controlado, mas suficientemente rico para explorar diferentes conceitos de Engenharia de Software.

---

## 4. Plataforma

A primeira versão do projeto será desenvolvida para:

* **Plataforma:** PC
* **Engine:** Unity
* **Linguagem principal:** C#

A possibilidade de suporte a outras plataformas poderá ser avaliada futuramente, mas não faz parte do escopo inicial do projeto.

---

## 5. Público-Alvo

O produto será direcionado principalmente a jogadores interessados em:

* Jogos de RPG;
* Exploração de dungeons;
* Combate estratégico;
* Progressão de personagens;
* Gerenciamento de recursos;
* Fantasia medieval;
* Experiências baseadas em risco e recompensa.

O projeto deverá manter uma abordagem acessível, evitando complexidade excessiva que prejudique a compreensão das mecânicas principais.

---

## 6. Objetivo do Produto

O objetivo do produto é entregar um Dungeon Crawler 2D funcional e completo em escala de MVP, no qual o jogador possa:

1. Preparar seu personagem;
2. Iniciar uma expedição;
3. Explorar uma dungeon;
4. Enfrentar desafios;
5. Coletar recursos e recompensas;
6. Evoluir seu personagem;
7. Superar um desafio final;
8. Retornar e se preparar para novas expedições.

A primeira versão deverá apresentar um ciclo de jogo completo, permitindo que o jogador tenha uma experiência consistente do início ao fim do MVP.

---

## 7. Objetivo de Engenharia de Software

Além de entregar um jogo funcional, o projeto será utilizado como demonstração prática de conhecimentos em Engenharia de Software.

O desenvolvimento buscará aplicar conceitos como:

* Programação Orientada a Objetos;
* Princípios SOLID;
* Domain Modeling;
* Separação de responsabilidades;
* Baixo acoplamento;
* Alta coesão;
* Design Patterns quando aplicáveis;
* Código testável;
* Testes automatizados;
* Persistência de dados;
* Logging;
* Observabilidade;
* Controle de qualidade;
* Dockerização;
* CI/CD;
* Documentação técnica.

A complexidade técnica deverá ser proporcional às necessidades reais do produto. O projeto não deverá adotar complexidade arquitetural apenas para demonstrar conhecimento.

---

## 8. Princípio Arquitetural

O projeto deverá buscar uma separação clara entre o **núcleo de domínio** e a **camada de apresentação executada pela Unity**.

A lógica central do jogo deverá ser projetada de forma que as regras de negócio não dependam diretamente de componentes específicos da Unity sempre que isso não for necessário.

Conceitualmente:

```text
                 DUNGEON CRAWLER
                        │
             ┌──────────┴──────────┐
             │                     │
       GAME DOMAIN              UNITY
             │                     │
      ┌──────┼──────┐              │
      │      │      │              │
   Combat  Items  Dungeon      Rendering
      │      │      │              │
      └──────┼──────┘              │
             │                     │
        Application ───────────────┘
             │
       Persistence
             │
        Save / Load
```

A arquitetura definitiva será especificada e documentada posteriormente, durante a **ETAPA 02 — Arquitetura**.

---

## 9. Princípios do Produto

O desenvolvimento do Dungeon Crawler será orientado pelos seguintes princípios:

### 9.1. Produto antes da complexidade

As decisões técnicas deverão atender às necessidades reais do jogo.

### 9.2. Código sustentável

O código deverá priorizar legibilidade, organização, manutenção e evolução.

### 9.3. Separação de responsabilidades

Cada componente deverá possuir responsabilidades claras e bem definidas.

### 9.4. Testabilidade

As regras importantes do domínio deverão ser projetadas de forma que possam ser testadas de maneira confiável.

### 9.5. Evolução incremental

O projeto será desenvolvido progressivamente, validando cada etapa antes de avançar.

### 9.6. Escopo controlado

O MVP deverá possuir limites claros para evitar expansão desnecessária do projeto.

### 9.7. Documentação contínua

Decisões importantes deverão ser registradas ao longo do desenvolvimento.

### 9.8. Qualidade técnica

O projeto deverá buscar práticas profissionais de desenvolvimento, incluindo versionamento, testes, automação e integração contínua.

---

## 10. Escopo em Alto Nível

Em sua visão inicial, o Dungeon Crawler deverá contemplar:

* Personagem jogável;
* Exploração de dungeons;
* Combate por turnos;
* Inimigos;
* Itens;
* Inventário;
* Equipamentos;
* Recursos;
* Progressão de personagem;
* Recompensas;
* Desafios especiais;
* Chefes;
* Sistema de vitória e derrota;
* Persistência de progresso;
* Interface de usuário;
* Sistema de preparação entre expedições.

O detalhamento e a priorização desses elementos serão definidos nas fases posteriores da **ETAPA 00** e consolidados no escopo do MVP.

---

## 11. Escopo Inicial de Plataforma

A primeira versão terá como foco exclusivo a execução em **PC**.

Não fazem parte do escopo inicial:

* Desenvolvimento para consoles;
* Desenvolvimento para dispositivos móveis;
* Multiplayer;
* Sistemas online;
* Serviços de nuvem obrigatórios para execução do jogo.

Esses itens poderão ser considerados futuramente, caso façam sentido para a evolução do projeto.

---

## 12. Objetivo de Portfólio

O Dungeon Crawler será uma peça estratégica do portfólio profissional, com o objetivo de demonstrar capacidade de desenvolvimento em uma stack diferente da principal experiência atual do projeto.

A escolha de **C#** representa uma expansão da experiência técnica, complementando projetos desenvolvidos anteriormente em Python.

O projeto deverá demonstrar:

```text
Python
Automação / Data / ETL
        │
        ▼
C# + Unity
POO / Jogos / Arquitetura
        │
        ▼
Engenharia de Software
```

O objetivo não será apenas demonstrar que o desenvolvedor consegue criar um jogo, mas que consegue projetar e desenvolver um sistema de software estruturado, testável, documentado e evolutivo.

---

## 13. Critérios de Sucesso em Alto Nível

O produto será considerado bem-sucedido quando:

* Possuir um ciclo de gameplay completo;
* For executável e jogável em PC;
* Apresentar uma experiência coerente do início ao fim do MVP;
* Possuir sistemas principais integrados;
* Apresentar arquitetura organizada;
* Possuir núcleo de domínio adequadamente desacoplado da Unity;
* Possuir testes automatizados para regras relevantes;
* Possuir persistência funcional;
* Possuir documentação técnica adequada;
* Possuir pipeline de qualidade e integração contínua;
* Estiver disponível em um repositório público organizado;
* Possuir documentação suficiente para que terceiros compreendam o projeto e sua arquitetura.

---

## 14. Fora do Escopo Inicial

Para manter o projeto controlado, os seguintes itens não fazem parte do escopo inicial:

* Multiplayer;
* Modo cooperativo;
* Sistemas online;
* Microtransações;
* Monetização;
* Multiplayer competitivo;
* Desenvolvimento para múltiplas plataformas;
* Mundo aberto;
* Grande quantidade de conteúdo;
* Sistemas excessivamente complexos que não contribuam diretamente para o MVP.

Novas funcionalidades poderão ser avaliadas posteriormente, desde que não comprometam o objetivo principal do MVP.

---

## 15. Visão de Futuro

O Dungeon Crawler deverá ser construído de forma que sua arquitetura permita evolução futura sem exigir uma reestruturação completa do núcleo do sistema.

Possíveis extensões futuras incluem:

* Novas dungeons;
* Novos inimigos;
* Novas classes;
* Novas habilidades;
* Novos equipamentos;
* Novos sistemas de progressão;
* Novos tipos de eventos;
* Novas mecânicas de exploração;
* Expansão para outras plataformas.

Essas possibilidades não fazem parte do MVP e não devem influenciar negativamente o escopo inicial.

---

## 16. Resumo da Visão

> O Dungeon Crawler é um RPG 2D de exploração de dungeons e combate por turnos, desenvolvido em C# e Unity para PC. O projeto busca entregar uma experiência completa em escala de MVP, combinando exploração, combate, gerenciamento de recursos e progressão de personagem com uma abordagem profissional de Engenharia de Software.

O projeto será desenvolvido com foco em qualidade técnica, arquitetura sustentável, testabilidade, documentação e evolução incremental, utilizando o desenvolvimento do jogo como uma demonstração prática de conhecimentos em C#, Programação Orientada a Objetos e Engenharia de Software.

---

## 17. Status do Documento

| Item                               | Status                 |
| ---------------------------------- | ---------------------- |
| Visão do produto                   | Definida               |
| Conceito geral                     | Definido               |
| Gênero                             | Definido               |
| Plataforma                         | Definida               |
| Stack principal                    | Definida               |
| Público-alvo                       | Definido               |
| Objetivo do produto                | Definido               |
| Objetivo de Engenharia de Software | Definido               |
| Princípio arquitetural             | Definido em alto nível |
| Escopo em alto nível               | Definido               |
| Critérios de sucesso               | Definidos              |
| Fora do escopo inicial             | Definido               |
| Visão de futuro                    | Definida               |

**Status da FASE 01:** Em validação.

**Próximo passo:** Checking da FASE 01.

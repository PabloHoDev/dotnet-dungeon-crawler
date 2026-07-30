# World and Dungeon

## ETAPA 00 — Visão e Definição do Produto

**Documento:** WORLD_AND_DUNGEON.md

**Projeto:** Dungeon Crawler

**Versão:** 1.0

**Status:** Em Desenvolvimento

---

# 1. Objetivo do Documento

Este documento define a estrutura conceitual do mundo do Dungeon Crawler.

Seu objetivo é estabelecer como o mundo será organizado, como as dungeons estarão distribuídas e quais serão os princípios que orientarão a exploração do jogador.

As definições aqui descritas servem como base para a modelagem do domínio, arquitetura do software e implementação dos sistemas responsáveis pela exploração.

Este documento não descreve detalhes técnicos de implementação.

Seu foco é definir o comportamento esperado do mundo do jogo.

---

# 2. Visão Geral

O Dungeon Crawler será construído sobre um mundo composto por diferentes regiões exploráveis.

Cada região possuirá sua própria identidade visual, temática e mecânica.

Ao invés de criar um único mapa contínuo, o jogador realizará expedições para diferentes dungeons.

Cada dungeon representa uma missão independente.

Após concluir ou abandonar uma expedição, o jogador retornará ao Hub para se preparar para uma nova incursão.

O ciclo geral será:

```text
HUB
 │
 ▼
Selecionar Dungeon
 │
 ▼
Explorar
 │
 ▼
Combater
 │
 ▼
Obter Recompensas
 │
 ▼
Retornar ao HUB
 │
 ▼
Preparação
 │
 ▼
Nova Expedição
```

Este modelo permite:

* Alta rejogabilidade;
* Evolução contínua do personagem;
* Facilidade para adicionar novas regiões;
* Expansão do conteúdo ao longo do desenvolvimento.

---

# 3. Filosofia de Design

O mundo deverá seguir cinco princípios fundamentais.

## 3.1 Identidade

Cada dungeon deverá possuir identidade própria.

Ela deverá ser reconhecida pelo jogador através de:

* Arquitetura;
* Bioma;
* Inimigos;
* Ambientação;
* Trilha sonora;
* Chefes;
* Recompensas.

O objetivo é fazer com que cada região seja memorável.

---

## 3.2 Progressão

O jogador deverá sentir que está avançando para locais cada vez mais perigosos.

A dificuldade crescerá gradualmente.

Novas regiões introduzirão:

* Novos inimigos;
* Novas mecânicas;
* Novos desafios;
* Novas recompensas.

---

## 3.3 Exploração

A exploração deverá incentivar curiosidade.

Nem todo caminho precisará ser obrigatório.

Algumas áreas poderão conter:

* Eventos;
* Tesouros;
* Salas secretas;
* NPCs;
* Recursos raros.

O jogador será recompensado por explorar.

---

## 3.4 Risco

Quanto maior o avanço na dungeon, maior deverá ser o risco.

O jogador precisará administrar cuidadosamente:

* Vida;
* Itens;
* Recursos;
* Equipamentos.

Essa administração faz parte da experiência principal do jogo.

---

## 3.5 Rejogabilidade

Mesmo explorando a mesma dungeon diversas vezes, cada expedição deverá oferecer uma experiência diferente.

Essa decisão será alcançada através do modelo híbrido de geração.

---

# 4. Estrutura Geral do Mundo

O mundo será organizado em torno de um Hub central.

A partir dele, o jogador poderá acessar diferentes dungeons.

```text
                     HUB

                      │

     ┌────────────────┼────────────────┐
     │                │                │

     ▼                ▼                ▼

 Cripta          Floresta          Catacumbas

     │                │                │

     ▼                ▼                ▼

 Exploração      Exploração      Exploração

     │                │                │

     ▼                ▼                ▼

 Boss             Boss             Boss
```

Cada dungeon será uma região independente.

Isso facilita:

* Expansão futura;
* Balanceamento;
* Desenvolvimento incremental;
* Organização da arquitetura.

---

# 5. Estrutura das Regiões

Cada região será composta pelos seguintes elementos.

```text
REGIÃO

│

├── Nome

├── Bioma

├── História

├── Dungeons

├── Inimigos

├── Eventos

├── Recursos

├── Boss

└── Recompensas
```

Cada região poderá introduzir novas mecânicas específicas.

Exemplo:

Uma floresta pode possuir armadilhas naturais.

Uma cripta pode possuir mortos-vivos.

Uma mina abandonada pode possuir áreas instáveis.

Essas características tornam cada região única.

---

# 6. Biomas

Os biomas representam a identidade visual e mecânica das regiões.

Cada bioma deverá possuir:

* Paleta visual;
* Ambientação;
* Sons;
* Tipos de inimigos;
* Eventos próprios;
* Recursos específicos.

Exemplo inicial de biomas:

| Bioma    | Características                                       |
| -------- | ----------------------------------------------------- |
| Cripta   | Mortos-vivos, corredores estreitos, atmosfera sombria |
| Floresta | Natureza, armadilhas, criaturas selvagens             |
| Mina     | Estruturas antigas, minerais, monstros subterrâneos   |
| Castelo  | Cavaleiros, salões, armaduras vivas                   |
| Ruínas   | Magia antiga, construções destruídas, artefatos       |

Novos biomas poderão ser adicionados futuramente sem necessidade de alterar a estrutura principal do jogo.

---

# 7. Estrutura das Dungeons

Cada dungeon será composta por uma sequência organizada de andares (floors).

A estrutura geral será:

```text
Dungeon

│

├── Entrada

├── Floor 1

├── Floor 2

├── Floor 3

├── ...

└── Boss
```

Cada dungeon possuirá:

* Nome;
* Tema;
* Bioma;
* Dificuldade;
* Quantidade de floors;
* Boss principal;
* Recompensas exclusivas.

Esses elementos definem a identidade da dungeon.

---

# 8. Modelo Híbrido de Dungeon

O Dungeon Crawler adotará oficialmente um modelo híbrido de geração.

Este modelo combina uma estrutura fixa com conteúdo gerado dinamicamente.

## Estrutura fixa

Os seguintes elementos serão definidos manualmente:

* Nome da dungeon;
* História;
* Bioma;
* Boss;
* Número de floors;
* Progressão entre áreas;
* Dificuldade base.

Esses elementos nunca serão alterados.

---

## Conteúdo dinâmico

Durante cada expedição, o jogo poderá gerar dinamicamente:

* Salas;
* Eventos;
* Inimigos;
* Baús;
* Ouro;
* Itens;
* Elites;
* NPCs;
* Recursos;
* Pequenos caminhos alternativos.

Cada partida oferecerá uma experiência diferente.

---

## Estrutura conceitual

```text
Dungeon

↓

Estrutura Fixa

↓

Dungeon Generator

↓

Selecionar Salas

↓

Selecionar Eventos

↓

Selecionar Inimigos

↓

Selecionar Loot

↓

Montar Expedição
```

Esta abordagem oferece um equilíbrio entre consistência e rejogabilidade.

---

# 9. Benefícios do Modelo Híbrido

A adoção do modelo híbrido proporciona diversas vantagens.

## Para o jogador

* Exploração sempre diferente;
* Progressão consistente;
* Identidade clara de cada dungeon;
* Maior fator de descoberta.

---

## Para o projeto

* Arquitetura desacoplada;
* Facilidade para testes automatizados;
* Facilidade para expansão do conteúdo;
* Reutilização de componentes;
* Melhor organização do domínio.

---

## Para o portfólio

A implementação deste modelo permitirá demonstrar conhecimentos em:

* Engenharia de Software;
* Programação Orientada a Objetos;
* Arquitetura em camadas;
* Padrões de projeto;
* Geração procedural controlada;
* Testes automatizados.

Essa decisão está alinhada ao objetivo do projeto de servir como um portfólio técnico robusto.

---

# 10. Relação com as Próximas Etapas

As definições deste documento servirão de base para:

* ETAPA 02 — Arquitetura;
* ETAPA 03 — Núcleo de Domínio;
* ETAPA 05 — Combate;
* ETAPA 06 — Inimigos;
* ETAPA 08 — Dungeon e Exploração;
* ETAPA 09 — Progressão;
* ETAPA 10 — Persistência.

Todas essas etapas utilizarão as estruturas definidas neste documento como referência.

---

# Status da Documentação

**Documento:** WORLD_AND_DUNGEON.md

**Parte:** 1 de 3

**Status:** Em desenvolvimento.

A próxima parte detalhará:

* Floors;
* Tipos de salas;
* Fluxo de exploração;
* Navegação;
* Estrutura interna das dungeons;
* Sistema de geração híbrida das salas.

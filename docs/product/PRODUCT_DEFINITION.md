# Product Definition — Índice

**Projeto:** Dungeon Crawler
**Documento:** `docs/product/PRODUCT_DEFINITION.md`
**Etapa:** ETAPA 00 — Visão e Definição do Produto

A definição do produto está distribuída em documentos por fase. Este índice indica onde cada assunto é tratado.

---

# 1. Documentos por Fase

| Fase | Documento |
|---|---|
| FASE 01 — Product Vision | [PRODUCT_VISION.md](PRODUCT_VISION.md) |
| FASE 02 — Game Concept | [GAME_CONCEPT.md](GAME_CONCEPT.md) |
| FASE 03 — Core Gameplay | [CORE_GAMEPLAY.md](CORE_GAMEPLAY.md) |
| FASE 04 — Game Mechanics | [GAME_MECHANICS.md](GAME_MECHANICS.md) |
| FASE 05 — World & Dungeon | [WORLD_AND_DUNGEON.md](WORLD_AND_DUNGEON.md) |
| FASE 06 — Characters | [CHARACTERS.md](CHARACTERS.md) |
| FASE 07 — Enemies | [ENEMIES.md](ENEMIES.md) |
| FASE 08 — Progression | [PROGRESSION.md](PROGRESSION.md) |
| FASE 09 — MVP | [MVP.md](MVP.md) |
| FASE 10 — Requirements | [REQUIREMENTS.md](REQUIREMENTS.md) |

---

# 2. Assuntos

| Assunto | Onde está |
|---|---|
| Visão do produto | PRODUCT_VISION.md §1–§2 |
| Objetivo | PRODUCT_VISION.md §6–§7 |
| Proposta do jogo | GAME_CONCEPT.md §1–§2 |
| Público-alvo | PRODUCT_VISION.md §5 |
| Experiência do jogador | GAME_CONCEPT.md §3–§4, §11 |
| Core gameplay loop | CORE_GAMEPLAY.md |
| Mecânicas | GAME_MECHANICS.md |
| Personagens | CHARACTERS.md |
| Inimigos | ENEMIES.md |
| Mundo e dungeons | WORLD_AND_DUNGEON.md |
| Progressão | PROGRESSION.md |
| Inventário e itens | GAME_MECHANICS.md §9–§11; MVP.md §19–§22 |
| Persistência | Fora do MVP — MVP.md §42 |
| Escopo do MVP | MVP.md |
| Requisitos funcionais | REQUIREMENTS.md §5–§15 |
| Requisitos não funcionais | REQUIREMENTS.md §16–§18 |
| Stack tecnológica | PRODUCT_VISION.md §4; `docs/architecture/ARCHITECTURE.md` |
| Critérios de sucesso | PRODUCT_VISION.md §13; MVP.md §36–§38 |
| Fora do escopo | PRODUCT_VISION.md §14; MVP.md §34; REQUIREMENTS.md §19 |

---

# 3. Precedência

Quando dois documentos divergirem, prevalece o mais recente e normativo:

```text
REQUIREMENTS.md  >  MVP.md  >  PROGRESSION.md  >  demais documentos
```

As divergências já encontradas e suas resoluções estão em `docs/architecture/adr/0003-consolidacao-de-divergencias-da-documentacao-de-produto.md`.

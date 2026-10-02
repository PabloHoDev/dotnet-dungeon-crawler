# ADR-0003 — Consolidação de divergências da documentação de produto

**Status:** Aceita
**Data:** 2026-10-02

## Contexto

Os documentos da ETAPA 00 foram escritos em sequência e alguns pontos divergiam entre si. A implementação precisa de uma única resposta para cada um.

## Decisão

Em caso de conflito, prevalecem os documentos mais recentes e normativos: `REQUIREMENTS.md`, depois `MVP.md`, depois `PROGRESSION.md`.

| Tema | Divergência | Adotado | Documento corrigido |
|---|---|---|---|
| Atributos | `CHARACTERS.md` listava seis (incluía Defesa); `PROGRESSION.md`, `MVP.md` e RF-003 listam cinco | Cinco atributos: Strength, Vitality, Agility, Intelligence, Luck. Defesa é um atributo **derivado** | `CHARACTERS.md` §12, §14, §42 |
| Nomes das classes | `CHARACTERS.md` usa Guerreiro / Arcanista / Ladino; `MVP.md` e RF-002 usam Warrior / Mage / Rogue | No código: `Warrior`, `Mage`, `Rogue`. Os nomes em português são texto de interface | `CHARACTERS.md` §7 |
| Bestiário | `ENEMIES.md` listava Goblin, Skeleton, Wolf; `MVP.md` e RF-015 listam Goblin, Skeleton, Orc | Goblin, Skeleton, Orc. Elite = Goblin com modificador; Mini Boss = Skeleton Knight; Boss = Dungeon Boss | `ENEMIES.md` §17, §44, §45, §55 |
| Persistência | `GAME_MECHANICS.md` a colocava como prioridade alta; `MVP.md` §42 a classifica como FUTURE | Fora do MVP. A progressão permanente é preservada em memória entre expedições da mesma sessão; Save/Load fica para a ETAPA 10 | `GAME_MECHANICS.md` §19 |
| Hub | `GAME_CONCEPT.md` descreve loja, NPCs e melhorias; `MVP.md` não os inclui | Hub mínimo: ver personagem, gerenciar inventário/equipamento, iniciar expedição | `CORE_GAMEPLAY.md` §8 |
| Eventos de sala | `WORLD_AND_DUNGEON.md` os detalha; RF-023 os deixa para "definição posterior" | Fora do Golden Path do MVP. A estrutura de salas deve permitir adicioná-los | `CORE_GAMEPLAY.md` §8 |
| Curva de XP | `XP(n) = 50 × n × (n + 1)`, sem dizer se o valor é o total ou o custo do nível | `XP(n)` é a XP **acumulada** para sair do nível `n`. Nível 1 começa em 0; nível 2 em 100; nível 3 em 300. O custo do nível `n` é `100 × n`. O nível 20 é alcançado com 19.000 de XP | `PROGRESSION.md` §8 |

## Documentos incompletos

Na mesma revisão foram resolvidos:

| Arquivo | Situação anterior | Resolução |
|---|---|---|
| `docs/product/CORE_GAMEPLAY.md` | Vazio, com a FASE 03 marcada como concluída | Escrito a partir de `GAME_CONCEPT.md`, `GAME_MECHANICS.md` e `MVP.md`, sem regras novas |
| `docs/product/PRODUCT_DEFINITION.md` | Apenas um sumário | Transformado em índice da documentação de produto |
| `docs/product/GAMEPLAY.md` | Vazio, duplicava `CORE_GAMEPLAY.md` | Removido |
| `docs/product/PRODUCT_OVERVIEW.md` | Vazio, duplicava `PRODUCT_VISION.md` | Removido |
| `REQUERIMENTS.md`, `WORLD_AND_DUGEON.md` | Nomes com erro de digitação | Renomeados para `REQUIREMENTS.md` e `WORLD_AND_DUNGEON.md` |

Os cabeçalhos de status dos documentos da ETAPA 00 foram alinhados ao roadmap (todas as fases concluídas).

## Consequências

- A ETAPA 04 implementa cinco atributos e três classes com os identificadores em inglês.
- A ETAPA 06 implementa Goblin, Skeleton e Orc.
- A ETAPA 09 implementa a curva de XP com a interpretação acumulada.

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

# 11. Estrutura dos Floors

Cada dungeon será composta por um conjunto de andares (Floors).

Os Floors representam a progressão física da exploração.

Cada novo andar deverá apresentar:

* Maior dificuldade;
* Novos desafios;
* Maior risco;
* Melhores recompensas.

Estrutura conceitual:

```text
Dungeon
│
├── Floor 1
├── Floor 2
├── Floor 3
├── ...
└── Boss Floor
```

Cada Floor será independente em sua geração, porém seguirá as regras da dungeon à qual pertence.

---

# 12. Objetivos dos Floors

Cada Floor deverá possuir um papel dentro da experiência do jogador.

Exemplo:

| Floor      | Objetivo                       |
| ---------- | ------------------------------ |
| Floor 1    | Introdução da expedição        |
| Floor 2    | Aumento gradual da dificuldade |
| Floor 3    | Maior pressão sobre recursos   |
| Floor 4    | Elite e desafios especiais     |
| Boss Floor | Confronto final                |

Essa distribuição poderá variar entre diferentes dungeons.

---

# 13. Estrutura das Salas

Cada Floor será composto por diferentes salas.

Exemplo conceitual:

```text
Floor

Entrada
   │
   ▼
Combate
   │
   ▼
Evento
   │
   ├────────────┐
   ▼            ▼
Tesouro     Combate
   │            │
   └─────┬──────┘
         ▼
       Elite
         │
         ▼
      Saída
```

A ordem das salas poderá variar conforme o gerador híbrido.

---

# 14. Tipos de Salas

O jogo utilizará diferentes categorias de salas.

## Sala Inicial

Primeira sala do Floor.

Responsável por iniciar a exploração.

---

## Sala de Combate

Inicia um combate comum.

Pode conter:

* Um inimigo;
* Grupo de inimigos;
* Inimigos especiais.

---

## Sala de Evento

Apresenta decisões ao jogador.

Exemplos:

* Altar misterioso;
* Comerciante perdido;
* Armadilha;
* Porta secreta;
* Fonte de cura;
* Escolhas morais.

---

## Sala de Tesouro

Concede recompensas.

Pode conter:

* Ouro;
* Equipamentos;
* Consumíveis;
* Recursos especiais.

---

## Sala Elite

Possui inimigos significativamente mais difíceis.

Recompensas superiores.

Maior risco.

---

## Sala Especial

Salas raras.

Exemplos:

* Biblioteca
* Santuário
* Arena
* Laboratório
* Prisão
* Cofre

---

## Sala do Chefe

Última sala da dungeon.

Representa o principal desafio da expedição.

---

# 15. Fluxo de Exploração

O jogador deverá navegar entre salas até alcançar o objetivo da dungeon.

Fluxo básico:

```text
Entrada

↓

Explorar

↓

Descobrir Sala

↓

Interagir

↓

Resolver Situação

↓

Receber Resultado

↓

Escolher Caminho

↓

Próxima Sala
```

Esse ciclo se repetirá até que o jogador:

* Derrote o chefe;
* Seja derrotado;
* Abandone a expedição.

---

# 16. Sistema de Conexões

As salas serão conectadas formando caminhos.

Exemplo:

```text
      Entrada
         │
         ▼
      Combate
      ┌──┴──┐
      ▼     ▼
 Evento   Combate
      │     │
      └──┬──┘
         ▼
      Tesouro
         │
         ▼
       Elite
         │
         ▼
      Boss
```

Cada conexão representa um caminho válido.

O sistema deverá permitir:

* Caminhos lineares;
* Bifurcações;
* Pequenos desvios;
* Áreas opcionais.

Evitar labirintos excessivamente complexos é um objetivo do MVP.

---

# 17. Exploração Não Linear

Embora exista um objetivo principal, o jogador poderá encontrar caminhos alternativos.

Exemplos:

* Salas opcionais;
* Eventos escondidos;
* Tesouros secretos;
* Atalhos;
* Pequenas áreas secundárias.

Esses caminhos aumentam a sensação de descoberta.

---

# 18. Modelo Híbrido de Geração

O jogo utilizará geração híbrida.

O fluxo será:

```text
Selecionar Dungeon

↓

Selecionar Floor

↓

Aplicar Estrutura Base

↓

Sortear Salas

↓

Sortear Eventos

↓

Sortear Inimigos

↓

Sortear Loot

↓

Montar Expedição
```

A estrutura principal nunca será alterada.

O conteúdo será gerado dinamicamente.

---

# 19. Regras de Geração

O gerador deverá obedecer algumas regras.

## Sempre existir

* Entrada;
* Caminho até o chefe;
* Saída válida.

---

## Nunca existir

* Caminhos impossíveis;
* Salas inacessíveis;
* Chefes bloqueados;
* Loops infinitos.

---

## Poderão existir

* Caminhos alternativos;
* Salas opcionais;
* Eventos raros;
* Tesouros escondidos.

---

# 20. Distribuição das Salas

Exemplo conceitual de distribuição:

| Tipo     |      Frequência |
| -------- | --------------: |
| Combate  |      Muito Alta |
| Evento   |           Média |
| Tesouro  |           Média |
| Elite    |           Baixa |
| Especial |     Muito Baixa |
| Boss     | Uma por Dungeon |

Os valores exatos serão balanceados durante o desenvolvimento.

---

# 21. Escalonamento Interno

A dificuldade aumentará conforme o jogador avança.

Exemplo:

```text
Floor 1
★

Floor 2
★★

Floor 3
★★★

Floor 4
★★★★

Boss
★★★★★
```

O aumento poderá ocorrer por meio de:

* Mais inimigos;
* Inimigos mais fortes;
* Eventos mais difíceis;
* Menor disponibilidade de recursos.

---

# 22. Objetivo do Gerador

O gerador não deverá criar mapas totalmente aleatórios.

Seu objetivo será:

* Preservar a identidade da dungeon;
* Garantir boa jogabilidade;
* Oferecer variedade;
* Evitar partidas repetitivas.

Cada expedição deverá parecer nova, mas familiar.

---

# 23. Relação com Arquitetura

As definições desta parte originarão futuramente componentes como:

* Dungeon
* Floor
* Room
* RoomType
* RoomConnection
* DungeonLayout
* DungeonGenerator
* ExplorationFlow

Esses elementos serão modelados na ETAPA 02 — Arquitetura e implementados no Núcleo de Domínio.

---

# Status da Documentação

**Documento:** WORLD_AND_DUNGEON.md

**Parte:** 2 de 3

**Status:** Em desenvolvimento.

A próxima parte concluirá o documento com:

* Eventos;
* Chefes;
* Progressão entre Dungeons;
* Sistema de Descoberta;
* Escalonamento Global;
* Relação com as próximas etapas;
* Critérios de validação;
* Status final da fase.

# 24. Sistema de Eventos

Os eventos representam situações especiais encontradas durante a exploração da dungeon.

Seu objetivo é interromper o ciclo tradicional de exploração e combate, oferecendo ao jogador momentos de tomada de decisão.

Os eventos deverão aumentar a variedade da experiência e incentivar múltiplas partidas.

---

## 24.1 Objetivos dos Eventos

Os eventos possuem cinco objetivos principais.

* Criar imprevisibilidade;
* Estimular decisões estratégicas;
* Recompensar exploração;
* Introduzir elementos narrativos;
* Variar o ritmo da partida.

Os eventos não deverão existir apenas para conceder recompensas.

Cada evento deverá apresentar pelo menos uma consequência relevante.

---

## 24.2 Estrutura Conceitual

Todo evento seguirá o mesmo fluxo conceitual.

```text
EVENTO

↓

SITUAÇÃO

↓

DECISÃO

↓

CONSEQUÊNCIA

↓

CONTINUAÇÃO DA EXPLORAÇÃO
```

Esse modelo facilita a padronização da arquitetura do sistema de eventos.

---

## 24.3 Categorias de Eventos

Inicialmente os eventos serão classificados nas seguintes categorias.

### Eventos Positivos

Oferecem vantagens ao jogador.

Exemplos:

* Fonte de cura;
* Baú especial;
* Comerciante;
* Altar sagrado;
* NPC aliado.

---

### Eventos Negativos

Apresentam riscos imediatos.

Exemplos:

* Armadilhas;
* Maldições;
* Emboscadas;
* Perda de recursos;
* Salas instáveis.

---

### Eventos Neutros

Dependem da decisão do jogador.

Exemplos:

* Porta misteriosa;
* Alavanca desconhecida;
* Estátua antiga;
* Artefato mágico;
* Escolha entre caminhos.

---

### Eventos Especiais

Eventos extremamente raros.

Exemplos:

* Mini Boss oculto;
* Sala secreta;
* Tesouro lendário;
* NPC raro;
* Portal para área especial.

---

# 25. Sistema de Chefes

Cada dungeon possuirá um chefe principal.

O chefe representa o maior desafio daquela região.

Ele também representa o encerramento da expedição.

---

## 25.1 Objetivos

Os chefes deverão:

* Testar o domínio das mecânicas aprendidas;
* Representar o ápice da dificuldade;
* Possuir identidade própria;
* Ser memoráveis.

---

## 25.2 Características

Cada chefe possuirá:

* Nome;
* História;
* Aparência própria;
* Mecânicas exclusivas;
* Ataques especiais;
* Recompensas únicas.

Exemplo conceitual:

```text
CHEFE

↓

Introdução

↓

Combate

↓

Fases

↓

Derrota

↓

Recompensa

↓

Conclusão da Dungeon
```

---

## 25.3 Estrutura das Lutas

As batalhas contra chefes deverão ser diferentes dos combates comuns.

O objetivo é criar desafios mais estratégicos.

Os chefes poderão possuir:

* Múltiplas fases;
* Mudanças de comportamento;
* Habilidades exclusivas;
* Alterações no campo de batalha.

Esses elementos tornam cada encontro mais memorável.

---

# 26. Progressão entre Dungeons

O jogador não iniciará diretamente nas áreas mais difíceis.

O avanço ocorrerá gradualmente.

Fluxo conceitual:

```text
Dungeon Inicial

↓

Dungeon Intermediária

↓

Dungeon Avançada

↓

Dungeon Final
```

Cada nova dungeon introduzirá:

* Novos inimigos;
* Novos desafios;
* Novos biomas;
* Novas recompensas;
* Novas mecânicas.

---

## 26.1 Desbloqueio

Novas dungeons poderão ser desbloqueadas por diferentes critérios.

Exemplos:

* Derrotar um chefe;
* Alcançar determinado nível;
* Encontrar um artefato;
* Completar uma missão.

Essas regras serão detalhadas posteriormente.

---

# 27. Sistema de Descoberta

A exploração deverá incentivar a curiosidade do jogador.

Nem todo conteúdo será apresentado imediatamente.

O jogador poderá descobrir:

* Salas ocultas;
* Eventos secretos;
* Tesouros raros;
* NPCs especiais;
* Caminhos alternativos.

Esse sistema aumenta significativamente a sensação de exploração.

---

## 27.1 Recompensa pela Exploração

Explorar deverá ser vantajoso.

O jogador poderá receber:

* Ouro;
* Equipamentos;
* Recursos;
* Informações;
* Itens raros;
* Conteúdo opcional.

Quanto maior a exploração, maiores poderão ser as recompensas.

---

# 28. Escalonamento Global de Dificuldade

O jogo deverá aumentar sua dificuldade progressivamente.

Esse aumento ocorrerá em diferentes níveis.

---

## Escalonamento por Região

Cada região será mais difícil que a anterior.

Exemplo:

```text
Cripta

↓

Floresta

↓

Mina

↓

Castelo

↓

Ruínas
```

---

## Escalonamento por Dungeon

Dentro da mesma região, cada dungeon poderá possuir níveis diferentes de dificuldade.

---

## Escalonamento por Floor

Cada novo andar deverá apresentar maior dificuldade.

---

## Escalonamento por Sala

Mesmo dentro do mesmo andar, determinadas salas poderão representar desafios maiores.

Exemplos:

* Sala Elite;
* Eventos especiais;
* Mini Boss;
* Sala do Chefe.

---

## Objetivo do Escalonamento

O aumento gradual da dificuldade deverá proporcionar:

* Sensação de evolução;
* Novos desafios;
* Necessidade de planejamento;
* Melhor utilização das mecânicas do jogo.

O crescimento da dificuldade deverá acompanhar a evolução do personagem.


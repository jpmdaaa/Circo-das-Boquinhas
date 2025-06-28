
## DEVLOG - Histórico de Atualizações

### **2025-06-28**
- **Att4 28/06**:
  1. Redução do tempo da mãozinha no tutorial.
  2. Permite dois tiros antes de passar a rodada em caso de erro.

- **Att3 28/06**:
  1. Correção de dicionários/mapa de respostas por modo.
  2. Ajuste na pontuação.
  3. Correção da tela de estrelas/final de jogo.
  4. Revisão da rotação do canhão no tutorial.

- **Att2 28/06**:
  - Correções:
    1. Problema de rodadas pulando (turno de jogador ignorado).
    2. Rotações do canhão no tutorial.
    3. Áudio da imagem não tocando no início.
    4. Áudio da imagem não tocando ao passar de turno.
  - Sons adicionados:
    1. Áudio do tutorial.
    2. Cortina abrindo.
    3. Faixa desenrolando.
    4. Tiro e disparo do canhão.

- **Att1 28/06**:
  1. Botão para tocar o som da boquinha novamente.
  2. Correção da quantidade de imagens por turno.
  3. Ajuste na animação de rotação do canhão.
  4. Organização da ordem das animações por turno.
  5. Correção da passagem de rodada ao trocar de turno.
  6. Ajuste do tempo conforme o número de jogadores.
  7. Sons correspondentes às respostas corretas e mapeamento.

### **2025-06-26**
- **Att Gameplay**:
  1. Correção da ordem da UI (elementos sobrepostos).
  2. Posicionamento do canhão à frente do feedback.
  3. Animação de balões estourando.
  4. Correção de painel preto ao acertar.
  5. Destruição de confetes ao sair da tela.

- **Att Round/Pontuação/Finalização**:
  1. Passagem de rodada ao acertar ou errar.
  2. Destruição de tiros instanciados.
  3. Ajuste de posicionamento dos cards dos jogadores.
  4. Passagem de rodada ao terminar o tempo.
  5. Cálculo da pontuação final.
  6. Finalização da partida.
  7. Atualização de ícones a cada rodada.
  8. Novo mapeamento das boquinhas.

### **2025-06-24**
- **Primeiro Commit**: Projeto Unity inicial criado.

---

# README - O Circo das Boquinhas

## Visão Geral do Jogo
**O Circo das Boquinhas** é um jogo educativo desenvolvido para estimular a consciência fonoarticulatória em crianças, auxiliando no processo de alfabetização. O jogo utiliza associações entre sons, letras e imagens em um ambiente lúdico de circo, onde os jogadores interagem com um canhão para acertar respostas corretas. O projeto é uma parceria entre a Playmove e o Método das Boquinhas.

---

## Game Design Document (GDD) - Resumo

### **Objetivo do Jogo**
- Desenvolver a consciência fonológica por meio da associação de sons, letras e imagens.
- Pode ser jogado individualmente ou em grupo (1-4 jogadores).

### **Modos de Jogo**
1. **Modo Boquinhas**: O jogador ouve um som e deve acertar a boquinha correspondente.
2. **Modo Letras**: O jogador vê uma boquinha e deve acertar a letra correspondente.
3. **Modo Figuras**: O jogador vê uma boquinha e deve acertar as figuras que começam com o mesmo som.

### **Fluxo do Jogo**
1. **Tela Inicial**: 
   - Botões: Jogar, Menu, Logo Boquinhas (tela institucional), Sistema Operacional (SOP).
   - Elementos decorativos (borracha, caneta, lápis).
2. **Menu**: 
   - Abas: Informações, Como Jogar, Configurações (narrações, trilhas sonoras, tempo).
3. **Tela de Seleção**: 
   - Escolha de avatares e modo de jogo.
   - Botão "Jogar" habilitado após seleção.
4. **Gameplay**: 
   - Tutorial dinâmico conforme o modo selecionado.
   - Rodadas e turnos com feedbacks visuais e sonoros.
   - Cálculo de pontuação baseado em acertos e erros.
5. **Finalização**: 
   - Tela de resultados com estrelas (0 a 3) e opções para jogar novamente ou voltar ao menu.

### **Mecânicas Principais**
- **Canhão Interativo**: Usado para mirar e atirar nos alvos corretos.
- **Feedbacks**: 
  - Positivo: "Muito bem, você acertou!" + animação do coelho.
  - Negativo: "Tente novamente!" ou "O Show Fracassou!" + animação triste.
- **Tempo**: Barra de tempo que varia conforme o número de jogadores e rodadas.
- **Pontuação**: 
  - Acertos na primeira tentativa valem 1 ponto; na segunda, 0,5 ponto.
  - Estrelas concedidas com base na porcentagem de acertos.

### **Configurações**
- **Narrações**: Podem ser desativadas (exceto sons essenciais para a gameplay).
- **Trilhas Sonoras**: Opção de mutar sons decorativos.
- **Tempo**: Pode ser removido da gameplay.

### **Público-Alvo**
- Crianças em fase de alfabetização.
- Crianças com dificuldades de aprendizagem ou distúrbios de leitura e escrita.

---

## Considerações Finais
O jogo prioriza o aspecto pedagógico, alinhado ao Método das Boquinhas, com elementos lúdicos para engajar as crianças. Para dúvidas ou sugestões, consulte o [site oficial do Método das Boquinhas](http://www.metododasboquinhas.com.br). 

**Desenvolvido por:** Playmove Indústria e Comércio S/A.  
**Versão atual:** 1.1 (07/05/2025).

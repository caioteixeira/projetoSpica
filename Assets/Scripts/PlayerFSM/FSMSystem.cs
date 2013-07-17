using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Este sistema de Maquina de Estados finitos foi escrito por Roberto Cezar Bianchini
em Julho de 2010 utilizando como base o capitulo 3.1 do livro Game Programming Gems 1 de Eric Dybsand,
foi traduzido e modificado em Abril de 2012 pela Truesoft, 
faz parte do código-fonte do Projeto Spica;

Versão original disponivel em:
http://unifycommunity.com/wiki/index.php?title=Finite_State_Machine&_sm_byp=iVVMSjb2m2nDkVZj

How to use/Como usar:
    1.	Insira os valores para as transições e estados da Maquina de estados 
    	nos Enums correspondentes; 

    2. 	Escreva nova(s) classes herdando de FSMState, preencha cada uma com pares (Transitions/States).
       	Esses pares representam o estado S2 que o sistema pode entrar caso durante o estado S1,
       	uma transição T seja disparada e o estado S1 tenha a transição dele para S2.
       	Vale lembrar que essa é uma FSM deterministica.
       	Você não pode ter uma transição levando para dois estados diferentes;
       	
       	O método Reason() é usado para determinar quando e qual transição deve ser disparada.
       	Você também pode escrever o código para disparar transições em outro lugar, apenas 
       	deixe esse método vazio caso perceba que não é apropriado a seu projeto.
       	
       	O método Act() possui o código das ações que serão acionados durante o periodo em que o estado
       	estiver ativo. 
       	Você também pode escrever o código em outro local, apenas deixe o método vazio se sentir que não
       	é apropriado para suas necessidades.
               
    3. 	Crie uma instância do FSMSystem e adicione os estados (já instancializados).
     
    4. 	Chame os métodos Act e Reason (ou qualquer outro que você utilize para ações e disparo de transições)
    	a partir dos métodos Update ou FixedUpdate (padrões do Unity);
         
    Transições Assincronas da Unity Engine, como OnTriggerEnter, SendMessage, também podem ser usadas,
    apenas chame o método PerformTransition da sua instância do FSMSystem passando como parâmetro a transição
    desejada para quando este evento ocorrer.
    
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
AND NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/


/// <summary>
/// Coloque os nomes das Transições neste enum.
/// Não troque o primeiro valor, NullTransition pois a classe FSMSystem a utiliza.
/// </summary>
public enum Transition
{
    NullTransition = 0, // Use esta transição para representar uma transição não existente no sistema;
	ToIdle,
	ToWalk,
	ToRun,
	ToJump,
	ToDefend,
	ToAtack,
	ToDamage,
	ToDie,
	ToPatrulha,
	ToLuta
}

/// <summary>
/// Coloque os nomes dos Estados neste enum.
/// Não troque o primeiro valor, NullStateID pois a classe FSMSystem a utiliza.
/// </summary>
public enum StateID
{
    NullStateID = 0, // Use este ID para representar um estado não presente no sistema; 
	Idle,
	Walk,
	Run,
	Jump,
	Defend,
	Atack,
	Damage,
	Die,
	Patrulha,
	Luta
}

/// <summary>
/// Esta classe representa os estados da FSM
/// Cada estado possui um Dictionary com pares (transição-estado)
/// mostrando que estado a FSM deve ativar se a transição for ativado
/// enquanto o estado for o estado atual.
/// Método Reason pode ser usado para determinar quando uma transição deve ser disparada.
/// Método Act possui o código das ações que devem ser executadas quando se está no estado.
/// </summary>
public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;
    public StateID ID { get { return stateID; } }
	
	public StateID previousStateID; // Alteração

    public void AddTransition(Transition trans, StateID id)
    {
        // Checa se um dos argumentos é invalido;
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERRO: NullTransition nao é permitida para uma transicao real");
            return;
        }

        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSMState ERRO: NullStateID nao é permitido para um StateID real");
            return;
        }

        // Como esta é uma maquina deterministica
        //   checa se a transição já está presente no mapa de transições
        if (map.ContainsKey(trans))
        {
            Debug.LogError("FSMState ERRO: Estado " + stateID.ToString() + " ja tem a transicao " + trans.ToString() +
                           "Impossivel aplicar para outro estado");
            return;
        }

        map.Add(trans, id);
    }

    /// <summary>
    /// Este método deleta um par (Transição-StateID) do mapa de transições
    /// Se o par não estiver presente no mapa, retorna um erro.
    /// </summary>
    public void DeleteTransition(Transition trans)
    {
        // Testa se é NullTransition;
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSMState ERRO: NullTransition nao é permitida");
            return;
        }

        // Testa se o par está presente no mapa antes de deletar;
        if (map.ContainsKey(trans))
        {
            map.Remove(trans);
            return;
        }
        Debug.LogError("FSMState ERRO: Transicao " + trans.ToString() + " passada para " + stateID.ToString() +
                       " nao esta na List de transicoes do estado");
    }

    /// <summary>
    /// Este método retorna o estado que será ativado se a
    ///    transição for disparada
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        // Testa se o mapa tem a transição
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }

    /// <summary>
    /// Este método pode ser usado para setar as condições do estado, antes de
    /// ativa-lo.
    /// É chamado automaticamente pela classe FSMSystem antes de entrar
    /// t
    /// </summary>
    public virtual void DoBeforeEntering(GameObject gameObject) { } //Alteração

    /// <summary>
    /// Este método pode ser usado para resetar variaveis, ou qualquer outra
    /// função antes de executar a transição para outro estado.
    /// É chamado automaticamente pela classe FSMSystem antes de mudar para
    /// outro estado.
    /// </summary>
    public virtual void DoBeforeLeaving(GameObject gameObject) { } //Alteração

    /// <summary>
    /// Este método pode ser usado para determinar se serão executadas 
    /// transições ou não.
    /// O GameObject NPC, é uma referencia ao objeto que é controlado
    /// pelo sistema.
    /// </summary>
    public abstract void Reason(GameObject player, GameObject gameObject);

    /// <summary>
    /// Este método controla o objeto no mundo do jogo.
    /// Qualquer ação, movimento ou comunicação, deve ser
    /// colocado aqui.
    /// O GameObject NPC é uma referencia ao objeto que é
    /// controlado pelo sistema./// </summary>
    public abstract void Act(GameObject player, GameObject gameObject);

} // class FSMState

[System.Serializable]
[Serialization.SerializeAll]
/// <summary>
/// Classe FSMSystem representa a classe principal da FSM
/// Possui uma List com os estados que o objecto possui e métodos para
/// adicionar e deletar estados, e para trocar o estado atual da Máquina.
/// </summary>
public class FSMSystem
{
    private List<FSMState> states;

    // A unica forma de trocar o estado atual é através das transições
    // Não é possivel trocar o currentState (estado atual) diretamente.
    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }
	public GameObject gObject;
	
	public FSMSystem() //UNITY Serializer Construtor
	{
		states = new List<FSMState>();
		
	}

    public FSMSystem(GameObject gameObject)
    {
        states = new List<FSMState>();
		
		gObject = gameObject;
    }

    /// <summary>
    /// Este método adiciona novos estados na FSM,
    /// ou imprime uma mensagem de erro se o estado já estiver na lista.
    /// Primeiro estado adicionado é também o estado inicial.
    /// </summary>
    public void AddState(FSMState s)
    {
        // Testa se a referência é nula, antes de executar.
        if (s == null)
        {
            Debug.LogError("FSM ERRO: Referência nula nao e permitida! ");
        }

        //	Primeiro estado adicionado também é estado inicial,
		//   o estado que a máquina estará quando começar a executar.
        if (states.Count == 0)
        {
            states.Add(s);
            currentState = s;
            currentStateID = s.ID;
            return;
        }

        // Adiciona o estado na List, se ele não estiver presente.
        foreach (FSMState state in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("FSM ERRO: Impossivel adicinar estado " + s.ID.ToString() +
                               ", ja esta presente na lista de estados.");
                return;
            }
        }
        states.Add(s);
    }

    /// <summary>
    /// Este método deleta um estado da List, se ela estiver presente
    ///   ou imprime uma mensagem de erro se não estiver presente.
    /// </summary>
    public void DeleteState(StateID id)
    {
        // Testa se a referência é nula, antes de deletar.
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERRO: NullStateID nao e permitido para um estado real.");
            return;
        }

        // Percorre a List e deleta o estado, se estiver presente.
        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERRO: Impossivel deletor o estado " + id.ToString() +
                       ". Nao esta presente na lista de estados.");
    }

    /// <summary>
    /// Este método tenta trocar o estado da FSM baseando-se no
    /// estado atual e na transição disparada.
    /// Se o estado atual não tiver um estado associado a transição
    /// disparada, uma mensagem de erro é impressa.
    /// </summary>
    public void PerformTransition(Transition trans)
    {
        // Testa se a transição é nula antes de trocar o estado atual.
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("FSM ERRO: NullTransition nao e permitida para uma transicao real.");
            return;
        }

        // Testa se o estado atual tem a transição passada como argumento.
        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            Debug.LogError("FSM ERRO: Estado " + currentStateID.ToString() +  " nao possui um estado 'alvo' " +
                           " para a transicao " + trans.ToString());
            return;
        }
        // Atualiza as variaveis: currentStateID e currentState     
        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
				
                // Executa o método de saida do estado atual, antes de troca-lo.
                currentState.DoBeforeLeaving(gObject);
				
				StateID tmpPreviousState = currentState.ID;
				
				// Atualiza o estado atual.
                currentState = state;
				

                // Executa o método de entrada do novo estado atual.
				currentState.previousStateID = tmpPreviousState;
                currentState.DoBeforeEntering(gObject);
                break;
            }
        }
   
    } // PerformTransition()

} //class FSMSystem
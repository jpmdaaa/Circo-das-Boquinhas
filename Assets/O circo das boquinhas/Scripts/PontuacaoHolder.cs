using System.Collections.Generic;
using UnityEngine;

public class PontuacaoHolder : MonoBehaviour
{
    public List<PontuacaoJogador> pontuacoes = new List<PontuacaoJogador>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

using UnityEngine;

public class ErrorHolder : MonoBehaviour
{
    //XGH
    public int player0errors;
    public int player1errors;
    public int player2errors;
    public int player3errors;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
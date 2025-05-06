using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    enum CellState { Empty, Blocked }
    [SerializeField] private int width = 22;
    [SerializeField] private int height = 22;
    private CellState[,] board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         board = new CellState[width, height];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CellIsBlocked(int x, int y)
    {
        return board[x, y] == CellState.Blocked;
    }

    void SetCellBlocked(int x, int y)
    {
        board[x,y] = CellState.Blocked;
        // Raise BoardStateChanged event?
    }

    void SetCellEmpty(int x, int y)
    {
        board[x, y] = CellState.Empty;
        // Raise BoardStateChanged event?
    }
}

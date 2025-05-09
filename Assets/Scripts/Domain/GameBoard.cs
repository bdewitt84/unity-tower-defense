public class GameBoard
{
    private enum CellState { Empty, Blocked }

    private int width { get; }
    private int height { get; }
    private CellState[,] board;

    public GameBoard(int width, int height)
    {
        this.width = width;
        this.height = height;
        board = new CellState[width, height];
    }

    // Returns true if board cell at x, y has state 'Blocked'
    public bool IsCellBlocked(int x, int y)
    {
        return board[x, y] == CellState.Blocked;
    }

    // Returns true if board cell at x, y has state 'Empty'
    public bool IsCellEmpty(int x, int y)
    {
        return board[x, y] == CellState.Empty;
    }

    // Sets GameBoard cell at x, y to blocked
    public void SetCellBlocked(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            board[x, y] = CellState.Blocked;
        }
    }

    // Sets GameBoard cell at x, y to empty
    public void SetCellEmpty(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            board[x, y] = CellState.Empty;
        }
    }

    // Returns true if grid coordinates x, y are indexable
    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}



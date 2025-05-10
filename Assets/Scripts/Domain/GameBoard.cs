public class GameBoard : GridMap
{
    private enum CellState { Empty, Blocked }
    private CellState[,] board;

    public GameBoard(int width, int height) : base(width, height)
    {
        board = new CellState[width, height];
    }

    // Returns true if board cell at x, y has state 'Blocked'
    public bool IsCellBlocked(GridCoordinate gridCoordinate)
    {
        return board[gridCoordinate.X, gridCoordinate.Y] == CellState.Blocked;
    }

    public bool IsCellBlocked(int x, int y)
    {
        return board[x, y] == CellState.Blocked;
    }

    // Returns true if board cell at x, y has state 'Empty'
    public bool IsCellEmpty(GridCoordinate gridCoordinate)
    {
        return board[gridCoordinate.X, gridCoordinate.Y] == CellState.Empty;
    }

    public bool IsCellEmpty(int x, int y)
    {
        return board[x, y] == CellState.Empty;
    }

    // Sets GameBoard cell at x, y to blocked
    public void SetCellBlocked(GridCoordinate gridCoordinate)
    {
        if (IsWithinBounds(gridCoordinate.X, gridCoordinate.Y))
        {
            board[gridCoordinate.X, gridCoordinate.Y] = CellState.Blocked;
        }
    }

    public void SetCellBlocked(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            board[x, y] = CellState.Blocked;
        }
    }

    // Sets GameBoard cell at x, y to empty
    public void SetCellEmpty(GridCoordinate gridCoordinate)
    {
        if (IsWithinBounds(gridCoordinate.X, gridCoordinate.Y))
        {
            board[gridCoordinate.X, gridCoordinate.Y] = CellState.Empty;
        }
    }
    public void SetCellEmpty(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            board[x, y] = CellState.Empty;
        }
    }
}

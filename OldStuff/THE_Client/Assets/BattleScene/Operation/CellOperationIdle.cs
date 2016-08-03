
public class CellOperationIdle : ICellOperation
{
    public CellOperationIdle()
    {
    }

    public void onCellClick(Cell cell)
    {
        BattleGlobal.SelectedCell = cell;
        CommandManager.getInstance().runCommand(BattleConsts.CMD_OnCellSelected);
        //cell.ShowOperateButton();
        //cell.SetBackgroundColor(Cell.SelectedColor);
        Chessboard.SelectedCell = cell;
    }

    public void onCellEnter(Cell cell)
    {
    }

    public void onCellExit(Cell cell)
    {

    }

    public void clear()
    {
    }
}
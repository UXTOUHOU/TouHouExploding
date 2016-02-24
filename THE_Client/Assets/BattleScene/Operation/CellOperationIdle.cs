
public class CellOperationIdle : ICellOperation
{
    public CellOperationIdle()
    {
    }

    public void onCellClickHandler(Cell cell)
    {
        BattleGlobal.SelectCell = cell;
        CommandManager.getInstance().runCommand(CommandConsts.CommandConsts_PopUpWindow, WindowName.UnitOperationView);
        //cell.ShowOperateButton();
        //cell.SetBackgroundColor(Cell.SelectedColor);
        Chessboard.SelectedCell = cell;
    }

    public void onCellEnterHandler(Cell cell)
    {
    }

    public void onCellExitHandler(Cell cell)
    {

    }
}
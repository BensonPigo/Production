namespace Sci.Production.Prg.Entity
{
    /// <inheritdoc/>
#pragma warning disable SA1602
    public enum WHTableName
    {
        Receiving_Detail,
        TransferIn_Detail,
        IssueReturn_Detail,
        ReturnReceipt_Detail,
        Issue_Detail,
        IssueLack_Detail,
        TransferOut_Detail,
        SubTransfer_Detail,
        BorrowBack_Detail,
        Adjust_Detail,
        RemoveC_Detail, // 只有 Vstrong_AutoWHAccessory 轉出使用
        Stocktaking_Detail,
        LocationTrans_Detail,
        DefaultError,
    }
#pragma warning restore SA1602
}

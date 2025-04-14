namespace PowerBI.Daily.PowerBI.Model
{
    /// <inheritdoc/>
    public class Machine_R01_Report
    {
        /// <inheritdoc/>
        public string M { get; set; }

        /// <inheritdoc/>
        public string Location { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string Machine { get; set; }

        /// <inheritdoc/>
        public string MachineGroup { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string BrandName { get; set; }

        /// <inheritdoc/>
        public string Model { get; set; }

        /// <inheritdoc/>
        public string SerialNo { get; set; }

        /// <inheritdoc/>
        public string Condition { get; set; }

        /// <inheritdoc/>
        public string PendingCountry { get; set; }

        /// <inheritdoc/>
        public string RepairStartDate { get; set; }

        /// <inheritdoc/>
        public string EstFinishRepairDate { get; set; }

        /// <inheritdoc/>
        public string MachineArrivalDate { get; set; }

        /// <inheritdoc/>
        public string TransferDate { get; set; }

        /// <inheritdoc/>
        public string LendTo { get; set; }

        /// <inheritdoc/>
        public string LendDate { get; set; }

        /// <inheritdoc/>
        public string LastEstReturnDate { get; set; }

        /// <inheritdoc/>
        public string Remark { get; set; }

        /// <inheritdoc/>
        public string FA { get; set; }

        /// <inheritdoc/>
        public string Junk { get; set; }

        /// <inheritdoc/>
        public string PO { get; set; }

        /// <inheritdoc/>
        public string Ref { get; set; }

        /// <inheritdoc/>
        public string YYYYMM { get; set; }
    }

#pragma warning disable
    /// <inheritdoc/>
    public class Machine_R01
    {
        /// <inheritdoc/>
        public string StartMachineID { get; set; }

        /// <inheritdoc/>
        public string EndMachineID { get; set; }

        /// <inheritdoc/>
        public string MachineBrandID { get; set; }

        /// <inheritdoc/>
        public string Model { get; set; }

        /// <inheritdoc/>
        public string MachineGroup { get; set; }

        /// <inheritdoc/>
        public string StartSerial { get; set; }

        /// <inheritdoc/>
        public string EndSerial { get; set; }

        /// <inheritdoc/>
        public string LocationM { get; set; }

        /// <inheritdoc/>
        public string StartMachineArrivalDate { get; set; }

        /// <inheritdoc/>
        public string EndMachineArrivalDate { get; set; }

        /// <inheritdoc/>
        public string Condition { get; set; }

        /// <inheritdoc/>
        public bool ExcludeDisposedData { get; set; }

        /// <inheritdoc/>
        public bool IncludeCancelData { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }

        /// <inheritdoc/>
        public bool IsTPE_BI { get; set; }

        /// <inheritdoc/>
        public string SBIDate { get; set; }

        /// <inheritdoc/>
        public string EBIDate { get; set; }
    }
#pragma warning restore
}

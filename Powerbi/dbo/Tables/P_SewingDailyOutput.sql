CREATE TABLE [dbo].[P_SewingDailyOutput] (
    [Ukey]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [MDivisionID]             VARCHAR (20)    NOT NULL,
    [FactoryID]               VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Facto__4DF47A4E] DEFAULT ('') NULL,
    [ComboType]               VARCHAR (1)     DEFAULT ('') NOT NULL,
    [Category]                VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Categ__4EE89E87] DEFAULT ('') NULL,
    [CountryID]               VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Count__4FDCC2C0] DEFAULT ('') NULL,
    [OutputDate]              DATE            NULL,
    [SewingLineID]            VARCHAR (10)    CONSTRAINT [DF__P_SewingD__Sewin__50D0E6F9] DEFAULT ('') NULL,
    [Shift]                   VARCHAR (30)    CONSTRAINT [DF__P_SewingD__Shift__51C50B32] DEFAULT ('') NULL,
    [SubconOutFty]            VARCHAR (15)    CONSTRAINT [DF__P_SewingD__Subco__52B92F6B] DEFAULT ('') NULL,
    [SubConOutContractNumber] VARCHAR (50)    CONSTRAINT [DF__P_SewingD__SubCo__53AD53A4] DEFAULT ('') NULL,
    [Team]                    VARCHAR (10)    CONSTRAINT [DF__P_SewingDa__Team__54A177DD] DEFAULT ('') NULL,
    [OrderID]                 VARCHAR (13)    CONSTRAINT [DF__P_SewingD__Order__55959C16] DEFAULT ('') NULL,
    [CustPONo]                VARCHAR (30)    CONSTRAINT [DF__P_SewingD__CustP__5689C04F] DEFAULT ('') NULL,
    [BuyerDelivery]           DATE            NULL,
    [OrderQty]                INT             CONSTRAINT [DF__P_SewingD__Order__577DE488] DEFAULT ((0)) NULL,
    [BrandID]                 VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Brand__587208C1] DEFAULT ('') NULL,
    [OrderCategory]           VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Order__59662CFA] DEFAULT ('') NULL,
    [ProgramID]               VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Progr__5A5A5133] DEFAULT ('') NULL,
    [OrderTypeID]             VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Order__5B4E756C] DEFAULT ('') NULL,
    [DevSample]               VARCHAR (5)     CONSTRAINT [DF__P_SewingD__DevSa__5C4299A5] DEFAULT ('') NULL,
    [CPURate]                 NUMERIC (15, 1) CONSTRAINT [DF__P_SewingD__CPURa__5D36BDDE] DEFAULT ((0)) NULL,
    [StyleID]                 VARCHAR (20)    CONSTRAINT [DF__P_SewingD__Style__5E2AE217] DEFAULT ('') NULL,
    [Season]                  VARCHAR (10)    CONSTRAINT [DF__P_SewingD__Seaso__5F1F0650] DEFAULT ('') NULL,
    [CdCodeID]                VARCHAR (15)    CONSTRAINT [DF__P_SewingD__CdCod__60132A89] DEFAULT ('') NULL,
    [ActualManpower]          NUMERIC (12, 1) CONSTRAINT [DF__P_SewingD__Actua__61074EC2] DEFAULT ((0)) NULL,
    [NoOfHours]               NUMERIC (12, 3) CONSTRAINT [DF__P_SewingD__NoOfH__61FB72FB] DEFAULT ((0)) NULL,
    [TotalManhours]           NUMERIC (12, 3) CONSTRAINT [DF__P_SewingD__Total__62EF9734] DEFAULT ((0)) NULL,
    [TargetCPU]               NUMERIC (10, 3) CONSTRAINT [DF__P_SewingD__Targe__63E3BB6D] DEFAULT ((0)) NULL,
    [TMS]                     INT             CONSTRAINT [DF__P_SewingDai__TMS__64D7DFA6] DEFAULT ((0)) NULL,
    [CPUPrice]                NUMERIC (10, 3) CONSTRAINT [DF__P_SewingD__CPUPr__65CC03DF] DEFAULT ((0)) NULL,
    [TargetQty]               INT             CONSTRAINT [DF__P_SewingD__Targe__66C02818] DEFAULT ((0)) NULL,
    [TotalOutputQty]          INT             CONSTRAINT [DF__P_SewingD__Total__67B44C51] DEFAULT ((0)) NULL,
    [TotalCPU]                NUMERIC (10, 3) CONSTRAINT [DF__P_SewingD__Total__68A8708A] DEFAULT ((0)) NULL,
    [CPUSewerHR]              NUMERIC (10, 3) CONSTRAINT [DF__P_SewingD__CPUSe__699C94C3] DEFAULT ((0)) NULL,
    [EFF]                     NUMERIC (10, 2) CONSTRAINT [DF__P_SewingDai__EFF__6A90B8FC] DEFAULT ((0)) NULL,
    [RFT]                     NUMERIC (10, 2) CONSTRAINT [DF__P_SewingDai__RFT__6B84DD35] DEFAULT ((0)) NULL,
    [CumulateOfDays]          INT             CONSTRAINT [DF__P_SewingD__Cumul__6C79016E] DEFAULT ((0)) NULL,
    [DateRange]               VARCHAR (15)    CONSTRAINT [DF__P_SewingD__DateR__6D6D25A7] DEFAULT ('') NULL,
    [ProdOutput]              INT             CONSTRAINT [DF__P_SewingD__ProdO__6E6149E0] DEFAULT ((0)) NULL,
    [Diff]                    INT             CONSTRAINT [DF__P_SewingDa__Diff__6F556E19] DEFAULT ((0)) NULL,
    [Rate]                    NUMERIC (10, 2) CONSTRAINT [DF__P_SewingDa__Rate__70499252] DEFAULT ((0)) NULL,
    [SewingReasonDesc]        NVARCHAR (1000) CONSTRAINT [DF__P_SewingD__Sewin__7231DAC4] DEFAULT ('') NULL,
    [SciDelivery]             DATE            NULL,
    CONSTRAINT [PK_P_SewingDailyOutput] PRIMARY KEY CLUSTERED ([Ukey] ASC, [MDivisionID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SewingReasonDesc';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'比例', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'差異(QAQty-InlineQty)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Diff';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'InlineQty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ProdOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'等同於CumulateOfDays，當大於10則顯示>=10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'DateRange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該Style在這條線上累積做多久', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CumulateOfDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Right First Time(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'RFT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'效率值EFF(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'EFF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直接人員每人每小時產出(PPH)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CPUSewerHR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TotalCPU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際產出數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TotalOutputQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目標數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TargetQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每件需多少CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CPUPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Costing TMS (sec)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目標CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TargetCPU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總人力工時', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TotalManhours';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均一人工時', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'NoOfHours';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際人力', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ActualManpower';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CD#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CdCodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Season';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單CPU Rate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CPURate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'開發樣品 OrderType.IsDevSample轉入寫入 Y/N', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'DevSample';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ProgramID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderCategory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'BuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CustPONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Team';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發外條款', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SubConOutContractNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發外工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SubconOutFty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Shift';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OutputDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國家別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order or Mockup order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SewingOutput_Detail_Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'飛雁交期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SewingDailyOutput',
    @level2type = N'COLUMN',
    @level2name = N'SciDelivery'
GO
CREATE NONCLUSTERED INDEX [Index_of_P_ImportEfficiencyBI]
    ON [dbo].[P_SewingDailyOutput]([OutputDate] ASC)
    INCLUDE([MDivisionID], [FactoryID], [ComboType], [SewingLineID], [Shift], [Team], [OrderID]);


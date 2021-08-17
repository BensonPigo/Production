CREATE TABLE [dbo].[DropDownList] (
    [Type]        VARCHAR (20)   CONSTRAINT [DF_DropDownList_Type] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (50)   CONSTRAINT [DF_DropDownList_ID] DEFAULT ('') NOT NULL,
    [Name]        NVARCHAR (50)  CONSTRAINT [DF_DropDownList_Name] DEFAULT ('') NULL,
    [RealLength]  NUMERIC (2)    CONSTRAINT [DF_DropDownList_RealLength] DEFAULT ((0)) NOT NULL,
    [Description] NVARCHAR (200) CONSTRAINT [DF_DropDownList_Description] DEFAULT ('') NULL,
    [Seq]         INT            CONSTRAINT [DF_DropDownList_Seq] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_DropDownList] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DropDownList', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將getReason.prg 轉Table , ID 和Name長度需一樣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DropDownList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DropDownList', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DropDownList', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DropDownList', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際存的長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DropDownList', @level2type = N'COLUMN', @level2name = N'RealLength';


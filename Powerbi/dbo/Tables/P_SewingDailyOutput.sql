CREATE TABLE [dbo].[P_SewingDailyOutput] (
    [Ukey]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [MDivisionID]                 VARCHAR (8000)  NOT NULL,
    [FactoryID]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_FactoryID_New] DEFAULT ('') NULL,
    [ComboType]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_ComboType_New] DEFAULT ('') NOT NULL,
    [Category]                    VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Category_New] DEFAULT ('') NULL,
    [CountryID]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_CountryID_New] DEFAULT ('') NULL,
    [OutputDate]                  DATE            NULL,
    [SewingLineID]                VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_SewingLineID_New] DEFAULT ('') NULL,
    [Shift]                       VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Shift_New] DEFAULT ('') NULL,
    [SubconOutFty]                VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_SubconOutFty_New] DEFAULT ('') NULL,
    [SubConOutContractNumber]     VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_SubConOutContractNumber_New] DEFAULT ('') NULL,
    [Team]                        VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Team_New] DEFAULT ('') NULL,
    [OrderID]                     VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_OrderID_New] DEFAULT ('') NULL,
    [Article]                     VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Article_New] DEFAULT ('') NULL,
    [SizeCode]                    VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_SizeCode_New] DEFAULT ('') NULL,
    [CustPONo]                    VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_CustPONo_New] DEFAULT ('') NULL,
    [BuyerDelivery]               DATE            NULL,
    [OrderQty]                    INT             CONSTRAINT [DF_P_SewingDailyOutput_OrderQty_New] DEFAULT ((0)) NULL,
    [BrandID]                     VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_BrandID_New] DEFAULT ('') NULL,
    [OrderCategory]               VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_OrderCategory_New] DEFAULT ('') NULL,
    [ProgramID]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_ProgramID_New] DEFAULT ('') NULL,
    [OrderTypeID]                 VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_OrderTypeID_New] DEFAULT ('') NULL,
    [DevSample]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_DevSample_New] DEFAULT ('') NULL,
    [CPURate]                     NUMERIC (38, 1) CONSTRAINT [DF_P_SewingDailyOutput_CPURate_New] DEFAULT ((0)) NULL,
    [StyleID]                     VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_StyleID_New] DEFAULT ('') NULL,
    [Season]                      VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Season_New] DEFAULT ('') NULL,
    [CdCodeID]                    VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_CdCodeID_New] DEFAULT ('') NULL,
    [ActualManpower]              NUMERIC (38, 1) CONSTRAINT [DF_P_SewingDailyOutput_ActualManpower_New] DEFAULT ((0)) NULL,
    [NoOfHours]                   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_NoOfHours_New] DEFAULT ((0)) NULL,
    [TotalManhours]               NUMERIC (38, 3) CONSTRAINT [DF_P_SewingDailyOutput_TotalManhours_New] DEFAULT ((0)) NULL,
    [TargetCPU]                   NUMERIC (38, 3) CONSTRAINT [DF_P_SewingDailyOutput_TargetCPU_New] DEFAULT ((0)) NULL,
    [TMS]                         INT             CONSTRAINT [DF_P_SewingDailyOutput_TMS_New] DEFAULT ((0)) NULL,
    [CPUPrice]                    NUMERIC (38, 3) CONSTRAINT [DF_P_SewingDailyOutput_CPUPrice_New] DEFAULT ((0)) NULL,
    [TargetQty]                   INT             CONSTRAINT [DF_P_SewingDailyOutput_TargetQty_New] DEFAULT ((0)) NULL,
    [TotalOutputQty]              INT             CONSTRAINT [DF_P_SewingDailyOutput_TotalOutputQty_New] DEFAULT ((0)) NULL,
    [TotalCPU]                    NUMERIC (38, 3) CONSTRAINT [DF_P_SewingDailyOutput_TotalCPU_New] DEFAULT ((0)) NULL,
    [CPUSewerHR]                  NUMERIC (38, 3) CONSTRAINT [DF_P_SewingDailyOutput_CPUSewerHR_New] DEFAULT ((0)) NULL,
    [EFF]                         NUMERIC (38, 2) CONSTRAINT [DF_P_SewingDailyOutput_EFF_New] DEFAULT ((0)) NULL,
    [RFT]                         NUMERIC (38, 2) CONSTRAINT [DF_P_SewingDailyOutput_RFT_New] DEFAULT ((0)) NULL,
    [CumulateOfDays]              INT             CONSTRAINT [DF_P_SewingDailyOutput_CumulateOfDays_New] DEFAULT ((0)) NULL,
    [DateRange]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_DateRange_New] DEFAULT ('') NULL,
    [ProdOutput]                  INT             CONSTRAINT [DF_P_SewingDailyOutput_ProdOutput_New] DEFAULT ((0)) NULL,
    [Diff]                        INT             CONSTRAINT [DF_P_SewingDailyOutput_Diff_New] DEFAULT ((0)) NULL,
    [Rate]                        NUMERIC (38, 2) CONSTRAINT [DF_P_SewingDailyOutput_Rate_New] DEFAULT ((0)) NULL,
    [SewingReasonDesc]            NVARCHAR (1000) CONSTRAINT [DF_P_SewingDailyOutput_SewingReasonDesc_New] DEFAULT ('') NULL,
    [SciDelivery]                 DATE            NULL,
    [CDCodeNew]                   VARCHAR (8000)  NULL,
    [ProductType]                 NVARCHAR (1000) NULL,
    [FabricType]                  NVARCHAR (1000) NULL,
    [Lining]                      VARCHAR (8000)  NULL,
    [Gender]                      VARCHAR (8000)  NULL,
    [Construction]                NVARCHAR (1000) NULL,
    [LockStatus]                  VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_LockStatus_New] DEFAULT ('') NOT NULL,
    [Cancel]                      VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Cancel_New] DEFAULT ('') NOT NULL,
    [Remark]                      VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Remark_New] DEFAULT ('') NOT NULL,
    [SPFactory]                   VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_SPFactory_New] DEFAULT ('') NOT NULL,
    [NonRevenue]                  VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_NonRevenue_New] DEFAULT ('') NOT NULL,
    [AT_HAND_TMS]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_AT_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [AT_HAND_CPU]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_AT_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_AT_HAND_TMS]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_AT_HAND_CPU]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [AT_MACHINE_TMS]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_AT_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [AT_MACHINE_CPU]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_AT_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_AT_MACHINE_TMS]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_AT_MACHINE_CPU]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_AT_CPU]                  NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_CPU_New] DEFAULT ((0)) NOT NULL,
    [BONDING_HAND_TMS]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_BONDING_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [BONDING_HAND_CPU]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_BONDING_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_BONDING_HAND_TMS]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_BONDING_HAND_CPU]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [BONDING_MACHINE_TMS]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_BONDING_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [BONDING_MACHINE_CPU]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_BONDING_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_BONDING_MACHINE_TMS]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_BONDING_MACHINE_CPU]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [CARTON_Price]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_CARTON_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_CARTON_Price]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_CARTON_Price_New] DEFAULT ((0)) NOT NULL,
    [CUTTING_TMS]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_CUTTING_TMS_New] DEFAULT ((0)) NOT NULL,
    [CUTTING_CPU]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_CUTTING_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_CUTTING_TMS]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_CUTTING_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_CUTTING_CPU]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_CUTTING_CPU_New] DEFAULT ((0)) NOT NULL,
    [DIE_CUT_TMS]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_DIE_CUT_TMS_New] DEFAULT ((0)) NOT NULL,
    [DIE_CUT_CPU]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_DIE_CUT_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_DIE_CUT_TMS]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_DIE_CUT_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_DIE_CUT_CPU]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_DIE_CUT_CPU_New] DEFAULT ((0)) NOT NULL,
    [DOWN_TMS]                    NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_DOWN_TMS_New] DEFAULT ((0)) NOT NULL,
    [DOWN_CPU]                    NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_DOWN_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_DOWN_TMS]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_DOWN_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_DOWN_CPU]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_DOWN_CPU_New] DEFAULT ((0)) NOT NULL,
    [EM_DEBOSS_I_H_TMS]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EM_DEBOSS_I_H_TMS_New] DEFAULT ((0)) NOT NULL,
    [EM_DEBOSS_I_H_CPU]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EM_DEBOSS_I_H_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_EM_DEBOSS_I_H_TMS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EM_DEBOSS_I_H_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_EM_DEBOSS_I_H_CPU]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EM_DEBOSS_I_H_CPU_New] DEFAULT ((0)) NOT NULL,
    [EM_DEBOSS_MOLD_Price]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EM_DEBOSS_MOLD_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_EM_DEBOSS_MOLD_Price]    NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EM_DEBOSS_MOLD_Price_New] DEFAULT ((0)) NOT NULL,
    [EMB_THREAD]                  NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EMB_THREAD_New] DEFAULT ((0)) NOT NULL,
    [TTL_EMB_THREAD]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMB_THREAD_New] DEFAULT ((0)) NOT NULL,
    [EMBOSS_DEBOSS_PCS]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EMBOSS_DEBOSS_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_EMBOSS_DEBOSS_PCS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBOSS_DEBOSS_PCS_New] DEFAULT ((0)) NOT NULL,
    [EMBOSS_DEBOSS_Price]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EMBOSS_DEBOSS_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_EMBOSS_DEBOSS_Price]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBOSS_DEBOSS_Price_New] DEFAULT ((0)) NOT NULL,
    [EMBROIDERY_Price]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EMBROIDERY_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_EMBROIDERY_Price]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBROIDERY_Price_New] DEFAULT ((0)) NOT NULL,
    [EMBROIDERY_STITCH]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_EMBROIDERY_STITCH_New] DEFAULT ((0)) NOT NULL,
    [TTL_EMBROIDERY_STITCH]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBROIDERY_STITCH_New] DEFAULT ((0)) NOT NULL,
    [FARM_OUT_QUILTING_PCS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_FARM_OUT_QUILTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_FARM_OUT_QUILTING_PCS]   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_FARM_OUT_QUILTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [FARM_OUT_QUILTING_Price]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_FARM_OUT_QUILTING_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_FARM_OUT_QUILTING_Price] NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_FARM_OUT_QUILTING_Price_New] DEFAULT ((0)) NOT NULL,
    [Garment_Dye_PCS]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_Garment_Dye_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_Garment_Dye_PCS]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_Garment_Dye_PCS_New] DEFAULT ((0)) NOT NULL,
    [Garment_Dye_Price]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_Garment_Dye_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_Garment_Dye_Price]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_Garment_Dye_Price_New] DEFAULT ((0)) NOT NULL,
    [GLUE_BO_HAND_TMS]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [GLUE_BO_HAND_CPU]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_GLUE_BO_HAND_TMS]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_GLUE_BO_HAND_CPU]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [GLUE_BO_MACHINE_TMS]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [GLUE_BO_MACHINE_CPU]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_GLUE_BO_MACHINE_TMS]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_GLUE_BO_MACHINE_CPU]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [GMT_DRY_PCS]                 NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GMT_DRY_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_GMT_DRY_PCS]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_DRY_PCS_New] DEFAULT ((0)) NOT NULL,
    [GMT_DRY_Price]               NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GMT_DRY_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_GMT_DRY_Price]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_DRY_Price_New] DEFAULT ((0)) NOT NULL,
    [GMT_WASH_PCS]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GMT_WASH_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_GMT_WASH_PCS]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_WASH_PCS_New] DEFAULT ((0)) NOT NULL,
    [GMT_WASH_Price]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_GMT_WASH_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_GMT_WASH_Price]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_WASH_Price_New] DEFAULT ((0)) NOT NULL,
    [HEAT_SET_PLEAT_PCS]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HEAT_SET_PLEAT_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_HEAT_SET_PLEAT_PCS]      NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_SET_PLEAT_PCS_New] DEFAULT ((0)) NOT NULL,
    [HEAT_SET_PLEAT_Price]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HEAT_SET_PLEAT_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_HEAT_SET_PLEAT_Price]    NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_SET_PLEAT_Price_New] DEFAULT ((0)) NOT NULL,
    [HEAT_TRANSFER_PANEL]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HEAT_TRANSFER_PANEL_New] DEFAULT ((0)) NOT NULL,
    [TTL_HEAT_TRANSFER_PANEL]     NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_TRANSFER_PANEL_New] DEFAULT ((0)) NOT NULL,
    [HEAT_TRANSFER_TMS]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HEAT_TRANSFER_TMS_New] DEFAULT ((0)) NOT NULL,
    [HEAT_TRANSFER_CPU]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HEAT_TRANSFER_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_HEAT_TRANSFER_TMS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_TRANSFER_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_HEAT_TRANSFER_CPU]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_TRANSFER_CPU_New] DEFAULT ((0)) NOT NULL,
    [HF_WELDED_PCS]               NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HF_WELDED_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_HF_WELDED_PCS]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HF_WELDED_PCS_New] DEFAULT ((0)) NOT NULL,
    [HF_WELDED_Price]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_HF_WELDED_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_HF_WELDED_Price]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_HF_WELDED_Price_New] DEFAULT ((0)) NOT NULL,
    [INDIRECT_MANPOWER_TMS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_INDIRECT_MANPOWER_TMS_New] DEFAULT ((0)) NOT NULL,
    [INDIRECT_MANPOWER_CPU]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_INDIRECT_MANPOWER_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_INDIRECT_MANPOWER_TMS]   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_INDIRECT_MANPOWER_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_INDIRECT_MANPOWER_CPU]   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_INDIRECT_MANPOWER_CPU_New] DEFAULT ((0)) NOT NULL,
    [INSPECTION_TMS]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_INSPECTION_TMS_New] DEFAULT ((0)) NOT NULL,
    [INSPECTION_CPU]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_INSPECTION_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_INSPECTION_TMS]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_INSPECTION_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_INSPECTION_CPU]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_INSPECTION_CPU_New] DEFAULT ((0)) NOT NULL,
    [JOKERTAG_TMS]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_JOKERTAG_TMS_New] DEFAULT ((0)) NOT NULL,
    [JOKERTAG_CPU]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_JOKERTAG_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_JOKERTAG_TMS]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_JOKERTAG_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_JOKERTAG_CPU]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_JOKERTAG_CPU_New] DEFAULT ((0)) NOT NULL,
    [LASER_TMS]                   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_LASER_TMS_New] DEFAULT ((0)) NOT NULL,
    [LASER_CPU]                   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_LASER_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_LASER_TMS]               NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_LASER_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_LASER_CPU]               NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_LASER_CPU_New] DEFAULT ((0)) NOT NULL,
    [PAD_PRINTING_PCS]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PAD_PRINTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_PAD_PRINTING_PCS]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PAD_PRINTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [PAD_PRINTING_Price]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PAD_PRINTING_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_PAD_PRINTING_Price]      NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PAD_PRINTING_Price_New] DEFAULT ((0)) NOT NULL,
    [POLYBAG_Price]               NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_POLYBAG_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_POLYBAG_Price]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_POLYBAG_Price_New] DEFAULT ((0)) NOT NULL,
    [PRINTING_PCS]                NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PRINTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [TTL_PRINTING_PCS]            NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PRINTING_PCS_New] DEFAULT ((0)) NOT NULL,
    [PRINTING_Price]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PRINTING_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_PRINTING_Price]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PRINTING_Price_New] DEFAULT ((0)) NOT NULL,
    [SP_THREAD_Price]             NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_SP_THREAD_Price_New] DEFAULT ((0)) NOT NULL,
    [TTL_SP_THREAD_Price]         NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_SP_THREAD_Price_New] DEFAULT ((0)) NOT NULL,
    [SUBLIMATION_PRINT_TMS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_PRINT_TMS_New] DEFAULT ((0)) NOT NULL,
    [SUBLIMATION_PRINT_CPU]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_PRINT_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_SUBLIMATION_PRINT_TMS]   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_PRINT_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_SUBLIMATION_PRINT_CPU]   NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_PRINT_CPU_New] DEFAULT ((0)) NOT NULL,
    [SUBLIMATION_ROLLER_TMS]      NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_ROLLER_TMS_New] DEFAULT ((0)) NOT NULL,
    [SUBLIMATION_ROLLER_CPU]      NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_ROLLER_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_SUBLIMATION_ROLLER_TMS]  NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_ROLLER_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_SUBLIMATION_ROLLER_CPU]  NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_ROLLER_CPU_New] DEFAULT ((0)) NOT NULL,
    [Inline_Category]             NVARCHAR (1000) CONSTRAINT [DF_P_SewingDailyOutput_Inline_Inline_Category_New] DEFAULT ('') NOT NULL,
    [Low_output_Reason]           NVARCHAR (1000) CONSTRAINT [DF_P_SewingDailyOutput_Inline_Low_output_Reason_New] DEFAULT ('') NOT NULL,
    [New_Style_Repeat_style]      VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_Inline_New_Style_Repeat_style_New] DEFAULT ('') NOT NULL,
    [ArtworkType]                 VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_ArtworkType_New] DEFAULT ('') NOT NULL,
    [PLEAT_HAND_CPU]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PLEAT_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [PLEAT_HAND_TMS]              NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PLEAT_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [PLEAT_MACHINE_CPU]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PLEAT_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [PLEAT_MACHINE_TMS]           NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_PLEAT_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_PLEAT_HAND_CPU]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PLEAT_HAND_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_PLEAT_HAND_TMS]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PLEAT_HAND_TMS_New] DEFAULT ((0)) NOT NULL,
    [TTL_PLEAT_MACHINE_CPU]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PLEAT_MACHINE_CPU_New] DEFAULT ((0)) NOT NULL,
    [TTL_PLEAT_MACHINE_TMS]       NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutput_TTL_PLEAT_MACHINE_TMS_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]                 VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                DATETIME        NULL,
    [BIStatus]                    VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutput_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SewingDailyOutput] PRIMARY KEY CLUSTERED ([Ukey] ASC, [MDivisionID] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput_Detail_Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�u�t�O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�զX���A', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ComboType';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order or Mockup order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��a�O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CountryID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���X��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OutputDate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���u�N��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Z�O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Shift';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�o�~�u�t', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SubconOutFty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�o�~����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SubConOutContractNumber';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�էO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Team';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q��s��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�C���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Article';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ؤo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ȥ�q��渹', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CustPONo';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ȥ���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'BuyerDelivery';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q��ƶq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�~�P�O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'BrandID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q�����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderCategory';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ȥ�~�P', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ProgramID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q�����O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'OrderTypeID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�}�o�˫~ OrderType.IsDevSample��J�g�J Y/N', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'DevSample';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q��CPU Rate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CPURate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ڦ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'StyleID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�u�`', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Season';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CdCodeID'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��ڤH�O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'ActualManpower';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�����@�H�u��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'NoOfHours';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�`�H�O�u��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TotalManhours';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ؼ�CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TargetCPU';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costing TMS (sec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�C��ݦh��CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CPUPrice';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ؼмƶq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TargetQty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��ڲ��X�ƶq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TotalOutputQty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'TotalCPU';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�����H���C�H�C�p�ɲ��X(PPH)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CPUSewerHR';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ĳv��EFF(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'EFF';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Right First Time(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'RFT'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��Style�b�o���u�W�ֿn���h�[', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'CumulateOfDays';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���P��CumulateOfDays�A��j��10�h���>=10', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'DateRange';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ProdOutput'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�t��(QAQty-InlineQty)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Diff';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'Rate';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��]�y�z', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SewingReasonDesc';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'SciDelivery';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出的Lock狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'LockStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為訂單公司別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutput', @level2type = N'COLUMN', @level2name = N'BIStatus';


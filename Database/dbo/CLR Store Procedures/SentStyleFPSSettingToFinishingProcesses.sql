﻿create procedure [SentStyleFPSSettingToFinishingProcesses](@xmlPar nvarchar(max))
AS external name SqlCallWebAPI.Sunrise_FinishingProcesses.SentStyleFPSSettingToFinishingProcesses;
go
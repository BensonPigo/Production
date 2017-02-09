CREATE Function [dbo].[GetOutputInformation]
(
	  @orderid varchar(13)			--
	 ,@outputdate date			--
	 
)
Returns TABLE
AS
RETURN 
(
    Select Max(s.OutputDate) as MaxOutputDate, sd.QAQty*(isnull((select Rate from Style_Location sl, Orders o where o.ID = sd.OrderID and o.StyleUkey = sl.StyleUkey and sl.location = sd.ComboType),0)/100) as OutputAmount
	From SewingOutput s, SewingOutput_Detail sd
	Where s.ID = sd.ID
	And sd.OrderID = @orderid
	And s.OutputDate <= @outputdate
	GROUP BY sd.OrderId,sd.ComboType,sd.QAQty
);
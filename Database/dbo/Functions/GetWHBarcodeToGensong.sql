

Create Function [dbo].[GetWHBarcodeToGensong]
	(
	  @Function			VarChar(4)
	 ,@TransactionID	VarChar(13)
	 ,@TransactionUkey	Bigint
	 ,@Action			VarChar(10) -- PMS's Action:  Confirm, Unconfirm
	 ,@ForT				VarChar(1)
	 ,@FtyInventoryUkey bigint = 0 -- for Old Datas
	 ,@OriBlanceQty		numeric(20,2) = 0.0 --only for Adjust
	 ,@FromNewBarcode	bit = 0
     ,@Qty              numeric(20,2) = 0.0
	)
Returns VarChar(255)
As
Begin
	declare @Barcode varchar(255)
	select
		@Barcode = 
			Case @ForT when 'F'
			then
			(
				case
				when @Function in('P10','P11','P12','P13','P33','P62'--Issue_Detail
									,'P15','P16'--IssueLack_Detail
									,'P19'--TransferOut_Detail
									,'P22','P23','P24','P25','P36'--SubTransfer_Detail
									,'P31','P32'--BorrowBack_Detail
									,'P37')-- ReturnReceipt_Detail
				then
                    case when @Qty >= 0
                    then (case when w.Action = 'Confirm' and @FromNewBarcode = 0 
					    then concat(w.From_OldBarcode, iif(w.From_OldBarcodeSeq='', '', '-'+w.From_OldBarcodeSeq))
					    else concat(w.From_NewBarcode, iif(w.From_NewBarcodeSeq='', '', '-'+w.From_NewBarcodeSeq))
					    end)
                    else (case when w.Action = 'Confirm' and @FromNewBarcode = 0 
					    then concat(w.From_NewBarcode, iif(w.From_NewBarcodeSeq='', '', '-'+w.From_NewBarcodeSeq))
					    else concat(w.From_OldBarcode, iif(w.From_OldBarcodeSeq='', '', '-'+w.From_OldBarcodeSeq))
					    end)end
				when @Function in('P07','P08'--Receiving_Detail
									,'P17'--IssueReturn_Detail
									,'P18')--TransferIn_Detail
				then
					(case w.Action when 'Confirm'
					then concat(w.To_NewBarcode, iif(w.To_NewBarcodeSeq = '', '', '-' + w.To_NewBarcodeSeq))
					else concat(w.To_OldBarcode, iif(w.To_OldBarcodeSeq = '', '', '-' + w.To_OldBarcodeSeq))
					end)
				when @Function in('P34','P35','P43','P45','P48')-- Adjust_Detail
				then
					(case when @OriBlanceQty > 0.0 and @FromNewBarcode = 0
					then concat(w.From_OldBarcode, iif(w.From_OldBarcodeSeq='', '', '-'+w.From_OldBarcodeSeq))
					else concat(w.From_NewBarcode, iif(w.From_NewBarcodeSeq='', '', '-'+w.From_NewBarcodeSeq))
					end)
				end
			)
			else -- @ForT = 'T'
			(
				case
				when @Function in('P10','P11','P12','P13','P33','P62'--Issue_Detail
									,'P15','P16'--IssueLack_Detail
									,'P19'--TransferOut_Detail
									,'P22','P23','P24','P25','P36'--SubTransfer_Detail
									,'P31','P32')--BorrowBack_Detail
				then
                    case when @Qty >= 0
                    then (case w.Action when 'Confirm'
					    then concat(w.To_NewBarcode, iif(w.To_NewBarcodeSeq = '', '', '-' + w.To_NewBarcodeSeq))
					    else concat(w.To_OldBarcode, iif(w.To_OldBarcodeSeq = '', '', '-' + w.To_OldBarcodeSeq))
					    end)
                    else (case w.Action when 'Confirm'
					    then concat(w.To_OldBarcode, iif(w.To_OldBarcodeSeq = '', '', '-' + w.To_OldBarcodeSeq))
					    else concat(w.To_NewBarcode, iif(w.To_NewBarcodeSeq = '', '', '-' + w.To_NewBarcodeSeq))
					    end)
                    end
				else ''
				end
			)
			End
	from WHBarcodeTransaction w with(nolock)    
	where w.[Function] = @Function
	and w.TransactionID = @TransactionID
	and w.TransactionUkey = @TransactionUkey
	and w.Action = @Action

	--舊資料處理
	if isnull(@Barcode, '') = ''
	begin	
		select @Barcode = fb.Barcode
		from FtyInventory_Barcode fb with(nolock)
		where fb.TransactionID = @TransactionID
		and fb.Ukey = @FtyInventoryUkey

		if @ForT = 'F' and @Function in('P10','P11','P12','P13','P33','P62'--Issue_Detail
									,'P15','P16'--IssueLack_Detail
									,'P19'--TransferOut_Detail
									,'P22','P23','P24','P25','P36'--SubTransfer_Detail
									,'P31','P32')--BorrowBack_Detail
			select @Barcode = min(fb.Barcode)
			from FtyInventory_Barcode fb with(nolock)
			where fb.Ukey = @FtyInventoryUkey
	end

	Return @Barcode
End
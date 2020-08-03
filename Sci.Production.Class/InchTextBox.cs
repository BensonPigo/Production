using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Sci.Production.Class
{
    /// <summary>
    /// 可以設定輸入模式為Inch或是CM的文字方塊，請使用InputType來確定輸入的驗證行為
    /// </summary>
    public class InchTextBox : Sci.Win.UI.TextBox
    {
        private UnitTypeEnum? _InputType;

        /// <summary>
        /// 是否要做額外的Validation(事先定義好的)
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("決定要以哪一種方式來呈現值以及驗證輸入")]
        public UnitTypeEnum? InputType
        {
            get
            {
                return this._InputType;
            }

            set
            {
                this._InputType = value;
                switch (value.GetValueOrDefault(UnitTypeEnum.InchText))
                {
                    case UnitTypeEnum.InchText:
                        this.Text = this.InchValue.ToInchText();
                        break;
                    case UnitTypeEnum.InchDecimal:
                        this.Text = this.InchValue.ToInchDecimal().ToString(this.FloatPartFormat ?? "0.00");
                        break;
                    case UnitTypeEnum.CM:
                        this.Text = this.InchValue.ToCmDecimal().ToString(this.FloatPartFormat ?? "0.00");
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 浮點數的Inch值
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("浮點數的Inch值")]
        public Inch InchValue { get; set; }

        /// <summary>
        /// 當使用Cm或是InchDecimal為InputType的時候，會使用這個屬性來Format結果字串
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("浮點數的位數")]
        public string FloatPartFormat { get; set; }

        /// <summary>
        /// 目前控制項，以CM為單位來呈現的值(及時換算自InchValue)
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("浮點數的CM值")]
        public string CMValue
        {
            get
            {
                if (this.InchValue.Piece.HasValue == false)
                {
                    return null;
                }
                else
                {
                    return this.InchValue.ToCmDecimal().ToString(this.FloatPartFormat ?? "0.000");
                }
            }
        }

        /// <summary>
        /// 目前控制項，以Inch為單位來呈現的值
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("把核心值轉換為Inch單位的數值字串")]
        public string InchDecimal
        {
            get
            {
                return this.InchValue.Piece.HasValue == false ?
                    string.Empty :
                    this.InchValue.ToInchDecimal().ToString(this.FloatPartFormat ?? "0.000");
            }
        }

        /// <summary>
        /// 把核心值轉換為Inch單位的文字表示式
        /// </summary>
        [Category("SCIC-TextBoxUnitRestriction")]
        [Description("把核心值轉換為Inch單位的文字表示式")]
        public string InchText
        {
            get
            {
                return this.InchValue.Piece.HasValue == false ?
                    string.Empty :
                    this.InchValue.ToInchText();
            }
        }

        /// <summary>
        /// 可以設定輸入模式為Inch或是CM的文字方塊，請使用InputType來確定輸入的驗證行為
        /// </summary>
        public InchTextBox()
        {
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            int v;
            if (this.InputType.GetValueOrDefault(UnitTypeEnum.InchText) == UnitTypeEnum.InchText && int.TryParse(this.Text, out v) == true && v.ToString() == this.Text)
            {
                this.Text += "\"";
            }

            switch (this.InputType.GetValueOrDefault(UnitTypeEnum.InchText))
            {
                case UnitTypeEnum.InchText:
                    if (this.TryValidateForInchText(this.Text) == false)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("invalid value");
                    }

                    this.Text = this.InchValue.ToInchText();
                    break;
                case UnitTypeEnum.InchDecimal:
                    if (this.TryValidateForInchDecimal(this.Text) == false)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("invalid value");
                    }

                    this.Text = this.InchValue.ToInchDecimal().ToString(this.FloatPartFormat ?? "0.000");
                    break;
                case UnitTypeEnum.CM:
                    if (this.TryValidateForCm(this.Text) == false)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("invalid value");
                    }

                    this.Text = this.InchValue.ToCmDecimal().ToString(this.FloatPartFormat ?? "0.000");
                    break;
                default:
                    throw new NotImplementedException();
            }

            // 如果上面已經驗證失敗，就不用做後續驗證了喔
            if (e.Cancel == false)
            {
                base.OnValidating(e);
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.Handled == true)
            {
                return;
            }

            if ((Keys)e.KeyChar == Keys.Back)
            {
                return;
            }

            if ((Keys)e.KeyChar == Keys.Enter)
            {
                return;
            }

            var allowedCharacters = "0123456789".IndexOf(e.KeyChar.ToString()) >= 0;
            if (allowedCharacters == false)
            {
                var inputType = this.InputType.GetValueOrDefault(UnitTypeEnum.InchText);
                switch (inputType)
                {
                    case UnitTypeEnum.InchText:
                        if ("/\"".IndexOf(e.KeyChar.ToString()) < 0)
                        {
                            e.Handled = true;
                        }

                        break;
                    case UnitTypeEnum.InchDecimal:
                    case UnitTypeEnum.CM:
                        if (e.KeyChar.ToString() != ".")
                        {
                            e.Handled = true;
                        }

                        break;
                }
            }

            base.OnKeyPress(e);
        }

        /// <summary>
        /// 以Inch的格式，驗證輸入值是否合於格式，若否則return false，若是，會在return true之前，把CmValue和InchValue放入驗證後的結果值
        /// </summary>
        /// <inheritdoc/>
        private bool TryValidateForInchText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                this.InchValue = default(Inch);
            }
            else
            {
                var newInchPiece = Inch.FromInchText(value);
                if (newInchPiece.Piece == null)
                {
                    return false;
                }

                this.InchValue = newInchPiece;
            }

            return true;
        }

        /// <inheritdoc/>
        private bool TryValidateForInchDecimal(string value)
        {
            decimal v;
            if (string.IsNullOrWhiteSpace(value))
            {
                this.InchValue = default(Inch);
            }
            else if (decimal.TryParse(value, out v) == false)
            {
                return false;
            }
            else
            {
                var newInchPiece = Inch.FromInchDecimal(v);
                if (newInchPiece.Piece == null)
                {
                    return false;
                }

                this.InchValue = newInchPiece;
            }

            return true;
        }

        /// <inheritdoc/>
        private bool TryValidateForCm(string value)
        {
            decimal v;
            if (string.IsNullOrWhiteSpace(value))
            {
                this.InchValue = default(Inch);
            }
            else if (decimal.TryParse(value, out v) == false)
            {
                return false;
            }
            else
            {
                var newInchPiece = Inch.FromCmDecimal(v);
                if (newInchPiece.Piece.HasValue == false)
                {
                    return false;
                }

                this.InchValue = newInchPiece;
            }

            return true;
        }
    }

    /// <summary>
    /// 是否要做額外的Validation
    /// </summary>
    public enum UnitTypeEnum
    {
        /// <summary>
        /// Inch以文字的型態
        /// </summary>
        InchText,

        /// <summary>
        /// Inch以數值的型態
        /// </summary>
        InchDecimal,

        /// <summary>
        /// CM
        /// </summary>
        CM
    }

    /// <summary>
    /// 代表Inch的值，提供轉換(To/From) InchText 與 CM 的功能
    /// </summary>
    public struct Inch
    {
        private const decimal TransRate = 2.54m;
        private const decimal InchPieceUnit = 0.0625m;  // 1m / 16m

        /// <inheritdoc/>
        public static Inch FromCmText(string text)
        {
            decimal v;
            if (decimal.TryParse(text, out v) == true)
            {
                return FromCmDecimal(v);
            }
            else
            {
                return default(Inch);
            }
        }

        /// <inheritdoc/>
        public static Inch FromCmDecimal(decimal? cmValue)
        {
            if (cmValue.HasValue == false)
            {
                return default(Inch);
            }
            else
            {
                return new Inch() { Value = cmValue / TransRate, CmValue = cmValue };
            }
        }

        /// <inheritdoc/>
        public static Inch FromInchDecimal(decimal? inchValue)
        {
            return new Inch() { Value = inchValue };
        }

        /// <inheritdoc/>
        public static Inch FromInchText(string inchText)
        {
            return new Inch() { Piece = TextToPiece(inchText) };
        }

        private static int? DecimalToPiece(decimal? value)
        {
            if (value.HasValue == false)
            {
                return null;
            }

            var intPart = (int)(value.Value / 1);
            var floatPart = value.Value - intPart;

            // 1/16 -> 1/8
            var pieceOfFloat = floatPart > ((InchPieceUnit * 2) * 7) ?
                16 :
                2 * Enumerable.Range(1, 7)
                    .FirstOrDefault(idx =>
                        floatPart > ((idx - 1) * (InchPieceUnit * 2)) &&
                        floatPart <= (idx * (InchPieceUnit * 2)));

            return (intPart * 16) + pieceOfFloat;
        }

        private static decimal? PieceToDecimal(decimal? piece)
        {
            if (piece.HasValue == false)
            {
                return null;
            }
            else
            {
                return piece.Value * InchPieceUnit;
            }
        }

        private static string PieceToText(decimal piece)
        {
            if (piece < 0)
            {
                return "0";
            }

            if (piece >= 16)
            {
                var intPart = (int)(piece / 16);
                var floatPart = piece % 16;
                if (intPart != 0 && floatPart != 0)
                {
                    return string.Format("{0}{1}", (int)(piece / 16), PieceToText(piece % 16));
                }
                else if (floatPart == 0)
                {
                    return string.Format("{0}\"", intPart);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                switch (Convert.ToInt32(piece))
                {
                    case 0:
                        return string.Empty;
                    case 1:
                        return "\"1/16";
                    case 2:
                        return "\"1/8";
                    case 3:
                        return "\"3/16";
                    case 4:
                        return "\"1/4";
                    case 5:
                        return "\"5/16";
                    case 6:
                        return "\"3/8";
                    case 7:
                        return "\"7/16";
                    case 8:
                        return "\"1/2";
                    case 9:
                        return "\"9/16";
                    case 10:
                        return "\"5/8";
                    case 11:
                        return "\"11/16";
                    case 12:
                        return "\"3/4";
                    case 13:
                        return "\"13/16";
                    case 14:
                        return "\"7/8";
                    case 15:
                        return "\"15/16";
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static int? TextToPiece(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (text.IndexOf("\"") < 0)
            {
                if (text.IndexOf("/") < 0)
                {
                    text += "\"";
                }
                else
                {
                    text = "\"" + text;
                }
            }
            else if (text.Count(ch => ch == '\"') != 1)
            {
                return null;
            }

            var part = text.Split('\"');
            var intPartValue = 0;
            if (int.TryParse("0" + part[0].ToString(), out intPartValue) == false)
            {
                return null;
            }

            if (MyUtility.Check.Empty(part[1]))
            {
                return intPartValue * 16;
            }
            else
            {
                var pricing = part[1].Split("//".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                decimal pric1, pric2;
                if (decimal.TryParse(pricing[0], out pric1) == false ||
                    decimal.TryParse(pricing[1], out pric2) == false)
                {
                    return null;
                }

                var floatPiece = Convert.ToInt32(pric1 / pric2 * 16);

                return (intPartValue * 16) + floatPiece;
            }
        }

        /// <summary>
        /// 吋的數值，會被FromInchDecimal和ToInchDecimal使用
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// 如果這個執行個體是從FromCmValue過來，會存在這邊，如果被ToCmDecimal，會先看這邊有沒有值，有的話直接回傳，沒有才去Value做運算
        /// </summary>
        public decimal? CmValue { get; set; }

        private int? _Piece;

        /// <summary>
        /// 八分之一吋，作為最小單位會被FromInchText和ToInchText使用
        /// </summary>
        public int? Piece
        {
            get
            {
                if (this._Piece.HasValue == false)
                {
                    if (this.Value.HasValue == false)
                    {
                        return null;
                    }

                    this._Piece = DecimalToPiece(this.Value.Value);
                }

                return this._Piece;
            }

            set
            {
                this._Piece = value;
                this.Value = PieceToDecimal(value);
            }
        }

        /// <inheritdoc/>
        public string ToInchText()
        {
            return PieceToText(this.Piece.GetValueOrDefault(0));
        }

        /// <inheritdoc/>
        public decimal ToInchDecimal()
        {
            return this.Value.GetValueOrDefault();
        }

        /// <inheritdoc/>
        public decimal ToCmDecimal()
        {
            if (this.CmValue.HasValue)
            {
                return this.CmValue.Value;
            }
            else
            {
                return this.Piece.GetValueOrDefault(0) * InchPieceUnit * TransRate;
            }
        }
    }
}

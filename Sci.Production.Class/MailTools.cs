using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class
{
    /// <summary>
    /// mail tools
    /// </summary>
    public class MailTools
    {
        public static SendMail_Result SendMail(SendMail_Request SendMail_Request, bool isTest = false)
        {
            SendMail_Result sendMail_Result = new SendMail_Result();
            DualResult dualResult;
            try
            {
                string mailFrom = "foxpro@sportscity.com.tw";
                string mailServer = "Mail.sportscity.com.tw";
                string EmailID = "foxpro";
                string EmailPwd = "orpxof";

                string result = string.Empty;
                //寄件者 & 收件者

                if (!isTest)
                {
                    if (!(dualResult = DBProxy.Current.Select(null, "select * from Production.dbo.System", out DataTable dt)))
                    {
                        sendMail_Result.result = false;
                        sendMail_Result.resultMsg = "Get system datas fail!";
                        return sendMail_Result;
                    }

                    if (dt != null || dt.Rows.Count > 0)
                    {
                        mailFrom = dt.Rows[0]["Sendfrom"].ToString();
                        mailServer = dt.Rows[0]["mailServer"].ToString();
                        EmailID = dt.Rows[0]["eMailID"].ToString();
                        EmailPwd = dt.Rows[0]["eMailPwd"].ToString();
                    }
                    else
                    {
                        sendMail_Result.result = false;
                        sendMail_Result.resultMsg = "Get system datas fail!";
                        return sendMail_Result; ;
                    }
                }

                MailMessage message = new MailMessage();
                message.From = new MailAddress(mailFrom);

                if (SendMail_Request.To != null && SendMail_Request.To != string.Empty)
                {
                    foreach (var to in SendMail_Request.To.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(to))
                        {
                            message.To.Add(to);
                        }
                    }
                }

                if (SendMail_Request.CC != null && SendMail_Request.CC != string.Empty)
                {
                    foreach (var cc in SendMail_Request.CC.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(cc))
                        {
                            message.CC.Add(cc);
                        }
                    }
                }

                if (SendMail_Request.Subject != null)
                {
                    message.Subject = SendMail_Request.Subject;
                }

                message.IsBodyHtml = true;
                if (SendMail_Request.Body != null)
                {
                    message.Body = SendMail_Request.Body;
                }

                // mail Smtp
                SmtpClient client = new SmtpClient(mailServer);

                // 寄件者 帳密
                client.Credentials = new NetworkCredential(EmailID, EmailPwd);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(message);

                sendMail_Result.result = true;
                sendMail_Result.resultMsg = string.Empty;
            }
            catch (System.Exception ex)
            {
                sendMail_Result.result = false;
                sendMail_Result.resultMsg = ex.ToString();
            }

            return sendMail_Result;
        }

        public static string HtmlStyle()
        {
            string style = @"
<style>
    .DataTable {
        width: 92vw;
        font-size: 1rem;
        font-weight: bold;
        border: solid 1px black;
        background-color: white;
    }
        .DataTable > tbody > tr:nth-of-type(odd) {
            background-color: #ffffff;
        }

        .DataTable > tbody > tr:nth-of-type(even) {
            background-color: #F0F2F2;
        }

        .DataTable > tbody > tr > td {
            border: solid 1px gray;
            padding: 1em;
            text-align: left;
            vertical-align: middle;
        }
</style>
";
            return style;
        }

        /// <summary>
        /// datatable to HTML
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>string</returns>
        public static string DataTableChangeHtml(DataTable dt)
        {
            string html = "<html> ";

            html += HtmlStyle();

            html += "<body> ";
            html += "<table class='DataTable'> ";
            html += "<thead><tr> ";
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                html += "<th>" + dt.Columns[i].ColumnName + "</th> ";
            }

            html += "</tr></thead> ";
            html += "<tbody> ";
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                html += "<tr> ";
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td> ";
                }

                html += "</tr> ";
            }

            html += "</tbody> ";
            html += "</table> ";
            html += "</body> ";
            html += "</html> ";
            return html;
        }

        /// <summary>
        /// datatable to HTML without Style and Html header
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>string</returns>
        public static string DataTableChangeHtml_WithOutStyleHtml(DataTable dt)
        {
            string html = "<html> ";

            html += @"
<style>
    .tg {border-collapse:collapse;border-spacing:0;}
.tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}
.tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;border-color:black;}
        }
</style>
";

            html += "<body> ";
            html += "<table class='tg'> ";
            html += "<thead><tr bgcolor=\"#FFDEA1\" > ";
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                html += "<th>" + dt.Columns[i].ColumnName + "</th> ";
            }

            html += "</tr></thead> ";
            html += "<tbody> ";
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                html += "<tr> ";
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td> ";
                }

                html += "</tr> ";
            }

            html += "</tbody> ";
            html += "</table> ";
            html += "</body> ";
            html += "</html> ";
            return html;
        }

        public class SendMail_Request
        {
            public string To { get; set; }
            public string CC { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }

        }

        public class SendMail_Result
        {
            public string resultMsg { get; set; }
            public bool result { get; set; }
        }
    }
}

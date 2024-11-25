using Ict;
using Newtonsoft.Json;
using Sci.Data;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Sci.CfgSection;

namespace Sci.Production.Win
{
    /// <summary>
    /// OTPVerification
    /// </summary>
    public partial class OTPVerification : Sci.Win.Tools.Base
    {
        private UserInfo userInfo = new UserInfo();
        private string uri;

        /// <inheritdoc/>
        public OTPVerification(UserInfo userInfo)
        {
            this.InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.userInfo = userInfo;
            this.displayBoxAccount.Text = this.userInfo.UserID;

            // 設定支持的 SSL/TLS 版本
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.SendOTP();
        }

        private void BtnSendOtp_Click(object sender, EventArgs e)
        {
            this.SendOTP();
        }

        private async void SendOTP()
        {
            try
            {
                this.btnSendOtp.Enabled = false;

                // 取得並寄送 OTP
                await this.GetOtpAsync();
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
            finally
            {
                // 按鈕禁用並開始倒數
                await this.StartCountdownAsync();
            }
        }

        private async Task StartCountdownAsync()
        {
            int countdown = 60; // 設定倒數秒數

            while (countdown > 0)
            {
                this.btnSendOtp.Text = $"{countdown}S";
                await Task.Delay(1000); // 等待一秒鐘
                countdown--;
            }

            // 倒數結束後，重置按鈕狀態
            this.btnSendOtp.Text = "Send Again";
            this.btnSendOtp.Enabled = true;
        }

        private async Task GetOtpAsync()
        {
            var cfgsection = (CfgSection)ConfigurationManager.GetSection("sci");
            string apiUrl = string.Empty;
            foreach (CfgSection.Module it in cfgsection.Modules)
            {
                if (it.Name != DBProxy.Current.DefaultModuleName)
                {
                    continue;
                }

                foreach (ConnectionStringElement connectionStringItem in it.ConnectionStrings)
                {
                    if (connectionStringItem.Name == "mfaOTP")
                    {
                        apiUrl = connectionStringItem.ConnectionString;
                        break;
                    }
                }
            }

            if (MyUtility.Check.Empty(apiUrl))
            {
                throw new Exception("Connection string not found.");
            }

            string[] strAry = apiUrl.Split(';');
            this.uri = strAry[0].ToString();
            string account = strAry[1].ToString();
            string password = strAry[2].ToString();

            AccessTokenRequest request = new AccessTokenRequest
            {
                application = "WebAPI",
                applicationCredential = "KeyOfWebAPIApplication",
                name = account,
                credential = password,
                timestamp = this.GetUnixTimestamp(),
                refreshAccessToken = true,
            };

            // 取得 access token
            var accessTokenResponse = await this.GetAccessTokenAsync(request);
            if (accessTokenResponse.errorCode != 0)
            {
                throw new Exception($"Failed to get access token: {accessTokenResponse.errorMsg}");
            }

            // 使用 accessToken 發送 GET 請求來取得 OTP
            var otpResponse = await this.GetOtpFromServerAsync(accessTokenResponse.accessToken, request.timestamp);
            if (otpResponse.errorCode != 0)
            {
                if (otpResponse.errorMsg.Contains("no e-mail"))
                {
                    throw new Exception($"Please ask the Local-IT for help in setting up the email address.");
                }

                throw new Exception($"Failed to get OTP: {otpResponse.errorMsg}");
            }
        }

        private async Task<AccessTokenResponse> GetAccessTokenAsync(AccessTokenRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // 將請求物件序列化為 JSON 字串
                    string jsonRequest = JsonConvert.SerializeObject(request);

                    // 建立 HttpContent 物件，指定內容類型為 application/json
                    HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    // 發送 POST 請求
                    HttpResponseMessage response = await httpClient.PostAsync(this.uri + "login/password", content);

                    // 確保響應是成功的
                    response.EnsureSuccessStatusCode();

                    // 讀取響應內容作為字串
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // 將 JSON 字串反序列化為 AccessTokenResponse 物件
                    AccessTokenResponse accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(jsonResponse);

                    return accessTokenResponse;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HttpRequestException: {e.Message}");
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected exception: {e.Message}");
                    throw;
                }
            }
        }

        private async Task<AccessTokenResponse> GetOtpFromServerAsync(string accessToken, long timestamp)
        {
            using (var httpClient = new HttpClient())
            {
                string name = this.userInfo.UserID;

                // 構建帶參數的 URL
                string url = $"{this.uri}verifyData/onDemand?accessToken={accessToken}&timeStamp={timestamp}&name={name}";

                // 發送 GET 請求
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // 確保響應是成功的
                response.EnsureSuccessStatusCode();

                // 讀取響應內容作為字串
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // 將 JSON 字串反序列化為 AccessTokenResponse 物件
                AccessTokenResponse otpResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(jsonResponse);

                return otpResponse;
            }
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                AccessTokenRequest request = new AccessTokenRequest
                {
                    application = "WebAPI",
                    applicationCredential = "KeyOfWebAPIApplication",
                    name = this.userInfo.UserID,
                    credential = this.txtOTP.Text, // 使用者輸入的 OTP
                    timestamp = this.GetUnixTimestamp(),
                    refreshAccessToken = true,
                };

                // 發送驗證請求
                var response = await this.VerifyOtpAsync(request);

                // 檢查響應
                if (response.errorCode == 0)
                {
                    // 驗證成功，關閉對話框並返回OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MyUtility.Msg.ErrorBox($"Invalid verification code.");
                }
            }
            catch (Exception ex)
            {
                // 處理例外狀況，例如網路問題等
                this.ShowErr(ex);
            }
        }

        private async Task<AccessTokenResponse> VerifyOtpAsync(AccessTokenRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                // 將請求物件序列化為 JSON 字串
                string jsonRequest = JsonConvert.SerializeObject(request);

                // 建立 HttpContent 物件，指定內容類型為 application/json
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // 發送 POST 請求到 verifyData/onDemand
                HttpResponseMessage response = await httpClient.PostAsync(this.uri + "verifyData/onDemand", content);

                // 確保響應是成功的
                response.EnsureSuccessStatusCode();

                // 讀取響應內容作為字串
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // 將 JSON 字串反序列化為 AccessTokenResponse 物件
                AccessTokenResponse otpResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(jsonResponse);

                return otpResponse;
            }
        }

        // 計算 Unix 時間戳的函數
        private long GetUnixTimestamp()
        {
            // 獲取當前 UTC 時間
            DateTime utcNow = DateTime.UtcNow;

            // Unix epoch 開始的時間
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // 計算時間戳（秒）
            long timestamp = (long)(utcNow - epochStart).TotalSeconds;

            return timestamp;
        }

        // 計算 Unix 時間戳（毫秒）的函數
        private long GetUnixTimestampMilliseconds()
        {
            // 獲取當前 UTC 時間
            DateTime utcNow = DateTime.UtcNow;

            // Unix epoch 開始的時間
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // 計算時間戳（毫秒）
            long timestampMilliseconds = (long)(utcNow - epochStart).TotalMilliseconds;

            return timestampMilliseconds;
        }

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Elements should be documented
        public class AccessTokenRequest
        {
            public string application;
            public string applicationCredential;
            public string name;
            public string credential;
            public long timestamp;
            public bool refreshAccessToken;
        }

        public class AccessTokenResponse
        {
            public int errorCode;
            public string accessToken;
            public string errorMsg;
            public string otp;
        }
#pragma warning restore SA1401 // Elements should be documented
#pragma warning restore SA1600 // Elements should be documented
    }
}

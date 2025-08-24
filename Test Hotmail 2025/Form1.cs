using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using MimeKit;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Test_Hotmail_2025
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnRead_Click(object sender, EventArgs e)
        {
            btnRead.Enabled = false;
            dgvInbox.Rows.Clear();

            try
            {
                string email = txtEmail.Text.Trim();
                string clientId = txtClientId.Text.Trim();
                string refreshToken = txtRefreshToken.Text.Trim();

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(refreshToken))
                {
                    MessageBox.Show("Nhập đủ Email, ClientId, RefreshToken.", "Thiếu thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maxCount = 10;
                //if (int.TryParse(txtMaxCount.Text.Trim(), out var n) && n > 0)
                //    maxCount = n;

                var accessToken = await GetAccessTokenAsync(refreshToken, clientId);

                using var imap = ImapConnect(email, accessToken);

                foreach (var msg in FetchInboxMessages(imap, maxCount))
                {
                    var from = string.Join(", ", msg.From.Select(a => a.ToString()));
                    var subject = msg.Subject ?? "";

                    var bodyText = msg.TextBody;
                    if (string.IsNullOrEmpty(bodyText))
                        bodyText = StripHtml(msg.HtmlBody ?? "");

                    dgvInbox.Rows.Add(subject, from, subject, Truncate(bodyText ?? "", 300));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRead.Enabled = true;
            }
        }


        private static async Task<string> GetAccessTokenAsync(string refreshToken, string clientId)
        {
            const string tokenUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
            var scopes =
                "offline_access " +
                "https://outlook.office.com/IMAP.AccessAsUser.All " +
                "https://outlook.office.com/POP.AccessAsUser.All " +
                "https://outlook.office.com/SMTP.Send";

            using var http = new HttpClient();
            var form = new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["scope"] = scopes
            };

            var resp = await http.PostAsync(tokenUrl, new FormUrlEncodedContent(form));
            var json = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Refresh failed {(int)resp.StatusCode}: {json}");

            var obj = JObject.Parse(json);
            var accessToken = (string?)obj["access_token"];
            if (string.IsNullOrEmpty(accessToken))
                throw new Exception("Token response invalid: " + json);

            // (Tuỳ chọn) Nếu muốn xoay vòng refresh token:
            // var newRefresh = (string?)obj["refresh_token"]; -> lưu lại
            return accessToken!;
        }

        private static ImapClient ImapConnect(string email, string accessToken)
        {
            var imap = new ImapClient();
            imap.Connect("outlook.office365.com", 993, SecureSocketOptions.SslOnConnect);
            var oauth2 = new MailKit.Security.SaslMechanismOAuth2(email, accessToken);
            imap.Authenticate(oauth2);
            return imap;
        }

        private sealed record MailFound(string Folder, string Subject, string From, string BodyText, string? UrlFound);

        private static MailFound? ReadEmail(ImapClient imap, string subjectEquals, string? urlRegex = null)
        {
            var foldersToCheck = new[] { "INBOX", "Junk" };

            foreach (var f in foldersToCheck)
            {
                var folder = imap.GetFolder(f);
                folder.Open(FolderAccess.ReadOnly);

                var uids = folder.Search(SearchQuery.All);
                foreach (var uid in uids)
                {
                    var msg = folder.GetMessage(uid);

                    if (!string.Equals(msg.Subject ?? "", subjectEquals, StringComparison.Ordinal))
                        continue;

                    // Ưu tiên text/plain → fallback html (strip)
                    string bodyText = msg.TextBody ?? "";
                    if (string.IsNullOrEmpty(bodyText))
                    {
                        var html = msg.HtmlBody ?? "";
                        bodyText = StripHtml(html);
                    }

                    string? url = null;
                    if (!string.IsNullOrEmpty(urlRegex))
                    {
                        var m = Regex.Match(bodyText, urlRegex, RegexOptions.IgnoreCase);
                        if (m.Success) url = m.Value;
                    }

                    var from = string.Join(", ", msg.From.Select(a => a.ToString()));
                    return new MailFound(f, msg.Subject ?? "", from, bodyText, url);
                }
            }
            return null; // không thấy
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            var noScript = Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var noStyle = Regex.Replace(noScript, "<style.*?</style>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var noTags = Regex.Replace(noStyle, "<.*?>", " ");
            var decoded = System.Net.WebUtility.HtmlDecode(noTags);
            return Regex.Replace(decoded, @"\s{2,}", " ").Trim();
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max) + "…";
        }
        private static IEnumerable<MimeMessage> FetchInboxMessages(ImapClient imap, int? maxCount)
        {
            var inbox = imap.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            var uids = inbox.Search(SearchQuery.All);
            if (uids == null || uids.Count == 0) yield break;

            var ordered = uids.OrderBy(u => u.Id).ToList();

            if (maxCount.HasValue && maxCount.Value > 0 && maxCount.Value < ordered.Count)
                ordered = ordered.Skip(ordered.Count - maxCount.Value).ToList();

            for (int i = ordered.Count - 1; i >= 0; i--)
            {
                MimeMessage msg = null!;
                try
                {
                    msg = inbox.GetMessage(ordered[i]);
                }
                catch { continue; }
                yield return msg;
            }
        }
    }
}

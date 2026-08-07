using System.Web;

namespace Naitrust.Application.ExternalServices.Communication;

public static class EmailTemplateHelper
{
    private const string PrimaryColor = "#1E90FF";
    private const string CompanyName = "Naitrust";
    private const string SupportEmail = "support@naitrust.com";

    private const string StyleText = "color: #52525b;";
    private const string StyleDark = "color: #18181b;";
    private const string StyleBold = "font-weight: 600;";
    private const string StyleFont = "font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;";
    private const string StyleOtpBox = "background-color: #f4f4f5; border: 1px solid #e4e4e7; border-radius: 12px; padding: 24px; text-align: center; margin: 24px 0;";
    private const string StyleOtpCode = "font-family: 'Courier New', Courier, monospace; font-size: 36px; font-weight: 700; letter-spacing: 8px; color: #18181b;";
    private const string StyleWarning = "background-color: #fffbeb; border-left: 3px solid #f59e0b; border-radius: 4px; padding: 14px 16px; margin: 20px 0; font-size: 13px; color: #92400e;";

    public static string GenerateEmailHtml(string title, string content, string websiteUrl, string? buttonText = null, string? buttonUrl = null)
    {
        var year = DateTime.UtcNow.Year;
        var buttonBlock = string.IsNullOrEmpty(buttonText) ? "" : $@"
          <tr>
            <td align=""center"" style=""padding: 8px 0 28px 0;"">
              <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"">
                <tr>
                  <td align=""center"" style=""border-radius: 8px; background-color: {PrimaryColor};"">
                    <a href=""{buttonUrl}"" target=""_blank"" style=""display: inline-block; padding: 14px 44px; {StyleFont} font-size: 15px; font-weight: 600; color: #ffffff; text-decoration: none; border-radius: 8px; letter-spacing: 0.3px;"">
                      {buttonText}
                    </a>
                  </td>
                </tr>
              </table>
            </td>
          </tr>";

        return $@"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <meta charset=""utf-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
  <meta name=""color-scheme"" content=""light"">
  <meta name=""supported-color-schemes"" content=""light"">
  <title>{HttpUtility.HtmlEncode(title)}</title>
</head>
<body style=""margin: 0; padding: 0; width: 100%; background-color: #f4f4f5; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;"">
  <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #f4f4f5;"">
    <tr>
      <td align=""center"" style=""padding: 40px 16px;"">
        <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 520px; background-color: #ffffff; border-radius: 12px; overflow: hidden; border: 1px solid #e4e4e7;"">
          <!-- Logo -->
          <tr>
            <td align=""center"" style=""padding: 32px 40px 16px 40px;"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td style=""{StyleFont} font-size: 26px; font-weight: 700; color: {PrimaryColor}; letter-spacing: -0.5px;"">
                    {CompanyName}
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <!-- Divider -->
          <tr>
            <td style=""padding: 0 40px;"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr><td style=""border-bottom: 1px solid #e4e4e7; font-size: 0; line-height: 0;"">&nbsp;</td></tr>
              </table>
            </td>
          </tr>
          <!-- Title -->
          <tr>
            <td align=""center"" style=""padding: 28px 40px 8px 40px;"">
              <h1 style=""margin: 0; {StyleFont} font-size: 22px; font-weight: 700; color: #18181b; line-height: 1.3; text-align: center;"">
                {HttpUtility.HtmlEncode(title)}
              </h1>
            </td>
          </tr>
          <!-- Content -->
          <tr>
            <td style=""padding: 8px 40px 16px 40px; {StyleFont} font-size: 15px; line-height: 1.7; color: #52525b;"">
              {content}
            </td>
          </tr>
          <!-- Button -->
          {buttonBlock}
          <!-- Footer divider -->
          <tr>
            <td style=""padding: 0 40px;"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr><td style=""border-bottom: 1px solid #e4e4e7; font-size: 0; line-height: 0;"">&nbsp;</td></tr>
              </table>
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td align=""center"" style=""padding: 24px 40px 32px 40px; background-color: #fafafa;"">
              <p style=""margin: 0 0 8px 0; {StyleFont} font-size: 12px; color: #a1a1aa; line-height: 1.5;"">
                &copy; {year} Naitrust Digital Solutions Limited
              </p>
              <p style=""margin: 0; {StyleFont} font-size: 12px; color: #a1a1aa;"">
                <a href=""{websiteUrl}"" style=""color: {PrimaryColor}; text-decoration: none;"">Website</a>
                &nbsp;&middot;&nbsp;
                <a href=""mailto:{SupportEmail}"" style=""color: {PrimaryColor}; text-decoration: none;"">Support</a>
              </p>
              <p style=""margin: 12px 0 0 0; {StyleFont} font-size: 11px; color: #d4d4d8;"">
                If you didn't request this, you can safely ignore this email.
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";
    }

    public static string BuildOtpContent(string greeting, string message, string otp, string? footerNote = null)
    {
        var footer = footerNote ?? "This code expires in 15 minutes.";
        return $@"
          <p style=""{StyleText} {StyleFont} font-size: 15px;"">Hello <span style=""{StyleDark} {StyleBold}"">{HttpUtility.HtmlEncode(greeting)}</span>,</p>
          <p style=""{StyleText} {StyleFont} font-size: 15px;"">{HttpUtility.HtmlEncode(message)}</p>
          <div style=""{StyleOtpBox}""><span style=""{StyleOtpCode}"">{HttpUtility.HtmlEncode(otp)}</span></div>
          <p style=""{StyleText} {StyleFont} font-size: 15px; text-align: center;"">Enter this code in the verification field.</p>
          <div style=""{StyleWarning}""><span style=""{StyleBold}"">Security notice:</span> {HttpUtility.HtmlEncode(footer)} Never share it with anyone.</div>";
    }

    public static string BuildWelcomeContent(string firstName)
    {
        return $@"
          <p style=""{StyleText} {StyleFont} font-size: 15px;"">Hello <span style=""{StyleDark} {StyleBold}"">{HttpUtility.HtmlEncode(firstName)}</span>,</p>
          <p style=""{StyleText} {StyleFont} font-size: 15px;"">Your email has been verified and your account is ready to go.</p>
          <p style=""{StyleText} {StyleFont} font-size: 15px;"">You can now create secure escrow transactions, invite parties, and track payments — all in one place.</p>
          <p style=""{StyleText} {StyleFont} font-size: 15px;"">If you have any questions, our support team is here to help.</p>";
    }

    public static string MaskEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains('@'))
        {
            return email;
        }

        var parts = email.Split('@');
        var local = parts[0];
        var domain = parts[1];
        if (local.Length <= 2)
        {
            return $"{local[0]}***@{domain}";
        }

        return $"{local[..2]}***{local[^1]}@{domain}";
    }
}

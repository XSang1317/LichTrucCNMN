using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LichTruc.Utils
{
    public class EmailUtils
    {
        //vào trang https://myaccount.google.com/lesssecureapps?pli=1 để tắt security
        private readonly EmailSettings _emailSettings;
        public bool mailSent { get; set; }
        public string status { get; set; }

        public EmailUtils(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
            if (_emailSettings.UserName == "") _emailSettings.UserName = _emailSettings.SenderEmail;
            mailSent = false;
            status = "";
        }
        public async Task SendEmail(string to_email, string Subject, string Body, string cc_email = "", string bcc_email = "")
        {
            /*var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.SenderEmail);
            mailMessage.To.Add(email);
            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = false;
            
            using (var client = new SmtpClient(_emailSettings.MailServer))
            {
                client.Port = _emailSettings.MailPort;
                client.UseDefaultCredentials = true;
                client.EnableSsl = _emailSettings.EnableSsl;
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password);                                
                client.Send(mailMessage);
                
            }*/

            using (var smtp = new SmtpClient
            {
                Host = _emailSettings.MailServer,
                Port = _emailSettings.MailPort,
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),                
            })
            {
                //smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                using (var message = new MailMessage(_emailSettings.SenderEmail, to_email)
                {
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = true
                })
                {
                    string[] ccmails = cc_email.Split(',');
                    foreach (string CCEmail in ccmails)
                    {
                        if (CCEmail != "") message.CC.Add(CCEmail);
                        //Console.WriteLine("Địa chỉ mail cc:" + CCEmail);
                    }
                    //if (cc_email != "") message.CC.Add(cc_email);
                    await smtp.SendMailAsync(message);
                }
            }
            /*HTMLMessageContent = Message;
            EmailLog emailModel = new EmailLog
            {
                EmailType = EmailType,
                Subject = Subject,
                EmailContent = Message,
                FromEmail = _emailSettings.SenderEmail,
                ToEmails = email,
                CreatedBy = sessionData != null ? sessionData.ApplicationUserId : "",
                OrganizationId = sessionData != null ? sessionData.OrganizationId : 0

            };

            var from = new EmailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            var to = new EmailAddress(email, "");
            var msg = MailHelper.CreateSingleEmail(from, to, Subject, Message, HTMLMessageContent);
            var response = await client.SendEmailAsync(msg);
            return Ok("Success");*/
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                //Console.WriteLine("[{0}] Send canceled.", token);
                status = "Send canceled";
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                status = "e.Error.ToString()";
            }
            else
            {
                //Console.WriteLine("Message sent.");
                status = "sent";
            }
            mailSent = true;
        }
    }

    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailKey { get; set; }
        public string? UserName { get; set; } = "";
    }
}

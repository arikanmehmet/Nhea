using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace Nhea.Communication
{
    internal class Mail
    {
        protected internal Mail()
        {
            Attachments = new List<Attachment>();
        }

        protected internal Guid Id { get; set; }

        protected internal int? MailProviderId { get; set; }

        protected internal string From { get; set; }

        protected internal string ToRecipient { get; set; }

        protected internal string CcRecipients { get; set; }

        protected internal string BccRecipients { get; set; }

        protected internal string Subject { get; set; }

        protected internal string Body { get; set; }

        protected internal DateTime Priority { get; set; }

        protected internal DateTime CreateDate { get; set; }

        protected internal bool HasAttachment { get; set; }

        //TODO: params yapal�m buray�
        protected internal List<Attachment> Attachments { get; set; }

        protected internal static void Send(string from, string toRecipient, string ccRecipients, string bccRecipients, string subject, string body)
        {
            SmtpHelper.SendMail(from, toRecipient, ccRecipients, bccRecipients, subject, body);
        }

        protected internal void Send()
        {
            int tryCount = 0;

            while (true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(Nhea.Configuration.Settings.Communication.OpenReportUrl))
                    {
                        string openReportUrl = Nhea.Configuration.Settings.Communication.OpenReportUrl;

                        if (!openReportUrl.EndsWith("?"))
                        {
                            openReportUrl += "?";
                        }

                        string key = "i=" + this.Id;

                        string hash = Nhea.Security.QueryStringSecurity.HashQueryString(key);

                        string imageHtml = String.Format("<img src=\"{0}\" height=\"1\" width=\"1\"/>", openReportUrl + hash);

                        this.Body += imageHtml;
                    }

                    SmtpHelper.SendMail(From, ToRecipient, CcRecipients, BccRecipients, Subject, Body, false, Attachments);
                    MailQueue.MoveToHistory(this, MailStatus.Sent);
                    return;
                }
                catch (Exception ex)
                {
                    if (tryCount < 10)
                    {
                        tryCount++;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        protected internal bool Save()
        {
            return MailQueue.Add(From, ToRecipient, CcRecipients, BccRecipients, Subject, Body);
        }

        protected internal void Delete()
        {
            MailQueue.Delete(this);
        }

        protected internal void SetError()
        {
            try
            {
                MailQueue.SetError(Id);
            }
            catch
            {
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Nhea.Communication;
using Nhea.Logging;

namespace Nhea.Web.Services.ScheduledServices
{
    public class MailQueueService
    {
        private ILogger CurrentLogger { get; set; }

        public MailQueueService(ILogger logger = null)
        {
            CurrentLogger = logger;
            SleepTime = 20000;
        }

        public MailQueueService(int sleepTime, ILogger logger = null)
        {
            CurrentLogger = logger;
            SleepTime = sleepTime;
        }

        public int SleepTime { get; set; }

        public void StartService()
        {
            while (true)
            {
                try
                {
                    Execute();
                }
                catch (Exception ex)
                {
                    if (CurrentLogger != null)
                    {
                        CurrentLogger.LogError(ex, ex.Message);
                    }
                    else
                    {
                        Logger.Log(ex, false);
                    }
                }

                Thread.Sleep(SleepTime);
            }
        }

        public void Execute()
        {
            List<Mail> mailList = new List<Mail>();

            foreach (KeyValuePair<int, MailProvider> item in MailProvider.Providers)
            {
                int standByPeriod = 1;

                MailProvider provider = item.Value;

                if (provider.StandbyPeriod > standByPeriod)
                {
                    standByPeriod = provider.StandbyPeriod;
                }

                SendMailList(MailQueue.Fetch(provider));

                Thread.Sleep(standByPeriod * 1000);
            }

            mailList.AddRange(MailQueue.Fetch());

            SendMailList(mailList);

            mailList.AddRange(MailQueue.Fetch());
        }

        private void SendMailList(List<Mail> mailList)
        {
            foreach (Mail mail in mailList)
            {
                try
                {
                    mail.Send();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    mail.SetError();

                    if (CurrentLogger != null)
                    {
                        CurrentLogger.LogError(ex, ex.Message);
                    }
                    else
                    {
                        Logger.Log(ex, false);
                    }
                }
            }
        }
    }
}

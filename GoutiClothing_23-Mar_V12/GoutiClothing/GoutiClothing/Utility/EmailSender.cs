﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace GoutiClothing.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //throw new NotImplementedException();

            return Task.CompletedTask;
        }
    }
}

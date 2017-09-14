﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookKeeperSPAAngular.Model
{
    public class FileMessageService : IMessageService
    {
        Task IMessageService.Send(string email, string subject, string message)
        {
            var emailMessage = $"To: {email}\nSubject: {subject}\nMessage: {message}\n\n";

            File.AppendAllText("emails.txt", emailMessage);

            return Task.FromResult(0);
        }
    }
}

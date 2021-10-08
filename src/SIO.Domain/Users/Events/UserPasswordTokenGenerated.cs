﻿using SIO.Infrastructure.Events;

namespace SIO.Domain.Users.Events
{
    public class UserPasswordTokenGenerated : Event
    {
        public string Token { get; set; }

        public UserPasswordTokenGenerated(string subject, int version) : base(subject, version)
        {
        }
    }
}

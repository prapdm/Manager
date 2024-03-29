﻿using System;

namespace Manager.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } 
        public string PasswordHash { get; set; }
        public int VerifcationCode{ get; set; }
        public string VerifcationToken{ get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Role Role { get; set; }
    }
}

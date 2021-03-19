﻿// ReSharper disable VirtualMemberCallInConstructor
namespace Quizizz.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Quizizz.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.StudentIngroups = new HashSet<StudentGroup>();
            this.Students = new HashSet<ApplicationUser>();
            this.CreatedQuizzes = new HashSet<Quiz>();
            this.Results = new HashSet<Result>();
            this.CreatedGroups = new HashSet<Group>();
            this.CreatedEvents = new HashSet<Event>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string TeacherId { get; set; }

        public virtual ApplicationUser Teacher { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<StudentGroup> StudentIngroups { get; set; }

        public virtual ICollection<ApplicationUser> Students { get; set; }

        public virtual ICollection<Quiz> CreatedQuizzes { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public virtual ICollection<Group> CreatedGroups { get; set; }

        public virtual ICollection<Event> CreatedEvents { get; set; }
    }
}
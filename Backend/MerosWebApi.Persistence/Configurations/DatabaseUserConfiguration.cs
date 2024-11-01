using MerosWebApi.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Configurations
{
    public class DatabaseUserConfiguration : IEntityTypeConfiguration<DatabaseUser>
    {
        public void Configure(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.ToCollection("users");

            builder.Property(u => u.Id)
                .HasElementName("_id");

            builder.Property(u => u.Full_name)
                .HasElementName("full_name")
                .HasMaxLength(150);

            builder.Property(u => u.IsActive)
                .HasElementName("is_active")
                .IsRequired();

            ConfigureUserTimesFields(builder);
            ConfigureEmailFields(builder);
            ConfigurePasswordFields(builder);
            ConfigureLoginFailedFields(builder);

            builder.HasIndex(u => u.Email);
            builder.HasIndex(u => u.IsActive);
        }

        public void ConfigureUserTimesFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.CreatedAt)
                .HasElementName("created_at")
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .HasElementName("updated_at");

            builder.Property(u => u.LastLoginAt)
                .HasElementName("last_login_at");
        }

        public void ConfigurePasswordFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.PasswordHash)
                .HasElementName("pwd_hash");

            builder.Property(u => u.PasswordSalt)
                .HasElementName("pwd_salt");

            builder.Property(u => u.ResetPasswordCode)
                .HasElementName("reset_pwd_code");

            builder.Property(u => u.ResetPasswordCount)
                .HasElementName("reset_pwd_count");

            builder.Property(u => u.ResetPasswordCreatedAt)
                .HasElementName("reset_pwd_cr_at");
        }

        public void ConfigureEmailFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.Email)
                .HasElementName("email")
                .HasMaxLength(320);

            builder.Property(u => u.UnconfirmedEmail)
                .HasElementName("unconf_email");

            builder.Property(u => u.UnconfirmedEmailCreatedAt)
                .HasElementName("unconf_email_cr_at");

            builder.Property(u => u.UnconfirmedEmailCode)
                .HasElementName("unconf_email_code");

            builder.Property(u => u.UnconfirmedEmailCount)
                .HasElementName("unconf_email_count");


        }

        public void ConfigureLoginFailedFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.LoginFailedAt)
                .HasElementName("login_fail_at");

            builder.Property(u => u.LoginFailedCount)
                .HasElementName("login_fail_count");
        }
    }
}

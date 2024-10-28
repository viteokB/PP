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
                .HasField("_id");

            builder.Property(u => u.Full_name)
                .HasField("full_name")
                .HasMaxLength(150);

            builder.Property(u => u.IsActive)
                .HasField("is_active");

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
                .HasField("created_at")
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .HasField("updated_at");

            builder.Property(u => u.LastLoginAt)
                .HasField("last_login_at");
        }

        public void ConfigurePasswordFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.PasswordHash)
                .HasField("pwd_hash");

            builder.Property(u => u.PasswordSalt)
                .HasField("pwd_salt");

            builder.Property(u => u.ResetPasswordCode)
                .HasField("reset_pwd_code");

            builder.Property(u => u.ResetPasswordCount)
                .HasField("reset_pwd_count");

            builder.Property(u => u.ResetPasswordCreatedAt)
                .HasField("reset_pwd_cr_at");
        }

        public void ConfigureEmailFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.Email)
                .HasField("email")
                .HasMaxLength(320);

            builder.Property(u => u.UnconfirmedEmail)
                .HasField("unconf_email");

            builder.Property(u => u.UnconfirmedEmailCreatedAt)
                .HasField("unconf_email_cr_at");

            builder.Property(u => u.UnconfirmedEmailCode)
                .HasField("unconf_email_code");

            builder.Property(u => u.UnconfirmedEmailCount)
                .HasField("unconf_email_count");


        }

        public void ConfigureLoginFailedFields(EntityTypeBuilder<DatabaseUser> builder)
        {
            builder.Property(u => u.LoginFailedAt)
                .HasField("login_fail_at");

            builder.Property(u => u.LoginFailedCount)
                .HasField("login_fail_count");
        }
    }
}

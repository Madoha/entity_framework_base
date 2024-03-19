﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .HasOne(a => a.Course)
                .WithOne(c => c.Author)
                .HasForeignKey<AuthorEntity>(a => a.CourseId);
        }
    }
}

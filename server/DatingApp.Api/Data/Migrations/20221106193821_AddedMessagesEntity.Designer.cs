﻿// <auto-generated />
using System;
using DatingApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatingApp.Api.Data.Migrations
{
    [DbContext(typeof(DatingAppDbContext))]
    [Migration("20221106193821_AddedMessagesEntity")]
    partial class AddedMessagesEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DatingApp.Api.Entities.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Interests")
                        .HasColumnType("longtext");

                    b.Property<string>("Introduction")
                        .HasColumnType("longtext");

                    b.Property<string>("KnownAs")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("LastActive")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LookingFor")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DateRead")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("DeletedByRecipient")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("DeletedBySender")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("char(36)");

                    b.Property<string>("RecipientUserName")
                        .HasColumnType("longtext");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("char(36)");

                    b.Property<string>("SenderUserName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsMain")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PublicId")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.UserLike", b =>
                {
                    b.Property<Guid>("LikerPersonId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("LikedByPersonId")
                        .HasColumnType("char(36)");

                    b.HasKey("LikerPersonId", "LikedByPersonId");

                    b.HasIndex("LikedByPersonId");

                    b.ToTable("UserLike");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.Message", b =>
                {
                    b.HasOne("DatingApp.Api.Entities.AppUser", "Recipient")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DatingApp.Api.Entities.AppUser", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.Photo", b =>
                {
                    b.HasOne("DatingApp.Api.Entities.AppUser", "AppUser")
                        .WithMany("Photos")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.UserLike", b =>
                {
                    b.HasOne("DatingApp.Api.Entities.AppUser", "LikedByPerson")
                        .WithMany("LikedByUsers")
                        .HasForeignKey("LikedByPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatingApp.Api.Entities.AppUser", "LikerPerson")
                        .WithMany("LikedUsers")
                        .HasForeignKey("LikerPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LikedByPerson");

                    b.Navigation("LikerPerson");
                });

            modelBuilder.Entity("DatingApp.Api.Entities.AppUser", b =>
                {
                    b.Navigation("LikedByUsers");

                    b.Navigation("LikedUsers");

                    b.Navigation("Photos");

                    b.Navigation("ReceivedMessages");

                    b.Navigation("SentMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
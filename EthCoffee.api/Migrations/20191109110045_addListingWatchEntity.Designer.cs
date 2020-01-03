﻿// <auto-generated />
using System;
using EthCoffee.api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EthCoffee.api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20191109110045_addListingWatchEntity")]
    partial class addListingWatchEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("EthCoffee.api.Models.Address", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("ZipCode");

                    b.HasKey("id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("EthCoffee.api.Models.AddressType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("AddressTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "Physical"
                        },
                        new
                        {
                            Id = 2,
                            Type = "Shipping"
                        },
                        new
                        {
                            Id = 3,
                            Type = "Billing"
                        },
                        new
                        {
                            Id = 4,
                            Type = "Postal"
                        },
                        new
                        {
                            Id = 5,
                            Type = "Headquarters"
                        });
                });

            modelBuilder.Entity("EthCoffee.api.Models.Avatar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Avatars");
                });

            modelBuilder.Entity("EthCoffee.api.Models.Listing", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("id");

                    b.HasIndex("UserId");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("EthCoffee.api.Models.ListingPhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<int>("ListingId");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("ListingPhotos");
                });

            modelBuilder.Entity("EthCoffee.api.Models.ListingWatch", b =>
                {
                    b.Property<int>("WatcherId");

                    b.Property<int>("WatchingId");

                    b.HasKey("WatcherId", "WatchingId");

                    b.HasIndex("WatchingId");

                    b.ToTable("ListingWatchs");
                });

            modelBuilder.Entity("EthCoffee.api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Gender");

                    b.Property<DateTime>("LastActiveDate");

                    b.Property<string>("Lastname");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Phone");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EthCoffee.api.Models.UserAddress", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("AddressId");

                    b.Property<int>("AddressTypeId");

                    b.Property<DateTime>("CreatedDate");

                    b.HasKey("UserId", "AddressId", "AddressTypeId");

                    b.HasIndex("AddressId");

                    b.HasIndex("AddressTypeId");

                    b.ToTable("UserAddress");
                });

            modelBuilder.Entity("EthCoffee.api.Models.Avatar", b =>
                {
                    b.HasOne("EthCoffee.api.Models.User", "User")
                        .WithOne("Avatar")
                        .HasForeignKey("EthCoffee.api.Models.Avatar", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EthCoffee.api.Models.Listing", b =>
                {
                    b.HasOne("EthCoffee.api.Models.User", "User")
                        .WithMany("MyListings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EthCoffee.api.Models.ListingPhoto", b =>
                {
                    b.HasOne("EthCoffee.api.Models.Listing", "Listing")
                        .WithMany("Photos")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EthCoffee.api.Models.ListingWatch", b =>
                {
                    b.HasOne("EthCoffee.api.Models.User", "Watcher")
                        .WithMany("Watchings")
                        .HasForeignKey("WatcherId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("EthCoffee.api.Models.Listing", "Watching")
                        .WithMany("Watchers")
                        .HasForeignKey("WatchingId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EthCoffee.api.Models.UserAddress", b =>
                {
                    b.HasOne("EthCoffee.api.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EthCoffee.api.Models.AddressType", "AddressType")
                        .WithMany()
                        .HasForeignKey("AddressTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EthCoffee.api.Models.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

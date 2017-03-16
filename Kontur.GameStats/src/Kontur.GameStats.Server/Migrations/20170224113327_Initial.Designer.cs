using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Kontur.GameStats.Server.Context;

namespace Kontur.GameStats.Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20170224113327_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.2");

            modelBuilder.Entity("Kontur.GameStats.Server.Models.GameModeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ServerModelId");

                    b.Property<string>("value");

                    b.HasKey("Id");

                    b.HasIndex("ServerModelId");

                    b.ToTable("GameModes");
                });

            modelBuilder.Entity("Kontur.GameStats.Server.Models.MatcheModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ServerModelId");

                    b.Property<int>("fragLimit");

                    b.Property<string>("gameMode");

                    b.Property<string>("map");

                    b.Property<double>("timeElapsed");

                    b.Property<int>("timeLimit");

                    b.Property<DateTime>("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("ServerModelId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Kontur.GameStats.Server.Models.ScoreBoardModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MatcheModelId");

                    b.Property<int>("deaths");

                    b.Property<int>("frags");

                    b.Property<int>("kills");

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.HasIndex("MatcheModelId");

                    b.ToTable("ScoreBoards");
                });

            modelBuilder.Entity("Kontur.GameStats.Server.Models.ServerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("endpoint");

                    b.Property<string>("name");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Kontur.GameStats.Server.Models.GameModeModel", b =>
                {
                    b.HasOne("Kontur.GameStats.Server.Models.ServerModel", "ServerModel")
                        .WithMany("gameModes")
                        .HasForeignKey("ServerModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kontur.GameStats.Server.Models.MatcheModel", b =>
                {
                    b.HasOne("Kontur.GameStats.Server.Models.ServerModel", "ServerModel")
                        .WithMany("matches")
                        .HasForeignKey("ServerModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Kontur.GameStats.Server.Models.ScoreBoardModel", b =>
                {
                    b.HasOne("Kontur.GameStats.Server.Models.MatcheModel", "MatcheModel")
                        .WithMany("scoreboard")
                        .HasForeignKey("MatcheModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

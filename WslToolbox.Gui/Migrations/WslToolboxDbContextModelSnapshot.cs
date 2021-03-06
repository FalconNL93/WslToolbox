// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WslToolbox.Gui;

namespace WslToolbox.Gui.Migrations
{
    [DbContext(typeof(WslToolboxDbContext))]
    partial class WslToolboxDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("WslToolbox.Gui.Distribution", b =>
                {
                    b.Property<int>("DistributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DistributionGuid")
                        .HasColumnType("TEXT");

                    b.HasKey("DistributionId");

                    b.ToTable("Distributions");
                });

            modelBuilder.Entity("WslToolbox.Gui.QuickAction", b =>
                {
                    b.Property<int>("QuickActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DistributionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QuickActionCommand")
                        .HasColumnType("TEXT");

                    b.HasKey("QuickActionId");

                    b.HasIndex("DistributionId");

                    b.ToTable("QuickActions");
                });

            modelBuilder.Entity("WslToolbox.Gui.QuickAction", b =>
                {
                    b.HasOne("WslToolbox.Gui.Distribution", "Distribution")
                        .WithMany("QuickActions")
                        .HasForeignKey("DistributionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distribution");
                });

            modelBuilder.Entity("WslToolbox.Gui.Distribution", b =>
                {
                    b.Navigation("QuickActions");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SOSTransito.Data;

namespace SOSTransito.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SOSTransito.Models.Atribuicao_Localidade", b =>
                {
                    b.Property<int>("ATRLOCId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LocalidadeId")
                        .HasColumnType("int");

                    b.Property<string>("LocalizadorHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusSistema")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("ATRLOCId");

                    b.HasIndex("LocalidadeId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Atribuicao_Localidade");
                });

            modelBuilder.Entity("SOSTransito.Models.CNH", b =>
                {
                    b.Property<int>("CNHId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<string>("LocalizadorHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Processo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegistroCNH")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("StatusCNH")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusSistema")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidadeCNH")
                        .HasColumnType("datetime2");

                    b.HasKey("CNHId");

                    b.HasIndex("ClienteId");

                    b.ToTable("CNH");
                });

            modelBuilder.Entity("SOSTransito.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("LocalidadeId")
                        .HasColumnType("int");

                    b.Property<string>("LocalizadorHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StatusSistema")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("email")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("ClienteId");

                    b.HasIndex("LocalidadeId");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("SOSTransito.Models.Localidade", b =>
                {
                    b.Property<int>("LocalidadeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LocalizadorHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Regiao")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("StatusSistema")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsuarioID")
                        .HasColumnType("int");

                    b.HasKey("LocalidadeId");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Localidade");
                });

            modelBuilder.Entity("SOSTransito.Models.Multa", b =>
                {
                    b.Property<int>("MultaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CNHId")
                        .HasColumnType("int");

                    b.Property<string>("LocalizadorHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrgAtuador")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pontuacao")
                        .HasColumnType("int");

                    b.Property<string>("Processo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusSistema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Veiculo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MultaId");

                    b.HasIndex("CNHId");

                    b.ToTable("Multa");
                });

            modelBuilder.Entity("SOSTransito.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("LocalizadorHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusSistema")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioID");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("SOSTransito.Models.Veiculo", b =>
                {
                    b.Property<int>("VeiculoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<string>("LocalizadorHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("RENAVAN")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("StatusSistema")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VeiculoId");

                    b.HasIndex("ClienteId");

                    b.ToTable("Veiculo");
                });

            modelBuilder.Entity("SOSTransito.Models.Atribuicao_Localidade", b =>
                {
                    b.HasOne("SOSTransito.Models.Localidade", "Localidades")
                        .WithMany("Atribuicao_Localidade")
                        .HasForeignKey("LocalidadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SOSTransito.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Localidades");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SOSTransito.Models.CNH", b =>
                {
                    b.HasOne("SOSTransito.Models.Cliente", "Clientes")
                        .WithMany("CNH")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clientes");
                });

            modelBuilder.Entity("SOSTransito.Models.Cliente", b =>
                {
                    b.HasOne("SOSTransito.Models.Localidade", "Localidades")
                        .WithMany("Clientes")
                        .HasForeignKey("LocalidadeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Localidades");
                });

            modelBuilder.Entity("SOSTransito.Models.Localidade", b =>
                {
                    b.HasOne("SOSTransito.Models.Usuario", null)
                        .WithMany("Localidades")
                        .HasForeignKey("UsuarioID");
                });

            modelBuilder.Entity("SOSTransito.Models.Multa", b =>
                {
                    b.HasOne("SOSTransito.Models.CNH", "CNH")
                        .WithMany("Multas")
                        .HasForeignKey("CNHId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CNH");
                });

            modelBuilder.Entity("SOSTransito.Models.Veiculo", b =>
                {
                    b.HasOne("SOSTransito.Models.Cliente", "Clientes")
                        .WithMany("Veiculos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clientes");
                });

            modelBuilder.Entity("SOSTransito.Models.CNH", b =>
                {
                    b.Navigation("Multas");
                });

            modelBuilder.Entity("SOSTransito.Models.Cliente", b =>
                {
                    b.Navigation("CNH");

                    b.Navigation("Veiculos");
                });

            modelBuilder.Entity("SOSTransito.Models.Localidade", b =>
                {
                    b.Navigation("Atribuicao_Localidade");

                    b.Navigation("Clientes");
                });

            modelBuilder.Entity("SOSTransito.Models.Usuario", b =>
                {
                    b.Navigation("Localidades");
                });
#pragma warning restore 612, 618
        }
    }
}

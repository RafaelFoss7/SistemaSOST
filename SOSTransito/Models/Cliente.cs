﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [StringLength(6, ErrorMessage = "O campo nome precisa ter de 6 a 60 caracteres", MinimumLength = 100)]
        public string Nome { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo CPF precisa ter de 11 caracteres", MinimumLength = 11)]
        public string CPF { get; set; }

        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "O campo data de nascimento é obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "O campo endereço é obrigatório.")]
        [StringLength(6, ErrorMessage = "O campo endereço precisa ter de 6 a 60 caracteres", MinimumLength = 60)]
        public string Endereco { get; set; }

        [DisplayName("Telefone")]
        [Required(ErrorMessage = "O campo telefone é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo telefone precisa ter de 11 caracteres", MinimumLength = 11)]
        public string Telefone { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [StringLength(6, ErrorMessage = "O campo e-mail precisa ter de 6 a 60 caracteres", MinimumLength = 60)]
        public string email { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        [Required]
        public string StatusSistema { get; set; }

        [Required]
        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual Localidade Localidades { get; set; }
        public int LocalidadeId { get; set; }

        public virtual CNH CNH { get; set; }

        public virtual ICollection<Veiculo> Veiculos { get; set; }
    }
}

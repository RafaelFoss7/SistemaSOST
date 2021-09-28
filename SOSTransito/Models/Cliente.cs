using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(100, ErrorMessage = "O campo nome precisa ter de 6 a 60 caracteres", MinimumLength = 6)]
        public string Nome { get; set; }

        [DisplayName("RG")]
        [Required(ErrorMessage = "O campo RG é obrigatório.")]
        [StringLength(12, ErrorMessage = "O campo RG precisa ter 12 caracteres", MinimumLength = 12)]
        public string RG { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [StringLength(14, ErrorMessage = "O campo CPF precisa ter de 11 caracteres", MinimumLength = 14)]
        public string CPF { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "O campo data de nascimento é obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [DisplayName("Endereço")]
        [Required(ErrorMessage = "O campo endereço é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo endereço precisa ter de 6 a 60 caracteres", MinimumLength = 6)]
        public string Endereco { get; set; }

        [DisplayName("Celular")]
        [StringLength(14, ErrorMessage = "O campo telefone precisa ter de 11 caracteres", MinimumLength = 14)] 
        public string Telefone { get; set; }

        [DisplayName("E-mail")]
        [StringLength(60, ErrorMessage = "O campo e-mail precisa ter de 6 a 60 caracteres", MinimumLength = 6)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um e-mail válido...")]
        public string email { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        public string NotificationYear { get; set; }

        //Relacionamentos...
        public virtual Localidade Localidades { get; set; }
        public int LocalidadeId { get; set; }

        public virtual ICollection<Veiculo> Veiculos { get; set; }
        public virtual ICollection<CNH> CNH { get; set; }
    }
}

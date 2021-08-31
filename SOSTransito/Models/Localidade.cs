using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("Localidade")]
    public class Localidade
    {
        //atributos...
        [Key]
        public int LocalidadeId { get; set; }

        [DisplayName("Cidade")]
        [Required(ErrorMessage = "O campo cidade é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo região precisa ter de 3 a 60 caracteres", MinimumLength = 3)]
        public string Regiao { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual ICollection<Atribuicao_Localidade> Atribuicao_Localidade { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

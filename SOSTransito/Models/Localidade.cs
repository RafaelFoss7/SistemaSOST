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
        public int LocalidadeID { get; set; }

        [DisplayName("Região")]
        [Required(ErrorMessage = "O campo região é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo região precisa ter de 3 a 60 caracteres", MinimumLength = 3)]
        public string Regiao { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        [Required]
        public string StatusSistema { get; set; }

        [Required]
        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual Usuario Usuarios { get; set; }
        public int UsuarioId { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

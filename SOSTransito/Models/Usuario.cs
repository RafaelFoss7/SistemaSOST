using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        //atributos...
        [Key]
        public int UsuarioID { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo nome precisa ter de 6 a 100 caracteres", MinimumLength = 6)]
        public string Nome { get; set; }

        [DisplayName("Tipo de Acesso")]
        [Required(ErrorMessage = "O campo tipo de acesso é obrigatório.")]
        public string Tipo { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "O campo email é obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo e-mail precisa ter de 6 a 60 caracteres", MinimumLength = 6)]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        public string Senha { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual ICollection<Localidade> Localidades { get; set; }
    }
}

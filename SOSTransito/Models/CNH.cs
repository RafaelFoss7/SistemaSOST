using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("CNH")]
    public class CNH
    { //Atributos...
        [Key]
        public int CNHId { get; set; }

        [DisplayName("Nº Registro CNH")]
        [Required(ErrorMessage = "O campo Nº registro da CNH é obrigatório.")]
        [StringLength(9, ErrorMessage = "O campo Nº registro da CNH precisa ter de 9 caracteres", MinimumLength = 9)]
        public string RegistroCNH { get; set; }

        [DisplayName("Validade CNH")]
        [Required(ErrorMessage = "O campo Nº registro da CNH é obrigatório.")]
        public DateTime ValidadeCNH { get; set; }

        //Informações de controle...
        [DisplayName("Status CNH")]
        [Required]
        public string StatusCNH { get; set; }

        [DisplayName("Processo CNH")]
        [Required]
        public string Processo { get; set; }

        [DisplayName("Status de Controle")]
        [Required]
        public string StatusSistema { get; set; }

        [Required]
        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual Cliente Clientes { get; set; }
        public int ClienteId { get; set; }

        public virtual ICollection<PAT> PATs { get; set; }
    }
}

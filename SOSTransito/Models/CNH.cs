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
        [StringLength(9, ErrorMessage = "O campo Nº registro da CNH precisa ter 9 caracteres", MinimumLength = 9)]
        public string RegistroCNH { get; set; }

        [DisplayName("Categoria da CNH")]
        [Required(ErrorMessage = "O campo categoria da CNH é obrigatório.")]
        public string Categoria { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Validade CNH")]
        [Required(ErrorMessage = "O campo Nº registro da CNH é obrigatório.")]
        public DateTime ValidadeCNH { get; set; }

        //Informações de controle...
        [DisplayName("Status CNH")]
        [Required]
        public string StatusCNH { get; set; }

        [DisplayName("Processo CNH")]
        public string Processo { get; set; }

        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        public string NotificationYear { get; set; }

        //Relacionamentos...
        public virtual Cliente Clientes { get; set; }
        public int ClienteId { get; set; }

        public virtual ICollection<Multa> Multas { get; set; }
    }
}

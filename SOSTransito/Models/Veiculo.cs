using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key]
        public int VeiculoId { get; set; }

        [DisplayName("Placa")]
        [Required(ErrorMessage = "O campo placa é obrigatório.")]
        [StringLength(7, ErrorMessage = "O campo nome precisa ter de 6 caracteres", MinimumLength = 7)]
        public string Placa { get; set; }

        [DisplayName("RENAVAN")]
        [Required(ErrorMessage = "O campo RENAVAN é obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo RENAVAN precisa ter 11 caracteres", MinimumLength = 11)]
        public string RENAVAN { get; set; }

        //Informações de controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual Cliente Clientes { get; set; }
        public int ClienteId { get; set; }
    }
}

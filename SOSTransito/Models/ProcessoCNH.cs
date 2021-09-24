using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOSTransito.Models
{
    [Table("ProcessoCNH")]
    public class ProcessoCNH
    {
        [Key]
        public int ProcessoCNHId { get; set; }

        [DisplayName("Status CNH")]
        [Required(ErrorMessage = "O campo status é obrigatório.")]
        public string StatusCNH { get; set; }

        [DisplayName("Processo CNH")]
        [Required(ErrorMessage = "O campo processo é obrigatório.")]
        public string Processo { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Prazo do Processo")]
        [Required(ErrorMessage = "O campo prazo é obrigatório.")]
        public DateTime Prazo { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Periodo(De)")]
        public DateTime De { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Periodo(Até)")]
        public DateTime Ate { get; set; }

        //Informações de Controle...
        [DisplayName("Status de Controle")]
        public string StatusSistema { get; set; }

        public string LocalizadorHash { get; set; }

        //Relacionamentos...
        public virtual CNH CNH { get; set; }
        public int CNHId { get; set; }
    }
}

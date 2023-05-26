﻿using DesafioTecnicoArtycs.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnicoArtycs.Domain.Entities
{
    public class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataCadastro { get; set; }
        
    }
}

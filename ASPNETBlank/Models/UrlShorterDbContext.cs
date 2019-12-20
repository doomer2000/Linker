﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Models
{
    public class UrlShorterDbContext : DbContext
    {
        public UrlShorterDbContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<UrlInfo> UrlInfos { get; set; }
    }
}

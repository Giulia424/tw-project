using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Configurations;
using System.IO;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

public interface IGenreRepository
{
  
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct = default);

   
    Task<ICollection<Genre>> GetAllAsync(Expression<Func<Genre, bool>>? filter = null, int? skip = null, int? take = null, 
        ICollection<Expression<Func<Genre, object>>>? includes = null, CancellationToken ct = default);

    Task<int> GetCountAsync(Expression<Func<Genre, bool>>? filter = null, CancellationToken ct = default);

    Task<Genre> AddAsync(Genre entity, CancellationToken ct = default);

  
    Task<Genre> UpdateAsync(Genre entity, CancellationToken ct = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
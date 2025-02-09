using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]

    public class PreferencesController : ControllerBase
    {
        private readonly IGenericRepository<Preference> _preferenceRepository;

        public PreferencesController(IGenericRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список всех предпочтений
        /// </summary>
        /// <returns>Список предпочтений</returns>
        [HttpGet]
        public async Task<ActionResult<IList<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = _preferenceRepository.Get()
                .Select(x => new PreferenceResponse
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
            return Ok(preferences);
        }
    }
}

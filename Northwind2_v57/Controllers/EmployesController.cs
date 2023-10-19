using Microsoft.AspNetCore.Mvc;
using Northwind2_v57.Entities;
using Northwind2_v57.Services;

namespace Northwind2_v57.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class EmployesController : ControllerBase
	{
		private readonly IServiceEmployes _serviceEmp;

		public EmployesController(IServiceEmployes service)
		{
			_serviceEmp = service;
		}

		// GET: api/Employes?rechercheNom=an
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employe>>>GetEmployés([FromQuery] string? rechercheNom)
		{
			List<Employe> employés = await _serviceEmp.ObtenirEmployés(rechercheNom);

			return Ok(employés);
		}

		// GET: api/Employes?dateEmbaucheMax=2013-01-01
		/*[HttpGet]
		public async Task<ActionResult<IEnumerable<Employe>>>
			GetEmployés([FromQuery] string? rechercheNom, [FromQuery] DateTime? dateEmbaucheMax)
		{
			var employés = await _serviceEmp.ObtenirEmployés(rechercheNom, dateEmbaucheMax);

			return Ok(employés);
		}*/

		[HttpGet("{id}")]
		public async Task<ActionResult<Employe>> GetEmployé(int id)
		{
			Employe? employé = await _serviceEmp.ObtenirEmployé(id);

			if (employé == null) return NotFound();

			return Ok(employé);
		}

		[HttpGet("/api/Regions/{id}")]
		public async Task<ActionResult<Region>> GetRégion(int id)
		{
			Region? region = await _serviceEmp.ObtenirRégion(id);

			if (region == null) return NotFound();

			return Ok(region);
		}
	}
}

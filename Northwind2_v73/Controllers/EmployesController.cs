using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind2_v73.Data;
using Northwind2_v73.Entities;
using Northwind2_v73.Services;
using System.Data;

namespace Northwind2_v73.Controllers
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

		#region Requêtes GET

		// GET: api/Employes?rechercheNom=an
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employe>>> GetEmployés([FromQuery] string? rechercheNom)
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
		#endregion

		#region Requêtes POST

		// POST: api/Employes
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Employe>> PostEmployé(Employe emp)
		{
			try
			{
				// Enregistre l’employé dans la base et le récupère avec son Id généré automatiquement
				Employe res = await _serviceEmp.AjouterEmployé(emp);

				// Renvoie une réponse de code 201 avec l'en-tête 
				// "location: <url d'accès à l’employé>" et un corps contenant l’employé
				return CreatedAtAction(nameof(GetEmployé), new { id = res.Id }, res);
			}
			catch (DbUpdateException e)
			{
				return this.CustomResponseForError(e);
			}
		}

		// POST: api/Affectations
		[HttpPost("/api/Affectations")]
		public async Task<ActionResult<Affectation>> PostAffectation([FromForm] Affectation a)
		{
			//Enregistre les données en base
			await _serviceEmp.AjouterAffectation(a);

			// Renvoie une réponse de code 201 avec l'en-tête 
			// "location: <url d'accès à l’employé>" et un corps contenant son affectation
			return CreatedAtAction(nameof(GetEmployé), new { id = a.IdEmploye }, a);
		}

		// POST: api/Employes/formdata
		[HttpPost("formdata")]
		public async Task<ActionResult<Employe>> PostEmployéFormData([FromForm] FormEmploye fe)
		{
			Employe emp = new()
			{
				IdAdresse = fe.IdAdresse,
				IdManager = fe.IdManager,
				Nom = fe.Nom,
				Prenom = fe.Prenom,
				Fonction = fe.Fonction,
				Civilite = fe.Civilite,
				DateNaissance = fe.DateNaissance,
				DateEmbauche = fe.DateEmbauche
			};

			// Récupère les données de l'adresse
			emp.Adresse = new()
			{
				Id = fe.Adresse.Id,
				Rue = fe.Adresse.Rue,
				CodePostal = fe.Adresse.CodePostal,
				Ville = fe.Adresse.Ville,
				Region = fe.Adresse.Region,
				Pays = fe.Adresse.Pays,
				Tel = fe.Adresse.Tel
			};

			// Récupère les données du fichier photo
			if (fe.Photo != null)
			{
				using Stream stream = fe.Photo.OpenReadStream();
				emp.Photo = new byte[fe.Photo.Length];
				await stream.ReadAsync(emp.Photo);
			}

			// Récupère les données du fichier notes
			if (fe.Notes != null)
			{
				using StreamReader reader = new(fe.Notes.OpenReadStream());
				emp.Notes = await reader.ReadToEndAsync();
			}

			Employe res = await _serviceEmp.AjouterEmployé(emp);

			// Renvoie une réponse de code 201 avec l'en-tête 
			// "location: <url d'accès à l’employé>" et un corps contenant l’employé
			return CreatedAtAction(nameof(GetEmployé), new { id = emp.Id }, res);
		}
		#endregion
	}
}

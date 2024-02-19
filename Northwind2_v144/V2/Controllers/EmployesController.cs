using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind2_v144.Controllers;
using Northwind2_v144.Entities;
using Northwind2_v144.Services;

namespace Northwind2_v144.V2.Controllers
{
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(2.0)]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        private readonly IServiceEmployes _serviceEmp;
        private readonly ILogger<EmployesController> _logger;

        public EmployesController(IServiceEmployes service, ILogger<EmployesController> logger)
        {
            _serviceEmp = service;
            _logger = logger;
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
        [Authorize(Policy = "GérerEmployés")]
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
        /// <summary>
        /// Crée un nouvel employé
        /// </summary>
        /// <param name="emp">Employé à créer</param>
        /// <response code="201">Renvoie l'employé créé avec son id auto-généré</response>
        /// <response code="400">L'employé ne peut pas être créé, car les données envoyées sont incorrectes</response>
        /// <response code="403">L'utilisateur ne possède pas la revendication nécessaire pour ajouter un emplyé</response>
        [HttpPost]
        [Authorize(Policy = "GérerEmployés")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
            catch (Exception e)
            {
                return this.CustomResponseForError(e);
                //return this.CustomResponseForError(e, emp, _logger);
            }
        }

        // POST: api/Affectations
        [HttpPost("/api/Affectations")]
        [Authorize(Policy = "GérerEmployés")]
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
        [Authorize(Policy = "GérerEmployés")]
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

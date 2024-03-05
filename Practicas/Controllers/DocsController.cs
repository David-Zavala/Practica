using Microsoft.AspNetCore.Mvc;
using Practicas.DTOs;
using Practicas.Interfaces;

namespace Practicas.Controllers
{
    public class DocsController(IDocsRepository docsRepository) : BaseApiController
    {
        private readonly IDocsRepository _docsRepository = docsRepository;

        [HttpGet]
        public async Task<ActionResult<List<Doc>>> GetDocs()
        {
            return await _docsRepository.GetDocs();
        }

        [HttpPost("docregister")]
        public async Task<ActionResult> RegisterDoc(Doc doc)
        {
            Doc ndoc = await _docsRepository.PostDoc(doc);
            if (ndoc.Id == -1) return BadRequest("Something gone wrong, please try again later");
            return Ok("New Document Registered");
        }
    }
}
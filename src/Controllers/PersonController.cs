using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

using src.Persistence;
using src.Models;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase{

    private DatabaseContext _context { get; set; }

    public PersonController(DatabaseContext context)
    {
        this._context = context;
    }

[HttpGet]
    public ActionResult<List<Pessoa>> Get(){
        //Pessoa pessoa = new Pessoa("lucas", 25,"12345678"); 
        //Contrato novoContrato = new Contrato("abc123", 50.46);

        //pessoa.contratos.Add(novoContrato);
        var result = _context.Pessoas.Include( p => p.contratos).ToList();

        if(!result.Any()){
            return NoContent();
        }

        return Ok(result);
        
        
        
    }

    [HttpPost]

    public ActionResult<Pessoa> Post([FromBody]Pessoa pessoa){

        _context.Pessoas.Add(pessoa);
        _context.SaveChanges();

        return Created("criado", pessoa);
    }

    [HttpPut("{id}")]
    public ActionResult<Object> Update([FromRoute]int id, [FromBody]Pessoa pessoa){
        //Console.WriteLine(id);
        //Console.WriteLine(pessoa);
    try
    {
        _context.Pessoas.Update(pessoa);
        _context.SaveChanges();
    } 
    catch (System.Exception)
    {   
        return BadRequest(new {
            msg = "Houve erro " + id + " atualizados",
            status = HttpStatusCode.OK
        });
    }  
        return Ok(new {
            msg = "Dados do id " + id + " atualizados",
            status = HttpStatusCode.OK
        });
    }

    [HttpDelete("{id}")]
    public ActionResult<Object> Delete([FromRoute] int id){
        var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

        if (result is null){
            return BadRequest(new {
                msg = "Conteudo inexistente",
                status = HttpStatusCode.BadRequest
            });
        }

        _context.Pessoas.Remove(result);
        _context.SaveChanges();

        return Ok(new {
            msg = "deletado pessoa de Id " + id,
            status = 200
            });

    }

    //public IActionResult Delete([FromRoute] int id);
}
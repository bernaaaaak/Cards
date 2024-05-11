using Cards.Data;
using Cards.Models.Domain;
using Cards.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class CostsDataController : ControllerBase
    {
        private readonly CardsDBContext dbContext;

        public CostsDataController(CardsDBContext dBContext) //ctor
        {
            this.dbContext = dBContext;
        }

        //GET ALL NUMERICS
        //GET: https://localhost:portnumber/api/numerics
        [HttpGet] //attribute
        public IActionResult GetAll() 
        {
            //Get Data From Database = Domain Models
            var costsDomain = dbContext.CostsData.ToList();

            //Map Domain Models to DTOs
            var costsDto = new List<CostsRequestDto>();
            foreach (var costDomain in costsDomain)
            {
                costsDto.Add(new CostsRequestDto(){
                    Price = costDomain.Price,
                    Name = costDomain.Name
                });
            }


            //Return DTOs
            return Ok(costsDto);
        }

        //GET SINGLE NUMERIC DATA (Get region by İD)
        //GET: https://localhost:portnumber/api/numerics/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById([FromRoute] int id) {

            // Get Numerics Domain Model From Database
            var costDomain =dbContext.CostsData.FirstOrDefault(x => x.Id == id);

           if(costDomain == null)
           {
               return NotFound();
           }

            //Return DTO back to client
            return Ok(costDomain);
        }


        //POST To Create New Numeric
        //POST :https://localhost:portnumber/api/numericData

        [HttpPost]
        public IActionResult Create([FromBody] CostsRequestDto costsRequestDto)
        {
            //map or convert DTO to domain model
            var costDomainModel = new CostsData
            {
                Price = costsRequestDto.Price,
                Name = costsRequestDto.Name
            };

            //use domain model to create numericData
            dbContext.CostsData.Add(costDomainModel); //Entity will do it
            dbContext.SaveChanges();

            //map domain model to create numericData
            var costDto = new CostsData
            {
                Id = costDomainModel.Id,
                Price = costDomainModel.Price,
                Name = costDomainModel.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = costDto.Id }, costDto);
            
        }

        //UPDATE SINGLE NUMERIC DATA (Get region by ID)
        //PUT: https://localhost:portnumber/api/numerics/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CostsRequestDto costsRequestDto)
        {
            // Retrieve the entity from the database
            var costDomainModel = dbContext.CostsData.FirstOrDefault(x => x.Id == id);

            // Check if the entity exists
            if (costDomainModel == null)
            {
                return NotFound();
            }

            // Update the entity properties
            costDomainModel.Price = costsRequestDto.Price;
            costDomainModel.Name = costsRequestDto.Name;

            // Save the changes to the database
            dbContext.SaveChanges();

            // Map the updated entity back to DTO
            var costsDto = new CostsData
            {
                Id = costDomainModel.Id,
                Price = costDomainModel.Price,
                Name = costDomainModel.Name
            };

            // Return the updated entity with a 200 OK status
            return Ok(costsDto);
        }

        //Delete: https://localhost:portnumber/api/numerics/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Retrieve the entity from the database
            var costDomainModel = dbContext.CostsData.FirstOrDefault(x => x.Id == id);

            // Check if the entity exists
            if (costDomainModel == null)
            {
                return NotFound(); 
            }

            // Remove the entity from the database
            dbContext.CostsData.Remove(costDomainModel);
            dbContext.SaveChanges();

            // Map the deleted entity to DTO
            var deletedCostDto = new CostsData
            {
                Id = costDomainModel.Id,
                Price = costDomainModel.Price,
                Name = costDomainModel.Name
            };

            // Return the deleted entity with a 200 OK status
            return Ok(deletedCostDto);
        }




    }
}

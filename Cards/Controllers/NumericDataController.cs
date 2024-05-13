using Cards.Data;
using Cards.Models.Domain;
using Cards.Models.DTOs;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class NumericDataController : ControllerBase
    {
        private readonly CardsDBContext dbContext;
        private readonly INumericDataRepository numericDataRepository;

        public NumericDataController(CardsDBContext dBContext, INumericDataRepository numericDataRepository) //ctor
        {
            this.dbContext = dBContext;
            this.numericDataRepository = numericDataRepository;
        }

        //GET ALL NUMERICS
        //GET: https://localhost:portnumber/api/numerics
        [HttpGet] //attribute
        public async Task<IActionResult> GetAll() 
        {

            //var numerics = new List<NumericData> //name of the model
            //{
            //    new NumericData
            //    {
            //        Id = int.MaxValue,
            //        Value = int.MaxValue

            //    },
            //     new NumericData
            //    {
            //        Id = int.MaxValue,
            //        Value = int.MaxValue

            //    }
            //};
             

            //Get Data From Database = Domain Models
            //var numericsDomain = await dbContext.NumericData.ToListAsync();
            var numericsDomain = await numericDataRepository.GetAllAsync(); // now controller calls the repository and repository is responsible from connection with database

            //Map Domain Models to DTOs
            var numericsDto = new List<NumericsDTO>();
            foreach (var numericDomain in numericsDomain) //auto mapper
            {
                numericsDto.Add(new NumericsDTO(){
                    Id = numericDomain.Id,
                    Value = numericDomain.Value,
                    Name = numericDomain.Name
                });
            }


            //Return DTOs
            return Ok(numericsDto);
        }

        //GET SINGLE NUMERIC DATA (Get region by İD)
        //GET: https://localhost:portnumber/api/numerics/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id) {

            //// var numerics =dbContext.NumericData.Find(id);

            //var numerics =dbContext.NumericData.FirstOrDefault(x => x.Id == id);

            //if(numerics == null)
            //{
            //    return NotFound();
            //}

            //return Ok(numerics);



            // Get Numerics Domain Model From Database
            var numericsDto = await numericDataRepository.GetByIdAsync(id);

           if (numericsDto == null)
           {
               return NotFound();
           }

           // no need to iterate one record
            ////Map/Convert Numerics Domain Models to Numerics DTOs
            //var numerics = new List<NumericsDTO>();
            //foreach (var numericDomain in numerics)
            //{
            //    numerics.Add(new NumericsDTO()
            //    {
            //        Id = numericDomain.Id,
            //        Value = numericDomain.Value,
            //         Name = numericDomain.Name
            //    });
            //}


            //Return DTO back to client
            return Ok(numericsDto);
        }

        //POST To Create New Numeric
        //POST :https://localhost:portnumber/api/numericData
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddNumericRequestDto addNumericRequestDto)
        {
            //map or convert DTO to domain model
            var numericDomainModel = new NumericData
            {
                Value = addNumericRequestDto.Value,
                Name = addNumericRequestDto.Name
            };

            //use domain model to create numericData
            //await dbContext.NumericData.AddAsync(numericDomainModel); //Entity will do it
            //await dbContext.SaveChangesAsync();
            numericDomainModel = await numericDataRepository.CreateAsync(numericDomainModel);

            //map domain model to create numericData
            var numericDto = new NumericData
            {
                Id = numericDomainModel.Id,
                Value = numericDomainModel.Value,
                Name = numericDomainModel.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = numericDto.Id }, numericDto);
            
        }

        //UPDATE SINGLE NUMERIC DATA (Get region by ID)
        //PUT: https://localhost:portnumber/api/numerics/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNumericRequestDto updateNumericRequestDto)
        {
            //Map DTO to Domain Model
            var numericDomainModel = new NumericData
            {
                Name = updateNumericRequestDto.Name,
                Value = updateNumericRequestDto.Value,
            };

            // Retrieve the entity from the database
            numericDomainModel = await numericDataRepository.UpdateAsync(id, numericDomainModel);

            // Check if the entity exists
            if (numericDomainModel == null)
            {
                return NotFound();
            }

            //no need since repository
            //// Update the entity properties
            //numericDomainModel.Value = updateNumericRequestDto.Value;
            //numericDomainModel.Name = updateNumericRequestDto.Name;

            //// Save the changes to the database
            //await dbContext.SaveChangesAsync();

            // Map the updated entity back to DTO
            var updatedNumericDto = new NumericData
            {
                Id = numericDomainModel.Id,
                Value = numericDomainModel.Value,
                Name = numericDomainModel.Name
            };

            // Return the updated entity with a 200 OK status
            return Ok(updatedNumericDto);
        }

        //DELETE NUMERIC DATA 
        //Delete: https://localhost:portnumber/api/numerics/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the entity from the database
            var numericDomainModel = await numericDataRepository.DeleteAsync(id);

            // Check if the entity exists
            if (numericDomainModel == null)
            {
                return NotFound(); 
            }

            // Remove the entity from the database
            dbContext.NumericData.Remove(numericDomainModel);
            await dbContext.SaveChangesAsync();

            // Map the deleted entity to DTO
            var deletedNumericDto = new NumericData
            {
                Id = numericDomainModel.Id,
                Value = numericDomainModel.Value,
                Name = numericDomainModel.Name
            };

            // Return the deleted entity with a 200 OK status
            return Ok(deletedNumericDto);
        }





        //SUBTRACTION : https://localhost:portnumber/api/numerics/{id}
        [HttpPost("charge/{id}")]
        public async Task<IActionResult> Cost(int id, [FromBody] CostsRequestDto costRequestDto)
        {
            // Retrieve the item from the database
            var costData = await dbContext.CostsData.FirstOrDefaultAsync(x => x.Name == costRequestDto.Name); // Vehicle type and related fare query
            var numericDataItem = await dbContext.NumericData.FirstOrDefaultAsync(x => x.Id == id); // Client Query

            // Check if the item exists cc
            if (costData == null || numericDataItem == null) 
            {
                return NotFound();
            }


            if (numericDataItem.Value < costData.Price)
            {
                return BadRequest("Cant be charged!");
            }

            // Subtract the subtraction value from the item's value
            numericDataItem.Value = numericDataItem.Value - costData.Price;

            // Save the changes to the database
            await dbContext.SaveChangesAsync();


            // Map the updated value to a DTO
            var updatedValueDto = new NumericData
            {
                Id = numericDataItem.Id,
                Value = numericDataItem.Value,
                Name = numericDataItem.Name
            };

            // Return the updated value with a 200 OK status
            return CreatedAtAction(nameof(GetById), new { id = updatedValueDto.Id }, updatedValueDto);
        }


        //PUT: https://localhost:portnumber/api/numerics/{id}
        [HttpPost("addMoney/{id}")]
        public async Task<IActionResult> AddMoney(int id, [FromBody] AddMoneyRequestDto addMoneyRequestDto)
        {
            // Retrieve the entity from the database
            var numericDomainModel =await dbContext.NumericData.FirstOrDefaultAsync(x => x.Id == id);

            // Check if the entity exists
            if (numericDomainModel == null)
            {
                return NotFound();
            }

            // Update the entity properties
            numericDomainModel.Value = addMoneyRequestDto.Value + numericDomainModel.Value;

            // Save the changes to the database
            await dbContext.SaveChangesAsync();

            // Map the updated entity back to DTO
            var updatedNumericDto = new NumericData
            {
                Id = numericDomainModel.Id,
                Value = numericDomainModel.Value,
                Name = numericDomainModel.Name
            };

            // Return the updated entity with a 200 OK status
            return Ok(updatedNumericDto);
        }

    }
}

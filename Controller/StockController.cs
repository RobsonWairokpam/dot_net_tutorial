using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        // public IActionResult GetAll()
        public async Task<IActionResult> GetAll()
        {
            // var stocks = _context.Stock.ToList()
            // .Select(s => s.ToStockDtos());
            // var stocks = await _context.Stock.ToListAsync();
            var stocks = await _stockRepo.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDtos());

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        // public IActionResult GetById([FromRoute] int id)

        {
            var stock = await _stockRepo.GetByIdAsync(id);
            // var stock = await _context.Stock.FindAsync(id);
            // var stock = _context.Stock.Find(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDtos());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto)
        // public IActionResult Create([FromBody] CreateStockDto stockDto)
        {
            var stockModel = stockDto.ToStockFrpomCreateDTO();
            // _context.Stock.Add(stockModel);
            // _context.SaveChanges();
            // await _context.Stock.AddAsync(stockModel);
            // await _context.SaveChangesAsync();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(
                nameof(GetById),
                new { id = stockModel.Id },
                stockModel.ToStockDtos()
            );
        }

        [HttpPut]
        [Route("{id}")]
        // public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockDto updatestockDto)
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateStockDto updatestockDto
        )
        {
            // var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);
            // var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepo.UpdateAsync(id, updatestockDto);
            if (stockModel == null)
            {
                return NotFound();
            }

            // if (
            //     !string.IsNullOrWhiteSpace(updatestockDto.Symbol)
            //     && updatestockDto.Symbol != "string"
            // )
            // {
            //     stockModel.Symbol = updatestockDto.Symbol;
            // }
            // if (
            //     !string.IsNullOrWhiteSpace(updatestockDto.CompanyName)
            //     && updatestockDto.CompanyName != "string"
            // )
            // {
            //     stockModel.CompanyName = updatestockDto.CompanyName;
            // }
            // if (updatestockDto.Purchase != 0)
            // {
            //     stockModel.Purchase = updatestockDto.Purchase;
            // }
            // if (updatestockDto.LastDiv != 0)
            // {
            //     stockModel.LastDiv = updatestockDto.LastDiv;
            // }
            // if (
            //     !string.IsNullOrWhiteSpace(updatestockDto.Industry)
            //     && updatestockDto.Industry != "string"
            // )
            // {
            //     stockModel.Industry = updatestockDto.Industry;
            // }
            // if (updatestockDto.MarketCap != 0)
            // {
            //     stockModel.MarketCap = updatestockDto.MarketCap;
            // }

            // _context.SaveChanges();
            // await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDtos());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            var stockModel = await _stockRepo.DeleteAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
            // _context.Stock.Remove(stockModel);
            // await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

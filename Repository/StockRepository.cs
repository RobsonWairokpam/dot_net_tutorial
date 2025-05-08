using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            // return await _context.Stock.ToListAsync();
            // return await _context.Stock.Include(c => c.Comment).ToListAsync();
            var stocks = _context.Stock.Include(c => c.Comment).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending
                        ? stocks.OrderByDescending(s => s.Symbol)
                        : stocks.OrderBy(s => s.Symbol);
                }
            }
            return await stocks.ToListAsync();
        }

        public async Task<bool> IsExistStock(int id)
        {
            return await _context.Stock.AnyAsync(c => c.Id == id);
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            // var stockModels = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            var stockModels = await _context
                .Stock.Include(c => c.Comment)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (stockModels == null)
            {
                return null;
            }
            return stockModels;

            // throw new NotImplementedException();
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(stockDto.Symbol) && stockDto.Symbol != "string")
            {
                stockModel.Symbol = stockDto.Symbol;
            }
            if (
                !string.IsNullOrWhiteSpace(stockDto.CompanyName)
                && stockDto.CompanyName != "string"
            )
            {
                stockModel.CompanyName = stockDto.CompanyName;
            }
            if (stockDto.Purchase != 0)
            {
                stockModel.Purchase = stockDto.Purchase;
            }
            if (stockDto.LastDiv != 0)
            {
                stockModel.LastDiv = stockDto.LastDiv;
            }
            if (!string.IsNullOrWhiteSpace(stockDto.Industry) && stockDto.Industry != "string")
            {
                stockModel.Industry = stockDto.Industry;
            }
            if (stockDto.MarketCap != 0)
            {
                stockModel.MarketCap = stockDto.MarketCap;
            }

            // _context.SaveChanges();
            await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}

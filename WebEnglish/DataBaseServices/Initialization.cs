using ContextDataBase;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataBaseServices
{
    public class Initialization
    {
        private readonly  AppDbContext _db;
        private ILogger<Initialization> _Logger;
        public Initialization
            (AppDbContext db, ILogger<Initialization> Logger)
        {
            _db = db; _Logger = Logger;
        }

        public async Task<bool> InitializeAsync(CancellationToken token = default)
        {
            Stopwatch timer = Stopwatch.StartNew();
            try
            {
                await _db.Database.EnsureCreatedAsync(token);
                _Logger.LogInformation($"БД создана за {timer.Elapsed.TotalSeconds}");


                if (!await _db.Words.AnyAsync(cancellationToken: token).ConfigureAwait(false))
                {
                    await _db.Words.AddRangeAsync(new List<Word>()
                    {
                       new() {EngWord = "Freezing", RusWord = "Обледенение"},
                       new() {EngWord = "Bracing", RusWord = "Укрепление"},
                       new() {EngWord = "Chilly", RusWord = "Прохладно"},
                       new() {EngWord = "Cool", RusWord = "Холод"},
                       new() {EngWord = "Mild", RusWord = "Мягкий"},
                       new() {EngWord = "Warm", RusWord = "Теплый"},
                       new() {EngWord = "Balmy", RusWord = "Нежный"},
                       new() {EngWord = "Hot", RusWord = "Горячий"},
                       new() {EngWord = "Sweltering", RusWord = "Изнуряющий"},
                       new() {EngWord = "Freezing", RusWord = "Обледенение"},
                       new() {EngWord = "Scorching", RusWord = "Палящий"},
                    }, token).ConfigureAwait(false);
                    await _db.SaveChangesAsync(token).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _Logger.LogInformation($"БД НЕ Создана");
                _Logger.LogInformation($"Ошибка типа {e}. Контекст ошибки {e.Message}");
                return false;
            }
            finally
            {
                timer.Stop();
            }
            return true;
        }
    }
}

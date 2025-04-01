using Microsoft.AspNetCore.Mvc;
using SkepsBeholder.Infra;
using SkepsBeholder.Model;
using SkepsBeholder.Model.Mongo;
using SkepsBeholder.Services.Interfaces;
using SkepsBeholder.Services.Mongo;
using System.Text.Json;

namespace SkepsBeholder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;
        private readonly IProcess _process;
        private readonly IMongoService _mongoService;
        private readonly IBlipSender _blipSender;
        public LogController(ILogger<LogController> logger, IProcess process, IMongoService mongoService, IBlipSender blipSender)
        {
            _logger = logger;
            _process = process;
            _mongoService = mongoService;
            _blipSender = blipSender;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody]Log log)
        {
            //_logger.LogInformation(log.Owner.Name);
            _ = _blipSender.SendContactAsync("estacioprd1", JsonSerializer.Serialize(log)).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    _logger.LogError(t.Exception, "Erro ao enviar contato.");
                }
            }, TaskContinuationOptions.OnlyOnFaulted);

            if (log.Error != null)
            {
                _ = _process.ProcessLogAsync(log).ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        _logger.LogError(t.Exception, "Erro ao processar log.");
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
            }

            return Ok();
        }

        [HttpPost("test")]
        public async Task<IActionResult> CriarRoteadorInfo([FromBody]RoteadorConfigMongo roteadorConfig)
        {

            await _mongoService.AddRoteadorConfigAsync(roteadorConfig);
            return Ok();
        }
    }
}
using SkepsBeholder.Model;
using SkepsBeholder.Model.Mongo;
using SkepsBeholder.Services.Email;
using SkepsBeholder.Services.Mongo;
using System.Text.Json;

namespace SkepsBeholder.Services
{
    public class Process : Interfaces.IProcess
    {
        private readonly IEmail _email;
        private readonly IMongoService _mongoService;

        public Process(IEmail email, IMongoService mongoService)
        {
            _email = email;
            _mongoService = mongoService;
        }

        public async Task ProcessLogAsync(Log log)
        {
            var roteadorConfig = await _mongoService.GetRoteadorConfigAsync(log.Owner.Name);

            if (roteadorConfig == null)
            {
                Console.WriteLine($"Configuração do roteador {log.Owner.Name} não encontrada.");
                return;
            }

            var statesError = log.States.Where(log => log.Error != null).ToList();

            foreach(var action in log.InputActions.Where(i => i.Error != null))
            {
                var message = new
                {
                    Bot = log.Owner.Name,
                    Usuario = log.User,
                    Type = action.Type.ToString(),
                };
                var actionErrorReport = await _mongoService.GetActionErroyByKeyAsync($"{log.Owner.Name}-{action.Type.ToString()}");
                if (actionErrorReport is null)
                {
                    var emails = roteadorConfig.EmailResponsavel.Split(";");
                    await EnviarEmail(emails.ToList(), JsonSerializer.Serialize(message));

                    await _mongoService.InsertActionErrorAsync(new ActionErrorMongo
                    {
                        ExpireAt = DateTime.UtcNow.AddMinutes(60),
                        Key = $"{log.Owner.Name}-{action.Type.ToString()}",
                    });
                }
            }

            foreach(var action in log.OutputActions.Where(i => i.Error != null))
            {
                var message = new
                {
                    Bot = log.Owner.Name,
                    Usuario = log.User,
                    Type = action.Type.ToString(),
                };
                var actionErrorReport = await _mongoService.GetActionErroyByKeyAsync($"{log.Owner.Name}-{action.Type.ToString()}");
                if (actionErrorReport is null)
                {
                    var emails = roteadorConfig.EmailResponsavel.Split(";");
                    await EnviarEmail(emails.ToList(), JsonSerializer.Serialize(message));

                    await _mongoService.InsertActionErrorAsync(new ActionErrorMongo
                    {
                        ExpireAt = DateTime.UtcNow.AddMinutes(60),
                        Key = $"{log.Owner.Name}-{action.Type.ToString()}",
                    });
                }
            }

            foreach (var state in statesError)
            {
                foreach (var action in state.InputActions.Where(i => i.Error != null))
                {
                    var message = new
                    {
                        Bot = log.Owner.Name,
                        Bloco = state.ExtensionData.Name,
                        Usuario = log.User,
                        Type = action.Type.ToString(),
                    };

                    var actionErrorReport = await _mongoService.GetActionErroyByKeyAsync($"{log.Owner.Name}-{state.ExtensionData.Name}-{action.Type.ToString()}");
                   
                    if(actionErrorReport is null)
                    {
                        var emails = roteadorConfig.EmailResponsavel.Split(";");
                        await EnviarEmail(emails.ToList(), JsonSerializer.Serialize(message));
                        
                        await _mongoService.InsertActionErrorAsync(new ActionErrorMongo
                        {
                            ExpireAt = DateTime.UtcNow.AddMinutes(60),
                            Key = $"{log.Owner.Name}-{state.ExtensionData.Name}-{action.Type.ToString()}",
                        });
                    }
                }

                foreach(var action in state.OutputActions.Where(i => i.Error != null))
                {
                    var message = new
                    {
                        Bot = log.Owner.Name,
                        Bloco = state.ExtensionData.Name,
                        Usuario = log.User,
                        Type = action.Type.ToString(),
                    };

                    var actionErrorReport = await _mongoService.GetActionErroyByKeyAsync($"{log.Owner.Name}-{state.ExtensionData.Name}-{action.Type.ToString()}");

                    if (actionErrorReport is null)
                    {
                        var emails = roteadorConfig.EmailResponsavel.Split(";");
                        await EnviarEmail(emails.ToList(), JsonSerializer.Serialize(message));
                        
                        await _mongoService.InsertActionErrorAsync(new ActionErrorMongo
                        {
                            ExpireAt = DateTime.UtcNow.AddMinutes(60),
                            Key = $"{log.Owner.Name}-{state.ExtensionData.Name}-{action.Type.ToString()}",
                        });
                    }
                }
            }
        }

        private async Task EnviarEmail(List<string> email, string message)
        {
            foreach (var e in email)
            {
                await _email.SendMessageAsync(e, message);
            }

            return;
        }
    }
}

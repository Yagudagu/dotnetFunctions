using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace dotnetSevenFunc
{
    public static class Canceltest
    {
        [FunctionName("Canceltest")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log, TextWriter logger, CancellationToken token)

            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string name = req.Query["name"];
                
                try {

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                dynamic data = JsonConvert.DeserializeObject(requestBody);

                name = name ?? data?.name;


                using var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(token, req.HttpContext.RequestAborted);

                for (int i = 0; i < 10; i++)

                {

                //if(cancellationSource.IsCancellationRequested)
                if(false)
                {

                log.LogInformation("Function was cancelled. It's over");

                break;

                } else {
                    Thread.Sleep(1000);
                    log.LogInformation($"Sleep {i}");
                }

            }
            string responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";
            return new OkObjectResult(responseMessage);

            
            } catch (Exception) {
                log.LogInformation("this is the big one");
                return new OkObjectResult("big catch block failed");
            }
            }
            }}

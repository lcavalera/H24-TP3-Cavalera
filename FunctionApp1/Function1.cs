using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class Function1
    {

            private readonly ILogger<Function1> _logger;

            public Function1(ILogger<Function1> logger)
            {
                _logger = logger;
            }

            [Function("ajoutdocuments")]
            [BlobOutput("%blobout%", Connection = "AzureWebJobsStorage")]
            public byte[] Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, out string blobout)
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                var content = new StreamReader(req.Body).ReadToEnd();
                Fichier? document = JsonConvert.DeserializeObject<Fichier>(content);

                int randomNum = new Random().Next(100, 999);
                string fichierNom = $"{document.IdUtilisateur}_{document.Type}_{randomNum}{document.Extension}";
                //string contentType = "application/" + document.Extension.Substring(document.Extension.IndexOf(".") + 1);

                //var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };
                blobout = fichierNom;
                _logger.LogInformation($"Fichier '{fichierNom}' uploading...");

                byte[] buffer = Convert.FromBase64String(document.DocumentConverti);
                using (var stream = new MemoryStream(buffer))
                {
                    BinaryData binaryData = new BinaryData(buffer);

                    //await new BlobServiceClient(
                    //    Environment.GetEnvironmentVariable("ServiceBlobConnection"))
                    //    .GetBlobContainerClient("documents")
                    //    .GetBlobClient(fichierNom)
                    //    .UploadAsync(binaryData, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

                    _logger.LogInformation($"Fichier '{fichierNom}' saved !");
                }
                return buffer;
            }
    }
}

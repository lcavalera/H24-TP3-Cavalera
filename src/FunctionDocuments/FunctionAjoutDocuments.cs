using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace FunctionDocuments
{
    public class FunctionAjoutDocuments
    {
        private readonly ILogger<FunctionAjoutDocuments> _logger;

        public FunctionAjoutDocuments(ILogger<FunctionAjoutDocuments> logger)
        {
            _logger = logger;
        }

        [Function("ajoutdocuments")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();
            Fichier? document = JsonConvert.DeserializeObject<Fichier>(content);

            int randomNum = new Random().Next(100, 999);
            string fichierNom = $"{document.IdUtilisateur}_{document.Type}_{randomNum}{document.Extension}";
            string contentType = "application/" + document.Extension.Substring(document.Extension.IndexOf(".") + 1);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            _logger.LogInformation($"Fichier '{fichierNom}' uploading...");
            
            byte[] buffer = Convert.FromBase64String(document.DocumentConverti);
            using (var stream = new MemoryStream(buffer))
            {
                BinaryData binaryData = new BinaryData(buffer);

                await new BlobServiceClient(
                    Environment.GetEnvironmentVariable("ServiceBlobConnection"))
                    .GetBlobContainerClient("documents")
                    .GetBlobClient(fichierNom)
                    .UploadAsync(binaryData, new BlobUploadOptions { HttpHeaders = blobHttpHeader });

                _logger.LogInformation($"Fichier '{fichierNom}' saved !");
            }
        }
    }
}

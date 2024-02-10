using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using ModernRecrut.Documents.ApplicationCore.Entites;
using ModernRecrut.Documents.ApplicationCore.Interfaces;

namespace ModernRecrut.Documents.ApplicationCore.Services
{
    public class StorageServiceHelper: IStorageServiceHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        private readonly IConfiguration _configuration;

        public StorageServiceHelper (BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _containerClient = _blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("NomConteneur"));
        }

        public async Task EnregistrerDocument(Fichier document)
        {
            int randomNum = new Random().Next(100, 999);
            string fichierNom = $"{document.IdUtilisateur}_{document.Type}_{randomNum}{document.Extension}";
            string contentType = "application/" + document.Extension.Substring(document.Extension.IndexOf(".")+1);


            BlobClient blobClient = _containerClient.GetBlobClient(fichierNom);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            byte[] buffer = Convert.FromBase64String(document.DocumentConverti);
            using (var stream = new MemoryStream(buffer))
            {
                BinaryData binaryData = new BinaryData(buffer);
                await blobClient.UploadAsync(binaryData, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
            }
        }

        public async Task<List<string>> ObtenirDocumentsDansConteneur(string idUtilisateur)
        {
            List<string> listDocuments = new List<string>();

            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                if (blobItem.Name.StartsWith($"{idUtilisateur}_"))
                {
                    //var extension = blobItem.Name.Substring()
                    //blobItem.Properties.ContentType = "application/json";
                    listDocuments.Add(blobItem.Name);
                }
            }
            return listDocuments;
        }

        public async Task Supprimer(string nomDocument)
        {
            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                if(blobItem.Name == nomDocument)
                {
                    await _containerClient.DeleteBlobAsync(nomDocument);
                }
            }
        }
    }
}

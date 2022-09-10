using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace AzureStorageSDKTraining.AZ204;

public interface IStorageClient
{
    Stream ReadFileFromFileSystem(string filePath);
    
    Task SaveFileAsync(Stream stream, string contentType, string uniqueFileName, string containerName);

    Task<List<string>> ListBlobsInsideContainerAsync(string containerName);

    Task<Stream> DownloadToStreamAsync(string containerName, string uniqueFileName);

    Task UpdateBlobMetadataAsync(BlobClient blob, IDictionary<string, string> metadata);

    Task<IDictionary<string, string>> ReadBlobMetadataASync(string containerName, string fileName);

    void PrintDictionaryContents(IDictionary<string, string> dictionary);

    void PrintAccountDetails();
}
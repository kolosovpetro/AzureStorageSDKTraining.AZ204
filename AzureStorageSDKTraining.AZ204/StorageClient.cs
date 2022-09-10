using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;

namespace AzureStorageSDKTraining.AZ204;

public class StorageClient : IStorageClient
{
    private const string BlobServiceEndpoint = "<primary-blob-service-endpoint>";
    private const string StorageAccountName = "<storage-account-name>";
    private const string StorageAccountKey = "<key>";

    private readonly BlobServiceClient _blobServiceClient;

    public StorageClient()
    {
        var credentials = new StorageSharedKeyCredential(StorageAccountName, StorageAccountKey);
        var uri = new Uri(BlobServiceEndpoint);

        _blobServiceClient = new BlobServiceClient(uri, credentials);
    }
    
    public Task SaveFileAsync(Stream stream, string contentType, string uniqueFileName, string containerName)
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> ListBlobsInsideContainerAsync(string containerName)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> DownloadToStreamAsync(string uniqueName)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBlobMetadataAsync(BlobClient blob, IDictionary<string, string> metadata)
    {
        throw new NotImplementedException();
    }

    public Task<IDictionary<string, string>> ReadBlobMetadataASync(string containerName, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<IDictionary<string, string>> ReadBlobMetadataASync(BlobClient blob)
    {
        throw new NotImplementedException();
    }

    public void PrintDictionaryContents(IDictionary<string, string> dictionary)
    {
        throw new NotImplementedException();
    }

    public void PrintAccountDetails()
    {
        throw new NotImplementedException();
    }
}
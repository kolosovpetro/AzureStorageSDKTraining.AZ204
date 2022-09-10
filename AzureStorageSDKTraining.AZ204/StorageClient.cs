using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

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

    public Stream ReadFileFromFileSystem(string filePath)
    {
        var stream = File.OpenRead(filePath);

        return stream;
    }

    public Task SaveFileAsync(Stream stream, string contentType, string uniqueFileName, string containerName)
    {
        var containerClient = GetContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(uniqueFileName);
        var headers = new BlobHttpHeaders { ContentType = contentType };
        var result = blobClient.UploadAsync(stream, headers);

        return result;
    }

    private BlobContainerClient GetContainerClient(string blobContainerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);
        containerClient.CreateIfNotExists();
        return containerClient;
    }

    public async Task<List<string>> ListBlobsInsideContainerAsync(string containerName)
    {
        var containerClient = GetContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var allBlobs = containerClient.GetBlobsAsync();

        var list = new List<string>();

        await foreach (var blob in allBlobs)
        {
            list.Add(blob.Name);
        }

        return list;
    }

    public async Task<Stream> DownloadToStreamAsync(string containerName, string uniqueFileName)
    {
        var containerClient = GetContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(uniqueFileName);
        var downloadInfo = await blobClient.DownloadAsync();
        var result = downloadInfo.Value.Content;

        return result;
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
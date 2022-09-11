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
    private const string BlobServiceEndpoint = "https://storagepkolosov.blob.core.windows.net/";

    private const string StorageAccountName = "storagepkolosov";

    private const string StorageAccountKey =
        "wBx7rG9hD9KufrA2ojLVYcqCTlI0LJKPp++iSYXYSPwkkCK15KnZ73Yu1//kPyNebkPJyqbbgJo5+AStOXvy4Q==";

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

    public BlobContainerClient GetContainerClient(string blobContainerName)
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

    public async Task UpdateBlobMetadataAsync(
        string containerName,
        string fileName,
        IDictionary<string, string> metadata)
    {
        var container = GetContainerClient(containerName);

        var blob = container.GetBlobClient(fileName);

        await blob.SetMetadataAsync(metadata);
    }

    public IDictionary<string, string> ReadBlobMetadataASync(string containerName, string fileName)
    {
        var container = GetContainerClient(containerName);

        var blob = container.GetBlobClient(fileName);

        var props = blob.GetProperties();

        var metaData = props.Value.Metadata;

        return metaData;
    }

    public void PrintDictionaryContents(IDictionary<string, string> dictionary)
    {
        Console.WriteLine("Dictionary contents: ");

        foreach (var pair in dictionary)
        {
            Console.WriteLine($"  -- Key: {pair.Key}, Value: {pair.Value}");
        }
    }

    public async Task PrintAccountDetails()
    {
        var accountInfo = await _blobServiceClient.GetAccountInfoAsync();

        await Console.Out.WriteLineAsync($"Connected to Azure Storage Account");
        await Console.Out.WriteLineAsync($"Account name:\t{StorageAccountName}");
        await Console.Out.WriteLineAsync($"Account kind:\t{accountInfo.Value.AccountKind}");
        await Console.Out.WriteLineAsync($"Account sku:\t{accountInfo.Value.SkuName}");
    }

    public Task SetBlobPropertiesAsync(string containerName, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task GetBlobPropertiesAsync(string containerName, string fileName)
    {
        throw new NotImplementedException();
    }
}
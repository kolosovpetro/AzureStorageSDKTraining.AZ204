using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace AzureStorageSDKTraining.AZ204.ConsoleApp;

public static class Program
{
    private static readonly IStorageClient StorageClient = new StorageClient();

    private const string DefaultContainer = "containerpkolosov";
    private const string DefaultBlob = "01_develop_azure_compute_solutions.PNG";

    public static async Task Main()
    {
        while (true)
        {
            DisplayMainMenu();
            var item = Console.ReadLine();

            if (item == "END")
            {
                Console.WriteLine("Bye bye!");
                break;
            }

            switch (item)
            {
                case "1":
                    await UploadAllFilesFromTestFolderAsync();
                    break;
                case "2":
                    await ListAllFilesInsideContainerAsync();
                    break;
                case "3":
                    await DownloadAllFilesLocallyAsync();
                    break;
                case "4":
                    await UpdateBlobMetadataAsync();
                    break;
                case "5":
                    await PrintBlobMetadataAsync();
                    break;
                case "6":
                    await PrintStorageAccountDetails();
                    break;
                case "7":
                    await SetBlobPropertiesAsync();
                    break;
                case "8":
                    await GetBlobPropertiesAsync();
                    break;
                default:
                    Console.Clear();
                    DisplayMainMenu();
                    break;
            }
        }
    }

    private static async Task GetBlobPropertiesAsync()
    {
        Console.Clear();

        var container = ReadContainerNameFromConsole();

        Console.WriteLine("Enter blob name: ");

        var blobName = ReadBlobNameFromConsole();

        var client = StorageClient.GetContainerClient(container);

        var blob = client.GetBlobClient(blobName);

        var exists = await blob.ExistsAsync();

        if (!exists)
        {
            Console.WriteLine($"Blob does not exist {blobName}");
            PressAnyKeyToContinue();
            return;
        }

        BlobProperties properties = await blob.GetPropertiesAsync();

        Console.WriteLine($"Getting properties to the blob {blobName} ...");

        Console.WriteLine($" ContentLanguage: {properties.ContentLanguage}");
        Console.WriteLine($" ContentType: {properties.ContentType}");
        Console.WriteLine($" CreatedOn: {properties.CreatedOn}");
        Console.WriteLine($" LastModified: {properties.LastModified}]");

        Console.WriteLine("\nSuccess.");

        PressAnyKeyToContinue();
    }

    private static async Task SetBlobPropertiesAsync()
    {
        Console.Clear();

        var container = ReadContainerNameFromConsole();

        Console.WriteLine("Enter blob name: ");

        var blobName = ReadBlobNameFromConsole();

        var client = StorageClient.GetContainerClient(container);

        var blob = client.GetBlobClient(blobName);

        var exists = await blob.ExistsAsync();

        if (!exists)
        {
            Console.WriteLine($"Blob does not exist {blobName}");
            PressAnyKeyToContinue();
            return;
        }

        BlobProperties properties = await blob.GetPropertiesAsync();

        var headers = new BlobHttpHeaders
        {
            ContentType = "text/plain",
            ContentLanguage = "en-us",

            CacheControl = properties.CacheControl,
            ContentDisposition = properties.ContentDisposition,
            ContentEncoding = properties.ContentEncoding,
            ContentHash = properties.ContentHash
        };

        Console.WriteLine($"Setting properties to the blob {blobName} ...");
        await blob.SetHttpHeadersAsync(headers);
        Console.WriteLine("Success.");

        PressAnyKeyToContinue();
    }

    private static async Task PrintStorageAccountDetails()
    {
        Console.Clear();

        await StorageClient.PrintAccountDetailsAsync();

        PressAnyKeyToContinue();
    }

    private static async Task PrintBlobMetadataAsync()
    {
        Console.Clear();

        var container = ReadContainerNameFromConsole();

        Console.WriteLine("Enter blob name: ");

        var blobName = ReadBlobNameFromConsole();

        var client = StorageClient.GetContainerClient(container);

        var blob = client.GetBlobClient(blobName);

        var exists = await blob.ExistsAsync();

        if (!exists)
        {
            Console.WriteLine($"Blob does not exist {blobName}");
            PressAnyKeyToContinue();
            return;
        }

        var meta = await blob.GetPropertiesAsync();
        StorageClient.PrintDictionaryContents(meta.Value.Metadata);

        PressAnyKeyToContinue();
    }

    private static async Task UpdateBlobMetadataAsync()
    {
        Console.Clear();

        var container = ReadContainerNameFromConsole();

        Console.WriteLine("Enter blob name: ");

        var blobName = ReadBlobNameFromConsole();

        var client = StorageClient.GetContainerClient(container);

        var blob = client.GetBlobClient(blobName);

        var exists = await blob.ExistsAsync();

        if (!exists)
        {
            Console.WriteLine($"Blob does not exist {blobName}");
            PressAnyKeyToContinue();
            return;
        }

        Console.WriteLine($"Setting metadata to blob {blobName} ...");
        var meta = TestFilesHelper.BlobMetaData;
        await blob.SetMetadataAsync(meta);

        PressAnyKeyToContinue();
    }

    private static void PressAnyKeyToContinue()
    {
        Console.WriteLine("Press any key to continue ...");
        Console.ReadKey();
        Console.Clear();
    }

    private static string ReadBlobNameFromConsole()
    {
        var blobName = Console.ReadLine();

        if (string.IsNullOrEmpty(blobName))
        {
            Console.WriteLine($"Blob value is not provided, retain default: {DefaultBlob}");

            blobName = DefaultBlob;
        }

        return blobName;
    }

    private static async Task DownloadAllFilesLocallyAsync()
    {
        Console.Clear();

        var container = ReadContainerNameFromConsole();

        var fileList = TestFilesHelper.TestFileNameList;

        foreach (var fileName in fileList)
        {
            Console.WriteLine($"Downloading file {fileName} ...");
            var stream = await StorageClient.DownloadToStreamAsync(container, fileName);
            await using var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        }

        PressAnyKeyToContinue();
    }

    private static async Task ListAllFilesInsideContainerAsync()
    {
        Console.Clear();

        var container = ReadContainerNameFromConsole();

        var blobList = await StorageClient.ListBlobsInsideContainerAsync(container);

        if (blobList.Count == 0)
        {
            Console.WriteLine($"Container {container} does not contain any blobs.");
            PressAnyKeyToContinue();

            return;
        }

        Console.WriteLine("Container contents:");

        foreach (var blob in blobList)
        {
            Console.WriteLine($"  -- {blob}");
        }

        PressAnyKeyToContinue();
    }

    private static async Task UploadAllFilesFromTestFolderAsync()
    {
        Console.Clear();
        var container = ReadContainerNameFromConsole();

        var containerClient = StorageClient.GetContainerClient(container);
        var filesList = TestFilesHelper.TestFileNameList;

        foreach (var fileName in filesList)
        {
            Console.WriteLine($"Uploading file {fileName} ...");
            const string testFolderPath = TestFilesHelper.TestFilesPath;

            var path = Path.Combine(testFolderPath, fileName);

            var blob = containerClient.GetBlobClient(fileName);

            var exists = await blob.ExistsAsync();

            if (exists)
            {
                Console.WriteLine($"Blob with name {fileName} already exists, skipping ...");
                continue;
            }

            var stream = StorageClient.ReadFileFromFileSystem(path);

            await blob.UploadAsync(stream);
        }

        PressAnyKeyToContinue();
    }

    private static string ReadContainerNameFromConsole()
    {
        Console.WriteLine("Enter container name: ");

        var container = Console.ReadLine();

        if (string.IsNullOrEmpty(container))
        {
            Console.WriteLine($"Container is not specified, retain default value: {DefaultContainer}");
            container = DefaultContainer;
        }

        return container;
    }

    private static void DisplayMainMenu()
    {
        Console.WriteLine("1. Upload all files from test folder to Azure Storage");
        Console.WriteLine("2. List all files inside container");
        Console.WriteLine("3. Download all files from container locally (with path)");
        Console.WriteLine("4. Update blob metadata (with params: container, blob name)");
        Console.WriteLine("5. Print blob metadata (with params: container, blob name)");
        Console.WriteLine("6. Print storage account details");
        Console.WriteLine("7. Set blob properties (with params: container, blob name)");
        Console.WriteLine("8. Get blob properties (with params: container, blob name)");
        Console.WriteLine("END. To quit");
    }
}
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureStorageSDKTraining.AZ204.ConsoleApp;

public static class Program
{
    private static readonly IStorageClient StorageClient = new StorageClient();

    private const string DefaultContainer = "containerpkolosov";

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
                    Console.Clear();
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
        throw new NotImplementedException();
    }

    private static async Task SetBlobPropertiesAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task PrintStorageAccountDetails()
    {
        throw new NotImplementedException();
    }

    private static async Task PrintBlobMetadataAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task UpdateBlobMetadataAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task DownloadAllFilesLocallyAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task ListAllFilesInsideContainerAsync()
    {
        throw new NotImplementedException();
    }

    private static async Task UploadAllFilesFromTestFolderAsync()
    {
        Console.WriteLine("Enter container name: ");

        var container = Console.ReadLine();

        if (string.IsNullOrEmpty(container))
        {
            Console.WriteLine($"Container is not specified, retain default value: {DefaultContainer}");
            container = DefaultContainer;
        }

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

        Console.WriteLine("Press any key to continue ...");
        Console.ReadKey();
        Console.Clear();
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
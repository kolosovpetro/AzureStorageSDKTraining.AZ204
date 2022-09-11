using System.Collections.Generic;

namespace AzureStorageSDKTraining.AZ204;

public static class TestFilesHelper
{
    private const string TestFilesDir = "TestFiles";
    public const string TestFilesPath = $"../../../../{TestFilesDir}";

    public static List<string> TestFileNameList => new()
    {
        "01_develop_azure_compute_solutions.PNG",
        "02_implement_azure_security.PNG",
        "03_connect_and_consume_services.PNG",
        "04_implement_cosmos_storage_solutions.PNG",
        "05_monitor_and_troubleshoot.PNG",
        "06_guides_to_watch.PNG",
        "07_skill_iq_quizes.PNG",
    };

    public static Dictionary<string, string> BlobMetaData => new()
    {
        { "author", "pkolosov" },
        { "scope", "az204preparation" },
        { "topsecret", "true" },
    };
}
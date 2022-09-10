# Practice with Azure Storage SDK

[![Run Build and Test](https://github.com/kolosovpetro/AzureStorageSDKTraining.AZ204/actions/workflows/run-build-and-test-dotnet.yml/badge.svg)](https://github.com/kolosovpetro/AzureStorageSDKTraining.AZ204/actions/workflows/run-build-and-test-dotnet.yml)

Just repo where I practice interaction with Azure Storage SDK such as upload, download, change metadata, access
policies (SAS) etc.

## Infrastructure provisioning

- Create resource group: `az group create --name "storage-training-rg" --location "centralus"`
- Crate storage
  account: `az storage account create --name "storagepkolosov" --resource-group "storage-training-rg" --location "centralus" --sku "Standard_ZRS" --kind "StorageV2"`
- Create
  container: `az storage container create --name "containerpkolosov" --account-name "storagepkolosov" --public-access "off"`

## Azure storage SDK entities

- `BlobServiceClient` - to manage azure storage account
- `BlobContainerClient` - to manage azure storage container
- `BlobClient` - to manage particular blob (file)

## Required packages

- `dotnet add package Azure.Storage.Blobs`


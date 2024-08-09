using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System.Collections.Generic;

return await Pulumi.Deployment.RunAsync(() =>
{
    var config = new Pulumi.Config();
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("resourceGroup", new ResourceGroupArgs(){
        ResourceGroupName= $"operator-demo-{config.Require("prefix")}"
    } );

    // Create an Azure resource (Storage Account)
    var storageAccount = new StorageAccount("sa", new StorageAccountArgs
    {
        AccountName = $"opdemosc{config.Require("prefix")}",
        ResourceGroupName = resourceGroup.Name,
        Sku = new SkuArgs
        {
            Name = SkuName.Standard_LRS
        },
        Kind = Kind.StorageV2
    });

    // var storageAccountKeys = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
    // {
    //     ResourceGroupName = resourceGroup.Name,
    //     AccountName = storageAccount.Name
    // });

    // var primaryStorageKey = storageAccountKeys.Apply(accountKeys =>
    // {
    //     var firstKey = accountKeys.Keys[0].Value;
    //     return Output.CreateSecret(firstKey);
    // });

  var virtualNetwork = new Pulumi.AzureNative.Network.VirtualNetwork("virtualNetwork", new()
    {
        AddressSpace = new Pulumi.AzureNative.Network.Inputs.AddressSpaceArgs
        {
            AddressPrefixes = new[]
            {
                "10.0.0.0/16",
            },
        },
        FlowTimeoutInMinutes = 10,
        ResourceGroupName = resourceGroup.Name,
        VirtualNetworkName = $"{config.Require("prefix")}-vnet",
    });
    //   // Export the primary key of the Storage Account
    // return new Dictionary<string, object?>
    // {
    //     ["primaryStorageKey"] = primaryStorageKey
    // };
});
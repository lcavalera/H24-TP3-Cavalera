param location string = resourceGroup().location
@minLength(3)
@maxLength(5)
param appName string
@allowed([
  'java'
  'dotnet-isolated'
])
param runtime string

resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: 'st${appName}${uniqueString(resourceGroup().id)}'
  location: location
  sku:{
    name: 'Standard_ZRS'
  }
  kind: 'StorageV2'
}

resource appFuncServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: 'plan-${appName}'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource appFunction 'Microsoft.Web/sites@2023-01-01' = {
  name: 'func-${appName}-${uniqueString(resourceGroup().id)}'
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: appFuncServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storage.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storage.listKeys().keys[0].value}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: runtime
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
      ]
    }
  }
}

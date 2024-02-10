param location string = resourceGroup().location
param storageName string
param containerName string

// Creation du storage
resource storageAccounts 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageName
  location: location
  sku: {
    name: 'Standard_GRS'
  }
  kind: 'StorageV2'
}

// Creation du blob service
resource blobservice 'Microsoft.Storage/storageAccounts/blobServices@2023-01-01' = {
 name: 'default'
  parent: storageAccounts
   properties:{
    deleteRetentionPolicy:{
      enabled: true
      allowPermanentDelete: false
      days: 7
    }
    containerDeleteRetentionPolicy:{
      enabled: true
      allowPermanentDelete: false
      days: 7
    }
   }
}
// Creation du container
resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  name: containerName
  parent: blobservice
  properties: {
    publicAccess: 'None'
    metadata: {}
  }
}

param location string = resourceGroup().location
param serverName string
param dataBases array
param dbUser string
@minLength(10)
@maxLength(20)
@secure()
param dbPassword string

// Creation du serveur
resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  location: location
  name: 'srv-${serverName}'
  properties: {
    publicNetworkAccess: 'Enabled'
    administratorLogin: dbUser
    administratorLoginPassword: dbPassword
    version: '12.0'
  }
  tags:{
    Administrateur: dbUser
  }
}

// Creation des régles du parafeu
resource sqlFireWallRules 'Microsoft.Sql/servers/firewallRules@2022-05-01-preview' = {
   name: 'AllowAllIps' 
   parent: sqlServer
   properties: { 
    endIpAddress: '255.255.255.255' 
    startIpAddress: '0.0.0.0' 
  } 
}

// Creation des databases
resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = [for dataBase in dataBases: {
  location: location
  name: 'db-${dataBase.name}'
  parent: sqlServer
  sku: {
    name: dataBase.sku
    tier: dataBase.sku
  }
  tags:{
    Application: dataBase.name
  }
}]

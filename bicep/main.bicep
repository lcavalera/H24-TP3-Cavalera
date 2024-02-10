@allowed([
  'CanadaCentral'
  'CanadaEast'
])
param location string
param serverName string
param dbUser string
@minLength(10)
@maxLength(20)
@secure()
param dbPassword string 
@allowed([
  'Standard'
  'Basic'
])
param sku string = 'Standard'
param storageName string

//Definition des noms de les apps à créer
var appNames = [
  'postulations'
  'documents'
  'emplois'
  'favoris'
  'mvc']

//Creation des objects pour chaque app à créer
  var apps = [for appName in appNames: {
    name: appName
    isProd: appName == 'mvc' ? true : false
  }]

  //Definition des noms des databases à créer
var dbNames = [
  'postulations'
  'emplois']

//Creation des objects pour chaque db à créer
var dataBases = [for dbName in dbNames: {
  name: dbName
  sku: dbName == 'emplois' ? 'Basic' : sku
}]

  //Creation des apps en utilisant les modules
  module appsService 'modules/appService.bicep' = {
    name: 'appsService'
    params: {
      location: location
      apps: apps
    }
  }

  //Creation des databases en utilisant les modules
  module dbService 'modules/sqldatabase.bicep' = {
    name: 'dataBases'
    params: {
      location: location
      serverName: serverName
      dataBases: dataBases
      dbUser: dbUser
      dbPassword: dbPassword
    }
  }

  //Creation du compte de stockage en utilisant les modules
  module storageBlobService 'modules/storageBlobs.bicep' = {
    name: 'blobService'
    params: {
      location: location
      storageName: storageName
      containerName: 'documents'
    }
  }

param location string = resourceGroup().location
param sku string = 'F1'

var appNames =[
   'mvc'
   'postulations'
   'emplois'
   'documents'
   'favoris'
]


module AppServices 'modules/appService.bicep' =  {
  name: 'appService'
  params: {
    appNames : appNames
    location: location
    sku: sku
  }
}

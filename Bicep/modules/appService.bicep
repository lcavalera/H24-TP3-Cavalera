param location string = resourceGroup().location
param apps array

// Creation des plans de service
resource servicePlan 'Microsoft.Web/serverfarms@2023-01-01' = [for app in apps: {
  location: location
  name: 'sp-${app.name}'
  sku:{
    name: app.name =='mvc' ? 'S1': app.name=='postulations' ? 'B1' : 'F1'
  }
  tags:{
    Application: app.name
  }
}]

// Creation des applications web
resource webApp 'Microsoft.Web/sites@2023-01-01' = [for i in range(0, length(apps)): {
  location:location
  name:'webapp-${apps[i].name}-${uniqueString(resourceGroup().id)}'
  properties:{
    serverFarmId:servicePlan[i].id
  }
  tags:{
    Application: apps[i].name
  }
}]

// Creation du staging
resource stagingSlot 'Microsoft.Web/sites/slots@2023-01-01' = [for i in range(0, length(apps)): if(apps[i].isProd == true) {
  location:location
  parent:webApp[i]
  name:'staging'
  properties:{
    serverFarmId:servicePlan[i].id
  }
  tags:{
    Application: apps[i].name
  }
}]

// Creation des régles de mise à l'echelle
resource scaleOutInRule 'Microsoft.Insights/autoscalesettings@2022-10-01' = [for i in range(0, length(apps)): if(apps[i].isProd == true) {
  name: servicePlan[i].name
  location: location
  properties: {
    enabled: true
    profiles: [
      {
        name: 'Scale out/in condition'
        capacity: {
          maximum: '3'
          default: '1'
          minimum: '1'
        }
        rules: [
          {
            scaleAction: {
              type: 'ChangeCount'
              direction: 'Increase'
              cooldown: 'PT5M'
              value: '1'
            }
            metricTrigger: {
              metricName: 'CpuPercentage'
              operator:  'GreaterThanOrEqual'
              timeAggregation: 'Average'
              threshold: 80
              metricResourceUri: servicePlan[i].id
              timeWindow: 'PT10M'
              timeGrain: 'PT1M'
              statistic: 'Average'
            }
          }
          {
            scaleAction: {
              type: 'ChangeCount'
              direction: 'Decrease'
              cooldown: 'PT5M'
              value: '1'
            }
            metricTrigger: {
              metricName: 'CpuPercentage'
              operator: 'LessThanOrEqual'
              timeAggregation: 'Average'
              threshold: 45
              metricResourceUri: servicePlan[i].id
              timeWindow: 'PT10M'
              timeGrain: 'PT1M'
              statistic: 'Average'
            }
          }
        ]
      }
    ]
    targetResourceUri: servicePlan[i].id
  }
  tags:{
    Application: apps[i].name
  }
}]

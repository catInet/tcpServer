<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="kotInternet" generation="1" functional="0" release="0" Id="3246480d-8ec4-4526-8630-2d159da9f529" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="kotInternetGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="tcpServer:listener" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/kotInternet/kotInternetGroup/LB:tcpServer:listener" />
          </inToChannel>
        </inPort>
        <inPort name="tcpServerPublish:listenerPublish" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/kotInternet/kotInternetGroup/LB:tcpServerPublish:listenerPublish" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="tcpServer:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/kotInternet/kotInternetGroup/MaptcpServer:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="tcpServer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/kotInternet/kotInternetGroup/MaptcpServer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="tcpServerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/kotInternet/kotInternetGroup/MaptcpServerInstances" />
          </maps>
        </aCS>
        <aCS name="tcpServerPublish:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/kotInternet/kotInternetGroup/MaptcpServerPublish:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="tcpServerPublish:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/kotInternet/kotInternetGroup/MaptcpServerPublish:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="tcpServerPublishInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/kotInternet/kotInternetGroup/MaptcpServerPublishInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:tcpServer:listener">
          <toPorts>
            <inPortMoniker name="/kotInternet/kotInternetGroup/tcpServer/listener" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:tcpServerPublish:listenerPublish">
          <toPorts>
            <inPortMoniker name="/kotInternet/kotInternetGroup/tcpServerPublish/listenerPublish" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MaptcpServer:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/kotInternet/kotInternetGroup/tcpServer/DataConnectionString" />
          </setting>
        </map>
        <map name="MaptcpServer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/kotInternet/kotInternetGroup/tcpServer/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MaptcpServerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/kotInternet/kotInternetGroup/tcpServerInstances" />
          </setting>
        </map>
        <map name="MaptcpServerPublish:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/kotInternet/kotInternetGroup/tcpServerPublish/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MaptcpServerPublish:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/kotInternet/kotInternetGroup/tcpServerPublish/StorageConnectionString" />
          </setting>
        </map>
        <map name="MaptcpServerPublishInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/kotInternet/kotInternetGroup/tcpServerPublishInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="tcpServer" generation="1" functional="0" release="0" software="C:\Users\Настя\Documents\Visual Studio 2013\Projects\kotInternet\kotInternet\csx\Release\roles\tcpServer" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="listener" protocol="tcp" portRanges="10100" />
            </componentports>
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;tcpServer&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;tcpServer&quot;&gt;&lt;e name=&quot;listener&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;tcpServerPublish&quot;&gt;&lt;e name=&quot;listenerPublish&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/kotInternet/kotInternetGroup/tcpServerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/kotInternet/kotInternetGroup/tcpServerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/kotInternet/kotInternetGroup/tcpServerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="tcpServerPublish" generation="1" functional="0" release="0" software="C:\Users\Настя\Documents\Visual Studio 2013\Projects\kotInternet\kotInternet\csx\Release\roles\tcpServerPublish" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="listenerPublish" protocol="tcp" portRanges="10101" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;tcpServerPublish&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;tcpServer&quot;&gt;&lt;e name=&quot;listener&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;tcpServerPublish&quot;&gt;&lt;e name=&quot;listenerPublish&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/kotInternet/kotInternetGroup/tcpServerPublishInstances" />
            <sCSPolicyUpdateDomainMoniker name="/kotInternet/kotInternetGroup/tcpServerPublishUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/kotInternet/kotInternetGroup/tcpServerPublishFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="tcpServerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="tcpServerPublishUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="tcpServerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="tcpServerPublishFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="tcpServerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="tcpServerPublishInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="a875900c-1e37-44b5-9d0b-38c1d10339c1" ref="Microsoft.RedDog.Contract\ServiceContract\kotInternetContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="df41646d-0725-468f-97b2-9139c4eeaf06" ref="Microsoft.RedDog.Contract\Interface\tcpServer:listener@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/kotInternet/kotInternetGroup/tcpServer:listener" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="9b3c7ea3-be66-4bf3-9e0e-a80ae3fad177" ref="Microsoft.RedDog.Contract\Interface\tcpServerPublish:listenerPublish@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/kotInternet/kotInternetGroup/tcpServerPublish:listenerPublish" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
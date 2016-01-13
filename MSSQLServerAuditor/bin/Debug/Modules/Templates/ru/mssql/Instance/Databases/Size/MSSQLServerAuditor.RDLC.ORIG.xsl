<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessor preprocessor="RDLPreprocessorDialog" name="RDLC">
		<Report Name="EmployeeReport" xmlns="http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition" xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner">
  <Body>
    <ReportItems>
      <Textbox Name="DatabaseID_Header">
        <Paragraphs>
          <Paragraph>
            <TextRuns>
              <TextRun>
                <Value>DatabaseID</Value>
                <Style>
                  <FontWeight>Bold</FontWeight>
                </Style>
              </TextRun>
            </TextRuns>
            <Style />
          </Paragraph>
        </Paragraphs>
        <Height>0.61862cm</Height>
        <Width>23.54792mm</Width>
        <Style />
      </Textbox>
      <Tablix Name="GetDatabaseSizeTable">
        <TablixBody>
          <TablixColumns>
            <TablixColumn>
              <Width>2.5cm</Width>
            </TablixColumn>
          </TablixColumns>
          <TablixRows>
            <TablixRow>
              <Height>0.6cm</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="Textbox1">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>Database ID</Value>
                              <Style />
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox1</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>
            <TablixRow>
              <Height>0.6cm</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="DatabaseID">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Fields!DatabaseID.Value</Value>
                              <Style />
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>DatabaseID</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
              </TablixCells>
            </TablixRow>
          </TablixRows>
        </TablixBody>
        <TablixColumnHierarchy>
          <TablixMembers>
            <TablixMember />
          </TablixMembers>
        </TablixColumnHierarchy>
        <TablixRowHierarchy>
          <TablixMembers>
            <TablixMember>
              <Visibility>
                <Hidden>true</Hidden>
              </Visibility>
              <KeepWithGroup>After</KeepWithGroup>
            </TablixMember>
            <TablixMember>
              <Group Name="Details" />
            </TablixMember>
          </TablixMembers>
        </TablixRowHierarchy>
        <DataSetName>GetDatabaseSize</DataSetName>
        <Top>0.01862cm</Top>
        <Left>2.53118cm</Left>
        <Height>1.2cm</Height>
        <Width>2.5cm</Width>
        <ZIndex>1</ZIndex>
        <Style>
          <Border>
            <Style>None</Style>
          </Border>
        </Style>
      </Tablix>
    </ReportItems>
    <Height>12.49841cm</Height>
    <Style>
      <Border>
        <Style>None</Style>
      </Border>
    </Style>
  </Body>
  <Width>194.32776mm</Width>
  <Page>
    <PageHeight>29.7cm</PageHeight>
    <PageWidth>21cm</PageWidth>
    <InteractiveHeight>11in</InteractiveHeight>
    <InteractiveWidth>8.5in</InteractiveWidth>
    <LeftMargin>2.5cm</LeftMargin>
    <RightMargin>2.5cm</RightMargin>
    <BottomMargin>2.5cm</BottomMargin>
    <ColumnSpacing>1cm</ColumnSpacing>
    <Style />
  </Page>
  <Description>GetDatabaseSize</Description>
  <Author>City24 Pty. Ltd.</Author>
  <AutoRefresh>0</AutoRefresh>
  <DataSources>
    <DataSource Name="DummyDataSource">
      <ConnectionProperties>
        <DataProvider>SQL</DataProvider>
        <ConnectString />
      </ConnectionProperties>
      <rd:DataSourceID>6a955a4e-a628-47bb-8d68-975d5c10d836</rd:DataSourceID>
    </DataSource>
  </DataSources>
  <DataSets>
    <DataSet Name="GetDatabaseSize" dataSetNumber="1">
      <Query>
        <DataSourceName>DummyDataSource</DataSourceName>
        <CommandText />
        <rd:UseGenericDesigner>true</rd:UseGenericDesigner>
      </Query>
      <Fields>
        <Field Name="DatabaseID">
          <DataField>DatabaseID</DataField>
          <rd:TypeName>System.String</rd:TypeName>
        </Field>
      </Fields>
      <rd:DataSetInfo>
        <rd:DataSetName>GetDatabase</rd:DataSetName>
        <rd:TableName>GetDatabaseSize</rd:TableName>
        <rd:TableAdapterFillMethod />
        <rd:TableAdapterGetDataMethod />
        <rd:TableAdapterName />
      </rd:DataSetInfo>
    </DataSet>
  </DataSets>
  <Language>en-US</Language>
  <ConsumeContainerWhitespace>true</ConsumeContainerWhitespace>
  <DataElementName>Report</DataElementName>
  <rd:ReportUnitType>Cm</rd:ReportUnitType>
  <rd:ReportID>5d8f6d26-e84e-4862-a68a-158814b91bfd</rd:ReportID>
</Report>
	</mssqlauditorpreprocessor>
</root>

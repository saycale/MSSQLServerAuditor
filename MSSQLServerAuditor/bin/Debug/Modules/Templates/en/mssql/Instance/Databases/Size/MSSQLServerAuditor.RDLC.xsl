<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="РДЛЦ отчёты" id="JRDLC_DataBaseSizes" columns="100" rows="100" splitter="yes">
                <mssqlauditorpreprocessor preprocessor="RDLPreprocessorDialog" name="RDLC" id="RDLC_DataBaseSize">
		<Report Name="DatabaseSizeReport" xmlns="http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition" xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner">

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
							<rd:TypeName>System.Int32</rd:TypeName>
						</Field>
						<Field Name="DatabaseName">
							<DataField>DatabaseName</DataField>
							<rd:TypeName>System.String</rd:TypeName>
						</Field>
						<Field Name="DatabaseSizeMB">
							<DataField>DatabaseSizeMB</DataField>
							<rd:TypeName>System.Int32</rd:TypeName>
						</Field>
						<Field Name="DatabaseDataSizeMB">
							<DataField>DatabaseDataSizeMB</DataField>
							<rd:TypeName>System.Int32</rd:TypeName>
						</Field>
						<Field Name="DatabaseLogSizeMB">
							<DataField>DatabaseLogSizeMB</DataField>
							<rd:TypeName>System.Int32</rd:TypeName>
						</Field>
					</Fields>

					<rd:DataSetInfo>
						<rd:DataSetName>GetDatabaseSize</rd:DataSetName>
						<rd:TableName>GetDatabaseSize</rd:TableName>
						<rd:TableAdapterFillMethod />
						<rd:TableAdapterGetDataMethod />
						<rd:TableAdapterName />
					</rd:DataSetInfo>
				</DataSet>
			</DataSets>

  <Body>
    <ReportItems>
      <Tablix Name="table1">
        <TablixBody>
          <TablixColumns>
            <TablixColumn>
              <Width>2.14323in</Width>
            </TablixColumn>
            <TablixColumn>
              <Width>2.25781in</Width>
            </TablixColumn>
            <TablixColumn>
              <Width>1.59115in</Width>
            </TablixColumn>
            <TablixColumn>
              <Width>1.59115in</Width>
            </TablixColumn>
          </TablixColumns>
          <TablixRows>
            <TablixRow>
              <Height>0.23333in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="textbox2">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>Database Name</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <FontSize>11pt</FontSize>
                                <FontWeight>Bold</FontWeight>
                                <Color>White</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>textbox2</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <BackgroundColor>SteelBlue</BackgroundColor>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="textbox3">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>Database Size MB</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <FontSize>11pt</FontSize>
                                <FontWeight>Bold</FontWeight>
                                <Color>White</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>textbox3</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <BackgroundColor>SteelBlue</BackgroundColor>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="Textbox4">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>Database Data Size MB</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <FontSize>11pt</FontSize>
                                <FontWeight>Bold</FontWeight>
                                <Color>White</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox4</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <BackgroundColor>SteelBlue</BackgroundColor>
                        <PaddingLeft>2pt</PaddingLeft>
                        <PaddingRight>2pt</PaddingRight>
                        <PaddingTop>2pt</PaddingTop>
                        <PaddingBottom>2pt</PaddingBottom>
                      </Style>
                    </Textbox>
                  </CellContents>
                </TablixCell>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="Textbox6">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>Database Log Size MB</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <FontSize>11pt</FontSize>
                                <FontWeight>Bold</FontWeight>
                                <Color>White</Color>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style>
                            <TextAlign>Right</TextAlign>
                          </Style>
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>Textbox6</rd:DefaultName>
                      <Style>
                        <Border>
                          <Color>LightGrey</Color>
                          <Style>Solid</Style>
                        </Border>
                        <BackgroundColor>SteelBlue</BackgroundColor>
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
              <Height>0.22333in</Height>
              <TablixCells>
                <TablixCell>
                  <CellContents>
                    <Textbox Name="DatabaseName">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Fields!DatabaseName.Value</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>DatabaseName</rd:DefaultName>
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
                <TablixCell>
                  <CellContents>
                    <Textbox Name="DatabaseSizeMB">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Fields!DatabaseSizeMB.Value</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <Format>#,###.00</Format>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>DatabaseSizeMB</rd:DefaultName>
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
                <TablixCell>
                  <CellContents>
                    <Textbox Name="DatabaseDataSizeMB">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Fields!DatabaseDataSizeMB.Value</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <Format>#,###.00</Format>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>DatabaseDataSizeMB</rd:DefaultName>
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
                <TablixCell>
                  <CellContents>
                    <Textbox Name="DatabaseLogSizeMB">
                      <CanGrow>true</CanGrow>
                      <KeepTogether>true</KeepTogether>
                      <Paragraphs>
                        <Paragraph>
                          <TextRuns>
                            <TextRun>
                              <Value>=Fields!DatabaseLogSizeMB.Value</Value>
                              <Style>
                                <FontFamily>Tahoma</FontFamily>
                                <Format>#,###.00</Format>
                              </Style>
                            </TextRun>
                          </TextRuns>
                          <Style />
                        </Paragraph>
                      </Paragraphs>
                      <rd:DefaultName>DatabaseLogSizeMB</rd:DefaultName>
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
            <TablixMember />
            <TablixMember />
            <TablixMember />
          </TablixMembers>
        </TablixColumnHierarchy>
        <TablixRowHierarchy>
          <TablixMembers>
            <TablixMember>
              <KeepWithGroup>After</KeepWithGroup>
              <RepeatOnNewPage>true</RepeatOnNewPage>
              <KeepTogether>true</KeepTogether>
            </TablixMember>
            <TablixMember>
              <Group Name="table1_Details_Group">
                <DataElementName>Detail</DataElementName>
              </Group>
              <TablixMembers>
                <TablixMember />
              </TablixMembers>
              <DataElementName>Detail_Collection</DataElementName>
              <DataElementOutput>Output</DataElementOutput>
              <KeepTogether>true</KeepTogether>
            </TablixMember>
          </TablixMembers>
        </TablixRowHierarchy>
        <DataSetName>GetDatabaseSize</DataSetName>
        <Height>0.45667in</Height>
        <Width>7.58333in</Width>
        <Style />
      </Tablix>
    </ReportItems>
    <Height>0.45667in</Height>
    <Style />
  </Body>

			<Width>20.0cm</Width>

			<Page>
				<PageHeight>30.0cm</PageHeight>
				<PageWidth>20.0cm</PageWidth>
				<InteractiveHeight>11in</InteractiveHeight>
				<InteractiveWidth>8.5in</InteractiveWidth>
				<LeftMargin>0.25cm</LeftMargin>
				<RightMargin>0.25cm</RightMargin>
				<BottomMargin>0.25cm</BottomMargin>
				<ColumnSpacing>0.2cm</ColumnSpacing>
				<Style />
			</Page>

			<Language>ru-RU</Language>
			<ConsumeContainerWhitespace>true</ConsumeContainerWhitespace>
			<DataElementName>Report</DataElementName>
			<rd:ReportUnitType>Cm</rd:ReportUnitType>
			<rd:ReportID>5d8f6d26-e84e-4862-a68a-158814b91bfd</rd:ReportID>

		</Report>
	</mssqlauditorpreprocessor>
       </mssqlauditorpreprocessors>

</root>

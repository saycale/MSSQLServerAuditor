<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessors name="Колонки" id="StackedColumn.Graph.en" columns="100" rows="100" splitter="yes">
		<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="" id="StackedColumn.Graph.en" column="1" row="1" colspan="1" rowspan="1">
			<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
				<FirstWeekDay>
					Monday
				</FirstWeekDay>

				<AxisXConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<AxisXLabelFilter>
						All
					</AxisXLabelFilter>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
						<LineWidth>
							1
						</LineWidth>
					</MinorGrid>
				</AxisXConfiguration>

				<AxisYConfiguration>
					<ShowLabels>
						true
					</ShowLabels>
					<MajorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MajorGrid>
					<MinorGrid>
						<Enabled>
							true
						</Enabled>
						<LineWidth>
							1
						</LineWidth>
						<LineColor>
							<Name>#848482</Name>
						</LineColor>
					</MinorGrid>
					<Format>
						#.##
					</Format>
					<Name>
						%
					</Name>
				</AxisYConfiguration>

				<GraphSource xsi:type="XmlFileGraphSource">
					<FileName>$INPUT$</FileName>
					<PathToItems>/MSSQLResults/MSSQLResult[@name='GetStackedColumnGraph' and @hierarchy='']/RecordSet[@id='1']</PathToItems>
					<ItemTag>Row</ItemTag>
					<NameTag>Name</NameTag>
					<ValueTag>PointValue1</ValueTag>
					<ValueTag>PointValue2</ValueTag>
					<ValueTag>PointValue3</ValueTag>
					<ValueGroup>Day</ValueGroup>
				</GraphSource>

				<ItemsConfiguration>
					<NameSortType>
						NameAscending
					</NameSortType>
					<Unit>
						Byte
					</Unit>
				</ItemsConfiguration>

				<LegendConfiguration>
					<Enabled>
						true
					</Enabled>
					<Docking>
						Top
					</Docking>
					<Alignment>
						Center
					</Alignment>
					<GraphicOverlap>
						true
					</GraphicOverlap>
				</LegendConfiguration>

				<GraphType>
					StackedColumn
				</GraphType>

			</GraphConfiguration>
		</mssqlauditorpreprocessor>
	</mssqlauditorpreprocessors>
</root>

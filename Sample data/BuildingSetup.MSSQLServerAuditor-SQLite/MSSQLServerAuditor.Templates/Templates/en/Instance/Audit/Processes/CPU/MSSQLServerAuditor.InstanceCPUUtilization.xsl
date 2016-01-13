<?xml version="1.0" encoding="UTF-8"?>
<root>
	<mssqlauditorpreprocessor preprocessor="GraphPreprocessorDialog" name="Instance CPU Utilization" id="InstanceCPUUtilization.Graph.en">
		<GraphConfiguration xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
			<FirstWeekDay>
				Monday
			</FirstWeekDay>

			<AxisXConfiguration>
				<AxisXLabelFilter>
					AllDays
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
				<PathToItems>MSSQLResults</PathToItems>
				<ItemTag>GetInstanceCPUUtilization</ItemTag>
				<DateTag>EventTime</DateTag>
				<NameTag></NameTag>
				<ValueTag>SQLProcessCPUUtilization</ValueTag>
				<ValueTag>OtherProcessesCPUUtilization</ValueTag>
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
				<GraphicOverlap>
					true
				</GraphicOverlap>
				<Docking>
					Top
				</Docking>
				<Enabled>
					true
				</Enabled>
				<GraphicOverlap>
					true
				</GraphicOverlap>
			</LegendConfiguration>

			<Size>
				<Height>100%</Height>
				<Width>100%</Width>
			</Size>

			<GraphType>
				Line
			</GraphType>

		</GraphConfiguration>
	</mssqlauditorpreprocessor>
</root>

